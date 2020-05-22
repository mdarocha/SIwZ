module Page.Search exposing (Model, Msg, init, update, view)

import Bootstrap.Button as Button
import Bootstrap.Form.Input as Input
import Bootstrap.Form.InputGroup as InputGroup
import Bootstrap.Grid as Grid
import Bootstrap.Grid.Col as Col
import Bootstrap.Spinner as Spinner
import Bootstrap.Card as Card
import Bootstrap.Card.Block as Block
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (..)
import Http
import Json.Decode as Decode
import Json.Encode as Encode
import Session
import Set
import Skeleton
import Url.Builder as UrlBuilder
import Time
import Iso8601 as TimeIso

type alias AutocompleteStop =
    { id : Int
    , name : String
    , city : String
    }


type AutocompleteStops
    = Failure
    | Loading
    | Success (List AutocompleteStop)

type alias RideStop =
    { id : Int
    , arrivalTime : Time.Posix
    , stopNumber : Int
    , name : String
    , city : String
    }

type alias RideTrain =
    { id : Int
    , name : String
    , trainType : Int
    , seats : Int
    , wagons : Int
    }

type alias Ride =
    { id : Int
    , from : Int
    , to : Int
    , stops : List RideStop
    , startTime : Time.Posix
    , train : RideTrain
    , freeTickets : Int
    , price : Int
    }

type Rides
    = NotStarted
    | RidesFailure
    | RidesLoading
    | RidesSuccess (List Ride)

type alias SearchBoxState =
    { text : String
    , selected : Maybe AutocompleteStop
    , showSuggestions : Bool
    , suggestions : AutocompleteStops
    }


type SearchBoxMsg
    = SearchTextUpdate String
    | ShowSuggestions
    | HideSuggestions
    | SuggestionSelected AutocompleteStop
    | ClearInput


type alias Model =
    { session : Session.Data
    , routeFromSearch : SearchBoxState
    , routeToSearch : SearchBoxState
    , departureTime : String
    , rides : Rides
    }


type Msg
    = RouteFromUpdate SearchBoxMsg
    | RouteToUpdate SearchBoxMsg
    | GotStopsList (Result Http.Error (List AutocompleteStop))
    | GotRides (Result Http.Error (List Ride))
    | SubmitSearch
    | DepartureTimeUpdate String


-- INIT


init : Session.Data -> ( Model, Cmd Msg )
init session =
    let
        newSearchBox =
            SearchBoxState "" Nothing False Loading
    in
    ( Model session newSearchBox newSearchBox "" NotStarted, getStopsList session.api )



-- UPDATE


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        RouteFromUpdate searchMsg ->
            let
                ( newState, cmds ) =
                    updateSearchBox searchMsg model.routeFromSearch
                rides = case newState.selected of
                    Nothing -> NotStarted
                    _ -> model.rides
            in
            ( { model | routeFromSearch = newState, rides = rides }, Cmd.map RouteFromUpdate cmds )

        RouteToUpdate searchMsg ->
            let
                ( newState, cmds ) =
                    updateSearchBox searchMsg model.routeToSearch
                rides = case newState.selected of
                    Nothing -> NotStarted
                    _ -> model.rides
            in
            ( { model | routeToSearch = newState, rides = rides }, Cmd.map RouteToUpdate cmds )

        GotStopsList result ->
            case result of
                Ok stops ->
                    let
                        oldFromState = model.routeFromSearch
                        oldToState = model.routeToSearch

                        newFromState = { oldFromState | suggestions = Success stops }
                        newToState = { oldToState | suggestions = Success stops }
                    in
                        ( { model |  routeToSearch = newToState, routeFromSearch = newFromState }, Cmd.none )

                Err _ ->
                    let
                        oldFromState = model.routeFromSearch
                        oldToState = model.routeToSearch

                        newFromState = { oldFromState | suggestions = Failure }
                        newToState = { oldToState | suggestions = Failure }
                    in
                        ( { model |  routeToSearch = newToState, routeFromSearch = newFromState }, Cmd.none )

        SubmitSearch ->
            case ( model.routeFromSearch.selected, model.routeToSearch.selected ) of
                (Just from, Just to) ->
                    ( { model | rides = RidesLoading }, getRides model.session.api from to )
                (_, _) ->
                    ( model, Cmd.none )

        DepartureTimeUpdate text ->
            ( { model | departureTime = text }, Cmd.none )

        GotRides result ->
            case result of
                Ok rides ->
                    ( { model | rides = RidesSuccess rides }, Cmd.none )

                Err _ ->
                    ( { model | rides = RidesFailure }, Cmd.none )

