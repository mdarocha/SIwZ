module Page.Home exposing (Model, init, view)

import Html exposing (..)
import Session
import Skeleton


type alias Model =
    { session : Session.Data
    }


init : Session.Data -> Model
init session =
    Model session


view : Model -> Skeleton.Details msg
view model =
    { title = "Dom"
    , body = [ text "Dom" ]
    }
