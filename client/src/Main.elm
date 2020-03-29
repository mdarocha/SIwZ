module Main exposing (..)

import Browser
import Browser.Navigation as Nav
import Html exposing (..)
import Url
import Http

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
type Model
    = Failure
    | Loading
    | Success String

init : String -> Url.Url -> Nav.Key -> ( Model, Cmd Msg)
init api url key =
    ( Loading
    , Http.get
        { url = api ++ "trainstop/get"
        , expect = Http.expectString GotStops
        }
    )


-- UPDATE
type Msg
    = LinkClicked Browser.UrlRequest
    | UrlChanged Url.Url
    | GotStops (Result Http.Error String)

update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        GotStops result ->
            case result of
                Ok text ->
                    (Success text, Cmd.none)
                Err _ ->
                    (Failure, Cmd.none)
        _ ->
            ( model, Cmd.none )

-- SUBSCRIPTIONS
subscriptions : Model -> Sub Msg
subscriptions _ =
    Sub.none

-- VIEw
view : Model -> Browser.Document Msg
view model =
    { title = "Trains"
    , body =
        [ case model of
            Failure ->
                text "Error"
            Loading ->
                text "Loading"
            Success result ->
                text result
        ]
    }
