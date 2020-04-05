module Page.About exposing (Model, Msg, init, update, view)

import Html exposing (..)
import Session
import Skeleton


type alias Model =
    { session : Session.Data
    , bodyText : String
    }


type Msg
    = TextChanged String


init : Session.Data -> ( Model, Cmd msg )
init session =
    ( Model session "O nas", Cmd.none )


update : Msg -> Model -> ( Model, Cmd msg )
update msg model =
    case msg of
        TextChanged text ->
            ( { model | bodyText = text }
            , Cmd.none
            )


view : Model -> Skeleton.Details msg
view model =
    { title = "O nas"
    , body = [ text model.bodyText ]
    }
