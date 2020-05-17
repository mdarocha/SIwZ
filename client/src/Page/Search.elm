module Page.Search exposing (Model, Msg, init, update, view)

import Bootstrap.Form.Input as Input
import Bootstrap.Grid as Grid
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (..)
import Http
import Json.Decode as Decode
import Json.Encode as Encode
import Session
import Skeleton


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
            ( { model | text = text }, Cmd.none )



-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    { title = "Route search"
    , body =
        [ Grid.row []
            [ Grid.col []
                [ Html.map RouteFromUpdate <| viewSearchBox model
                , text model.routeFromSearch.text
                ]
            ]
        ]
    }


viewSearchBox : Model -> Html SearchBoxMsg
viewSearchBox model =
    div [ class "search-box" ]
        [ Input.search [ Input.attrs [ onInput SearchTextUpdate ] ]
        , case model.trainStops of
            Success stops ->
                ul [] (List.map (\s -> li [] [ text (s.name ++ " - " ++ s.city) ]) stops)

            Loading ->
                ul [] []

            Failure ->
                div [] [ text "Błąd ładowania podpowiedzi - spróbuj odswieżyć stronę" ]
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
