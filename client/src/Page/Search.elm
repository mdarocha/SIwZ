module Page.Search exposing (Model, Msg, init, update, view)

import Bootstrap.Button as Button
import Bootstrap.Card as Card
import Bootstrap.Card.Block as Block
import Bootstrap.Form.Checkbox as Checkbox
import Bootstrap.Form.Input as Input
import Bootstrap.Form.InputGroup as InputGroup
import Bootstrap.Grid as Grid
import Bootstrap.Grid.Col as Col
import Bootstrap.Spinner as Spinner
import Bootstrap.Text as Text
import Browser.Navigation as Nav
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (..)
import Http
import Iso8601 as TimeIso
import Json.Decode as Decode
import Json.Encode as Encode
import Session
import Set
import Skeleton
import Time
import Url.Builder as UrlBuilder


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
    , train : RideTrain
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
    , queryFromId : Maybe Int
    , queryToId : Maybe Int
    , useDepartureTime : Bool
    , departureTime : String
    , rides : Rides
    }


type alias TicketParams =
    { from : Int
    , to : Int
    , ride : Int
    }


type Msg
    = RouteFromUpdate SearchBoxMsg
    | RouteToUpdate SearchBoxMsg
    | GotStopsList (Result Http.Error (List AutocompleteStop))
    | GotRides (Result Http.Error (List Ride))
    | SubmitSearch
    | DepartureTimeUpdate String
    | SubmitRide TicketParams



-- INIT


init : Session.Data -> Maybe Int -> Maybe Int -> ( Model, Cmd Msg )
init session from to =
    let
        newSearchBox =
            SearchBoxState "" Nothing False Loading
    in
    ( Model session newSearchBox newSearchBox from to False "" NotStarted, getStopsList session.api )



-- UPDATE


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        RouteFromUpdate searchMsg ->
            let
                ( newState, cmds ) =
                    updateSearchBox searchMsg model.routeFromSearch

                rides =
                    case newState.selected of
                        Nothing ->
                            NotStarted

                        _ ->
                            model.rides
            in
            ( { model | routeFromSearch = newState, rides = rides }, Cmd.map RouteFromUpdate cmds )

        RouteToUpdate searchMsg ->
            let
                ( newState, cmds ) =
                    updateSearchBox searchMsg model.routeToSearch

                rides =
                    case newState.selected of
                        Nothing ->
                            NotStarted

                        _ ->
                            model.rides
            in
            ( { model | routeToSearch = newState, rides = rides }, Cmd.map RouteToUpdate cmds )

        GotStopsList result ->
            case result of
                Ok stops ->
                    let
                        oldFromState =
                            model.routeFromSearch

                        oldToState =
                            model.routeToSearch

                        fromSuggestions =
                            { oldFromState | suggestions = Success stops }

                        toSuggestions =
                            { oldToState | suggestions = Success stops }

                        ( newFromState, newToState ) =
                            case ( model.queryFromId, model.queryToId ) of
                                ( Just queryFrom, Just queryTo ) ->
                                    let
                                        maybeFromStop =
                                            List.head <| List.filter (\s -> s.id == queryFrom) stops

                                        maybeToStop =
                                            List.head <| List.filter (\s -> s.id == queryTo) stops
                                    in
                                    case ( maybeFromStop, maybeToStop ) of
                                        ( Just from, Just to ) ->
                                            ( { fromSuggestions | selected = Just from }, { toSuggestions | selected = Just to } )

                                        ( _, _ ) ->
                                            ( fromSuggestions, toSuggestions )

                                ( _, _ ) ->
                                    ( fromSuggestions, toSuggestions )
                    in
                    ( { model | routeToSearch = newToState, routeFromSearch = newFromState }, Cmd.none )

                Err _ ->
                    let
                        oldFromState =
                            model.routeFromSearch

                        oldToState =
                            model.routeToSearch

                        newFromState =
                            { oldFromState | suggestions = Failure }

                        newToState =
                            { oldToState | suggestions = Failure }
                    in
                    ( { model | routeToSearch = newToState, routeFromSearch = newFromState }, Cmd.none )

        SubmitSearch ->
            case ( model.routeFromSearch.selected, model.routeToSearch.selected ) of
                ( Just from, Just to ) ->
                    let
                        getCmd =
                            getRides model.session.api from to model.useDepartureTime model.departureTime

                        url =
                            UrlBuilder.absolute [ "search" ]
                                [ UrlBuilder.int "from" from.id
                                , UrlBuilder.int "to" to.id
                                ]

                        navCmd =
                            Nav.replaceUrl model.session.key url
                    in
                    ( { model | rides = RidesLoading }, Cmd.batch [ getCmd, navCmd ] )

                ( _, _ ) ->
                    ( model, Cmd.none )

        DepartureTimeUpdate text ->
            ( { model | departureTime = text }, Cmd.none )

        GotRides result ->
            case result of
                Ok rides ->
                    ( { model | rides = RidesSuccess rides }, Cmd.none )

                Err _ ->
                    ( { model | rides = RidesFailure }, Cmd.none )

        SubmitRide ticketParams ->
            let
                url =
                    UrlBuilder.absolute [ "ticket" ]
                        [ UrlBuilder.int "from" ticketParams.from
                        , UrlBuilder.int "to" ticketParams.to
                        , UrlBuilder.int "ride" ticketParams.ride
                        ]
            in
            ( model, Nav.pushUrl model.session.key url )


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


