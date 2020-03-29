module Main exposing (..)

import Browser
import Browser.Navigation as Nav
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (onInput, onClick)
import Url
import Http
import Json.Decode as JsonDecode
import Json.Encode as JsonEncode

main : Program String Model Msg
main =
    Browser.application
        { init = init
        , view = view
        , update = update
        , subscriptions = subscriptions
        , onUrlChange = UrlChanged
        , onUrlRequest = LinkClicked
        }


-- MODEL
type Stops
    = Failure
    | Loading
    | Success (List TrainStop)

type alias Model =
    { api : String
    , stops: Stops
    , stopToAdd: TrainStop
    }

init : String -> Url.Url -> Nav.Key -> ( Model, Cmd Msg)
init api url key =
    ( Model api Loading (TrainStop "" "" ""), getStops api )


-- UPDATE
type StopUpdateMsg
    = NameUpdate String
    | CityUpdate String
    | SubmitStop

type Msg
    = LinkClicked Browser.UrlRequest
    | UrlChanged Url.Url
    | GotStops (Result Http.Error (List TrainStop))
    | AddStop (Result Http.Error ())
    | StopUpdate StopUpdateMsg

update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        GotStops result ->
            case result of
                Ok stops ->
                    let
                        oldStops =
                            case model.stops of
                                Success s -> s
                                _ -> []
                    in
                        ( { model | stops = Success (List.append oldStops stops) }, Cmd.none)
                Err _ ->
                    ( { model | stops = Failure }, Cmd.none)

        AddStop result ->
            case result of
                Ok _ ->
                    let
                        newStopList =
                            case model.stops of
                                Success stops ->
                                    List.append stops [ model.stopToAdd ]
                                _ ->
                                    [ model.stopToAdd ]
                        newStopToAdd = TrainStop "" "" ""
                    in
                        ( { model | stopToAdd = newStopToAdd, stops = Success newStopList }, Cmd.none)
                Err _ ->
                    ( model, Cmd.none )

        StopUpdate updateMsg ->
            case updateMsg of
                NameUpdate newName ->
                    let
                        oldStopToAdd = model.stopToAdd
                        newStopToAdd = { oldStopToAdd | name = newName }
                    in
                        ( { model | stopToAdd = newStopToAdd }, Cmd.none )
                CityUpdate newCity ->
                    let
                        oldStopToAdd = model.stopToAdd
                        newStopToAdd = { oldStopToAdd | city = newCity }
                    in
                        ( { model | stopToAdd = newStopToAdd }, Cmd.none )
                SubmitStop ->
                    ( model,  addStop model.api model.stopToAdd )
        _ ->
            ( model, Cmd.none )


-- SUBSCRIPTIONS
subscriptions : Model -> Sub Msg
subscriptions _ =
    Sub.none


-- VIEW
view : Model -> Browser.Document Msg
view model =
    { title = "Trains"
    , body =
        [
            div [] [ viewStopsList model.stops, viewInputStop model.stopToAdd ]
        ]
    }

viewStopsList : Stops -> Html Msg
viewStopsList model =
    div []
        [ case model of
            Failure ->
                text "Error"
            Loading ->
                text "Loading"
            Success stops ->
                ul [] (List.map (\s -> li [] [ text (s.name ++ " Miasto: " ++ s.city) ]) stops)
        ]

viewInputStop : TrainStop -> Html Msg
viewInputStop stopToAdd =
    div []
        [ input [ type_ "text", placeholder "nazwa", value stopToAdd.name, onInput (\v -> StopUpdate (NameUpdate v)) ] []
        , input [ type_ "text", placeholder "miasto", value stopToAdd.city, onInput (\v -> StopUpdate (CityUpdate v)) ] []
        , button [ onClick (StopUpdate SubmitStop) ] [ text "Dodaj miasto" ]
        ]


-- HTTP
getStops : String -> Cmd Msg
getStops api =
    Http.get
        { url = api ++ "trainstop/get"
        , expect = Http.expectJson GotStops stopsDecoder
        }

addStop : String -> TrainStop -> Cmd Msg
addStop api stop =
    Http.post
        { body = Http.jsonBody (stopEncoder stop)
        , expect = Http.expectWhatever AddStop
        , url = api ++ "trainstop/create"
        }

-- JSON
type alias TrainStop =
    { id: String
    , city: String
    , name: String
    }

stopsDecoder : JsonDecode.Decoder (List TrainStop)
stopsDecoder =
    JsonDecode.list stopDecoder

stopDecoder : JsonDecode.Decoder TrainStop
stopDecoder =
    JsonDecode.map3 TrainStop
        (JsonDecode.field "id" JsonDecode.string)
        (JsonDecode.field "city" JsonDecode.string)
        (JsonDecode.field "name" JsonDecode.string)

stopEncoder : TrainStop -> JsonEncode.Value
stopEncoder stop =
    JsonEncode.object
        [ ( "id", JsonEncode.string stop.id )
        , ( "city", JsonEncode.string stop.city )
        , ( "name", JsonEncode.string stop.name )
        ]