updateSearchBox : SearchBoxMsg -> SearchBoxState -> ( SearchBoxState, Cmd SearchBoxMsg )
updateSearchBox msg model =
    case msg of
        SearchTextUpdate text ->
            ( { model | text = text, showSuggestions = True }, Cmd.none )

        ShowSuggestions ->
            ( { model | showSuggestions = True }, Cmd.none )

        HideSuggestions ->
            ( { model | showSuggestions = False }, Cmd.none )

        SuggestionSelected stop ->
            ( { model | text = "", selected = Just stop }, Cmd.none )

        ClearInput ->
            ( { model | selected = Nothing, text = "" }, Cmd.none )



-- UTIL


performSearch : List AutocompleteStop -> String -> List AutocompleteStop
performSearch suggestions input =
    let
        inputWords =
            List.map String.toLower <| String.words input

        matchesAllWords item =
            let
                matchesWord word =
                    String.contains word (String.toLower item.city)
                        || String.contains word (String.toLower item.name)
            in
            List.all matchesWord inputWords
    in
    List.filter matchesAllWords suggestions


stopToString : AutocompleteStop -> String
stopToString stop =
    stop.city ++ " - " ++ stop.name

-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    { title = "Znajdź pociąg"
    , body =
        [ Grid.row []
            [ Grid.col [ Col.md6, Col.attrs [ class "mt-4" ] ] [ Html.map RouteFromUpdate <| viewSearchBox "Od" model.routeFromSearch ]
            , Grid.col [ Col.md6, Col.attrs [ class "mt-4" ] ] [ Html.map RouteToUpdate <| viewSearchBox "Do" model.routeToSearch ]
            ]
        , Grid.row []
            [ Grid.col [ Col.md6, Col.attrs [ class "mt-4" ] ] [ Input.datetimeLocal [ Input.attrs [ onInput DepartureTimeUpdate ] ] ]
            , Grid.col [ Col.md3, Col.offsetMd3, Col.attrs [ class "mt-4" ] ] [ Button.button [ Button.attrs [ onClick SubmitSearch ], Button.primary, Button.large, Button.block] [ text "Szukaj" ] ]
            ]
        , div [ class "mt-4" ] [ viewRides model.rides ]
        ]
    }


viewSearchBox : String -> SearchBoxState -> Html SearchBoxMsg
viewSearchBox label state =
    let
        suggestionItem =
            \s -> li [ class "dropdown-item", onMouseDown <| SuggestionSelected s ] [ text (stopToString s) ]
    in
    div [ class "search-box" ]
        [ case state.selected of
            Just stop ->
                InputGroup.config
                    (InputGroup.text [ Input.value (stopToString stop), Input.readonly True ])
                    |> InputGroup.successors [ InputGroup.button [ Button.attrs [ onClick ClearInput ], Button.danger ] [ text "X" ] ]
                    |> InputGroup.view

            Nothing ->
                Input.text [ Input.attrs [ onInput SearchTextUpdate, onFocus ShowSuggestions, onBlur HideSuggestions ], Input.value state.text, Input.placeholder label ]
        , if state.showSuggestions then
            case state.suggestions of
                Success stops ->
                    let
                        items =
                            performSearch stops state.text |> List.map suggestionItem
                    in
                    if List.length items > 0 then
                        ul [ class "dropdown-menu" ] items

                    else
                        div [] []

                Loading ->
                    div [] []

                Failure ->
                    div [ class "error" ] [ text "Błąd ładowania podpowiedzi - spróbuj odswieżyć stronę" ]

          else
            div [] []
        ]


viewRides : Rides -> Html Msg
viewRides rides =
    case rides of
        RidesLoading ->
            div [ class "text-center" ] [ Spinner.spinner [] [] ]

        RidesFailure ->
            h5 [ class "text-center" ] [ text "Wystąpił błąd" ]

        RidesSuccess rideList ->
            if List.length rideList == 0 then
                h4 [ class "text-center" ] [ text "Nie znaleziono pociągów" ]
            else
                div [] (List.map viewRide rideList)

        NotStarted ->
            div [ class "text-center" ] [ text "Rozpocznij wyszukiwanie, wybierając stacje" ]