niceTime : Time.Posix -> String
niceTime time =
    String.padLeft 2 '0' <|
        String.fromInt (Time.toHour Time.utc time)
            ++ ":"
            ++ (String.padLeft 2 '0' <| String.fromInt (Time.toMinute Time.utc time))


rideStopToString : RideStop -> String
rideStopToString stop =
    stop.name ++ " - " ++ stop.city



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
            [ Grid.col [ Col.md6, Col.attrs [ class "mt-4" ] ] [ Checkbox.checkbox [] "Szukaj wg. czasu odjazdu", Input.datetimeLocal [ Input.attrs [ onInput DepartureTimeUpdate ] ] ]
            , Grid.col [ Col.md3, Col.offsetMd3, Col.attrs [ class "mt-4" ] ] [ Button.button [ Button.attrs [ class "ride-search-button", onClick SubmitSearch ], Button.primary, Button.large, Button.block ] [ text "Szukaj" ] ]
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
        emptyStop =
            RideStop 0 (Time.millisToPosix 0) 0 "ERROR" "ERROR"

        escapeMaybe =
            Maybe.withDefault emptyStop

        fromStop =
            (List.head <| List.filter (\s -> s.id == ride.from) ride.stops) |> escapeMaybe

        toStop =
            (List.head <| List.filter (\s -> s.id == ride.to) ride.stops) |> escapeMaybe

        firstStop =
            List.head ride.stops |> escapeMaybe

        lastStop =
            (List.head <| List.reverse ride.stops) |> escapeMaybe

        sortedStops =
            List.sortBy .stopNumber ride.stops
    in
    Card.config [ Card.attrs [ class "mt-5 ride-card" ] ]
        |> Card.headerH4 [ class "d-flex flex-wrap" ]
            [ div [ style "flex" "1", style "white-space" "nowrap" ]
                [ span [ class "oi oi-clock mr-1", style "font-size" "1.2rem" ] []
                , span [] [ text (niceTime fromStop.arrivalTime) ]
                ]
            , span [ class "font-italic" ]
                [ text <|
                    ride.train.name
                        ++ " "
                        ++ firstStop.city
                        ++ " - "
                        ++ lastStop.city
                ]
            ]
        |> Card.block []
            [ Block.custom <| viewStopList sortedStops fromStop toStop ]
        |> Card.block [ Block.align Text.alignXsRight ]
            [ Block.custom <|
                div [ class "ride-price" ]
                    [ span [ class "oi oi-paperclip" ] []
                    , span [] [ text <| String.fromInt ride.price ++ " zł" ]
                    , Button.button
                        [ Button.primary
                        , Button.attrs
                            [ class "align-baseline"
                            , onClick (SubmitRide <| TicketParams ride.from ride.to ride.id)
                            ]
                        ]
                        [ text "Kup bilet" ]
                    ]
            ]
        |> Card.view


viewStopListStop : RideStop -> RideStop -> RideStop -> Html Msg
viewStopListStop from to stop =
    li
        [ classList
            [ ( "stops-list-route-part", stop.stopNumber >= from.stopNumber && stop.stopNumber <= to.stopNumber )
            , ( "stops-list-route-part-first", stop.stopNumber == from.stopNumber )
            , ( "stops-list-route-part-last", stop.stopNumber == to.stopNumber )
            ]
        ]
        [ span [ class "stops-list-time" ] [ text (niceTime stop.arrivalTime) ]
        , span [] [ text <| rideStopToString stop ]
        ]


viewStopList : List RideStop -> RideStop -> RideStop -> Html Msg
viewStopList stops from to =
    ul [ class "stops-list" ] <| List.map (viewStopListStop from to) stops



-- HTTP


getStopsList : String -> Cmd Msg
getStopsList api =
    Http.get
        { url = api ++ "stops"
        , expect = Http.expectJson GotStopsList stopsDecoder
        }


ridesUrl : AutocompleteStop -> AutocompleteStop -> Bool -> String -> String
ridesUrl from to useDate date =
    let
        baseQuery =
            [ UrlBuilder.int "from" from.id
            , UrlBuilder.int "to" to.id
            ]

        query =
            if useDate then
                List.append baseQuery [ UrlBuilder.string "date" date ]

            else
                baseQuery
    in
    UrlBuilder.relative [ "rides" ] query


getRides : String -> AutocompleteStop -> AutocompleteStop -> Bool -> String -> Cmd Msg
getRides api from to useDate date =
    Http.get
        { url = api ++ ridesUrl from to useDate date
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
        Decode.map6 Ride
            (Decode.field "id" Decode.int)
            (Decode.field "from" Decode.int)
            (Decode.field "to" Decode.int)
            (Decode.field "trainStops" rideStopDecoder)
            (Decode.field "train" rideTrainDecoder)
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
