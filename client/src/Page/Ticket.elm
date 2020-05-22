module Page.Ticket exposing (Model, Msg, init, update, view)

import Html exposing (..)
import Session
import Skeleton


-- MODEL


type alias Model =
    { session : Session.Data
    }

-- MSG

type Msg
    = Dunno


-- INIT
init : Session.Data -> Maybe Int -> Maybe Int -> Maybe Int -> ( Model, Cmd Msg )
init session from to ride =
    ( Model session, Cmd.none )

-- UPDATE
update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        Dunno ->
            ( model, Cmd.none )

-- VIEW
view : Model -> Skeleton.Details Msg
view model =
    { title = "Kup bilet"
    , body = [ text "ticket" ]
    }
