module Main exposing (..)

import Browser
import Browser.Navigation as Nav
import Html exposing (..)
import Url

main : Program () Model Msg
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
type alias Model =
    { text : String
    }

init : () -> Url.Url -> Nav.Key -> ( Model, Cmd Msg)
init flags url key =
    (Model "Hello world", Cmd.none)


-- UPDATE
type Msg
    = LinkClicked Browser.UrlRequest
    | UrlChanged Url.Url

update : Msg -> Model -> ( Model, Cmd Msg )
update _ model =
    ( model, Cmd.none )

-- SUBSCRIPTIONS
subscriptions : Model -> Sub Msg
subscriptions _ =
    Sub.none

view : Model -> Browser.Document Msg
view model =
    { title = "Trains"
    , body =
        [ text model.text ]
    }
