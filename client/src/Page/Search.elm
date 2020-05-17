module Page.Search exposing (Model, Msg, init, update, view)

import Bootstrap.Form.Input as Input
import Bootstrap.Grid as Grid
import Bootstrap.Form.InputGroup as InputGroup
import Bootstrap.Button as Button
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (..)
import Http
import Json.Decode as Decode
import Json.Encode as Encode
import Session
import Skeleton
import Set

type alias TrainStop =
    { id : Int
    , name : String
    , city : String
    }


type TrainStops
    = Failure
    | Loading
    | Success (List TrainStop)


type alias SearchBoxState =
    { text : String
    , selected : Maybe TrainStop
    , showSuggestions : Bool
    }


type SearchBoxMsg
    = SearchTextUpdate String
    | ShowSuggestions
    | HideSuggestions
    | SuggestionSelected TrainStop
    | ClearInput

type alias Model =
    { session : Session.Data
    , routeFromSearch : SearchBoxState
    , trainStops : TrainStops
    }


type Msg
    = RouteFromUpdate SearchBoxMsg
    | GotStopsList (Result Http.Error (List TrainStop))



-- INIT


init : Session.Data -> ( Model, Cmd Msg )
init session =
    let
        newSearchBox =
            SearchBoxState "" Nothing False
    in
    ( Model session newSearchBox Loading, getStopsList session.api )



-- UPDATE


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        RouteFromUpdate searchMsg ->
            let
                ( newState, cmds ) =
                    updateSearchBox searchMsg model.routeFromSearch
            in
            ( { model | routeFromSearch = newState }, Cmd.map RouteFromUpdate cmds )

        GotStopsList result ->
            case result of
                Ok stops ->
                    ( { model | trainStops = Success stops }, Cmd.none )

                Err _ ->
                    ( { model | trainStops = Failure }, Cmd.none )


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
performSearch : List TrainStop -> String -> List TrainStop
performSearch suggestions input =
    let
        inputWords = List.map String.toLower <| String.words input

        matchesAllWords item =
            let
                matchesWord word =
                    String.contains word (String.toLower item.city)
                    || String.contains word (String.toLower item.name)
            in
            List.all matchesWord inputWords
    in
        List.filter matchesAllWords suggestions

stopToString : TrainStop -> String
stopToString stop =
    stop.name ++ " - " ++ stop.city


-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    { title = "Route search"
    , body =
        [ Grid.row []
            [ Grid.col []
                [ Html.map RouteFromUpdate <| viewSearchBox model.trainStops model.routeFromSearch
                ]
            ]
        ]
    }

viewSearchBox : TrainStops -> SearchBoxState -> Html SearchBoxMsg
viewSearchBox trainStops state =
    let
        suggestionItem = \s -> li [ class "dropdown-item", onMouseDown <| SuggestionSelected s ] [ text (stopToString s) ]
    in
        div [ class "search-box" ]
            [ case state.selected of
                Just stop ->
                    InputGroup.config
                        (InputGroup.text [ Input.value (stopToString stop), Input.readonly True ])
                        |> InputGroup.successors [ InputGroup.button [ Button.attrs [ onClick ClearInput ], Button.danger ] [ text "X" ] ]
                        |> InputGroup.view
                Nothing ->
                    Input.text [ Input.attrs [ onInput SearchTextUpdate, onFocus ShowSuggestions, onBlur HideSuggestions], Input.value state.text ]

            , if state.showSuggestions then
                case trainStops of
                    Success stops ->
                        let
                            items = performSearch stops state.text |> List.map suggestionItem
                        in
                            if List.length items > 0 then
                                ul [ class "dropdown-menu" ] items
                            else
                                div [] []
                    Loading ->
                        div [] []

                    Failure ->
                        div [] [ text "Błąd ładowania podpowiedzi - spróbuj odswieżyć stronę" ]
              else
                div [] []
            ]



-- HTTP


getStopsList : String -> Cmd Msg
getStopsList api =
    Http.get
        { url = api ++ "stops"
        , expect = Http.expectJson GotStopsList stopsDecoder
        }



-- JSON


stopsDecoder : Decode.Decoder (List TrainStop)
stopsDecoder =
    Decode.list <|
        Decode.map3 TrainStop
            (Decode.field "id" Decode.int)
            (Decode.field "city" Decode.string)
            (Decode.field "name" Decode.string)
