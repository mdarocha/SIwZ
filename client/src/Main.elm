module Main exposing (..)

import Browser
import Browser.Navigation as Nav
import Html exposing (..)
import Url
import Http
import Json.Decode as Json

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
    }

init : String -> Url.Url -> Nav.Key -> ( Model, Cmd Msg)
init api url key =
    ( Model api Loading, getStops api )


-- UPDATE
type Msg
    = LinkClicked Browser.UrlRequest
    | UrlChanged Url.Url
    | GotStops (Result Http.Error (List TrainStop))

update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        GotStops result ->
            case result of
                Ok stops ->
                    ( { model | stops = Success stops }, Cmd.none)
                Err _ ->
                    ( { model | stops = Failure }, Cmd.none)
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
        [ case model.stops of
            Failure ->
                text "Error"
            Loading ->
                text "Loading"
            Success stops ->
                ul []
                    (List.map (\s -> li [] [ text s.name ]) stops)
        ]
    }

-- HTTP
getStops : String -> Cmd Msg
getStops api =
    Http.get
        { url = api ++ "trainstop/get"
        , expect = Http.expectJson GotStops stopsDecoder
        }

type alias TrainStop =
    { id: String
    , city: String
    , name: String
    }

stopsDecoder : Json.Decoder (List TrainStop)
stopsDecoder =
    Json.list stopDecoder

stopDecoder : Json.Decoder TrainStop
stopDecoder =
    Json.map3 TrainStop
        (Json.field "id" Json.string)
        (Json.field "city" Json.string)
        (Json.field "name" Json.string)