viewRide : Ride -> Html Msg
viewRide ride =
    let
        toString stop =
            stop.name ++ " - " ++ stop.city

        niceTime time =
            String.padLeft 2 '0' <| String.fromInt (Time.toHour Time.utc time)
            ++ ":" ++
            (String.padLeft 2 '0' <| String.fromInt (Time.toMinute Time.utc time))

        stopsListStop fromStop toStop stop =
            li [ classList
                    [ ("stops-list-route-part", stop.stopNumber >= fromStop.stopNumber && stop.stopNumber <= toStop.stopNumber)
                    , ("stops-list-route-part-first", stop.stopNumber == fromStop.stopNumber)
                    , ("stops-list-route-part-last", stop.stopNumber == toStop.stopNumber)
                    ]
                ]
                [ span [ class "stops-list-time" ] [ text (niceTime stop.arrivalTime) ]
                , span [] [ text <| toString stop ]
                ]

        stopsList stops from to =
            ul [ class "stops-list" ] <| List.map (stopsListStop from to) stops

        maybeFromStop = List.head <| List.filter (\s -> s.id == ride.from) ride.stops
        maybeToStop   = List.head <| List.filter (\s -> s.id == ride.to) ride.stops
    in
        case (maybeFromStop, maybeToStop) of
            (Just fromStop, Just toStop) ->
                Card.config [ Card.attrs [ class "mt-5" ] ]
                    |> Card.headerH4 []
                        [ span [ class "oi oi-clock mr-1" ] []
                        , span [] [ text (niceTime ride.startTime) ]
                        , span [ class "font-italic float-right" ] [ text ride.train.name ]
                        ]
                    |> Card.block []
                        [
                            Block.custom <| stopsList ride.stops fromStop toStop
                        ]
                    |> Card.view
            (_, _) ->
                div [] [ text "Wystąpił błąd" ]
-- HTTP


getStopsList : String -> Cmd Msg
getStopsList api =
    Http.get
        { url = api ++ "stops"
        , expect = Http.expectJson GotStopsList stopsDecoder
        }

ridesUrl : String -> AutocompleteStop -> AutocompleteStop -> String
ridesUrl api from to =
    UrlBuilder.relative
        [ "rides" ]
        [ UrlBuilder.int "from" from.id
        , UrlBuilder.int "to" to.id
        ]

getRides : String -> AutocompleteStop -> AutocompleteStop -> Cmd Msg
getRides api from to =
    Http.get
        { url = api ++ (ridesUrl api from to)
        , expect = Http.expectJson GotRides ridesDecoder
        }


-- JSON


stopsDecoder : Decode.Decoder (List AutocompleteStop)
stopsDecoder =
    Decode.list <|
        Decode.map3 AutocompleteStop
            (Decode.field "id" Decode.int)
            (Decode.field "name" Decode.string)
            (Decode.field "city" Decode.string)

ridesDecoder : Decode.Decoder (List Ride)
ridesDecoder =
    Decode.list <|
        Decode.map8 Ride
            (Decode.field "id" Decode.int)
            (Decode.field "from" Decode.int)
            (Decode.field "to" Decode.int)
            (Decode.field "trainStops" rideStopDecoder)
            (Decode.field "startTime" TimeIso.decoder)
            (Decode.field "train" rideTrainDecoder)
            (Decode.field "freeTickets" Decode.int)
            (Decode.field "price" Decode.int)

rideStopDecoder : Decode.Decoder (List RideStop)
rideStopDecoder =
    Decode.list <|
        Decode.map5 RideStop
            (Decode.field "stopId" Decode.int)
            (Decode.field "arrivalTime" TimeIso.decoder)
            (Decode.field "stopNo" Decode.int)
            (Decode.field "name" Decode.string)
            (Decode.field "city" Decode.string)

rideTrainDecoder : Decode.Decoder RideTrain
rideTrainDecoder =
    Decode.map5 RideTrain
        (Decode.field "id" Decode.int)
        (Decode.field "name" Decode.string)
        (Decode.field "type" Decode.int)
        (Decode.field "seats" Decode.int)
        (Decode.field "wagons" Decode.int)
