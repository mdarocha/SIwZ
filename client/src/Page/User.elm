module Page.User exposing (Model, Msg, init, update, view)

import Browser.Navigation as Nav
import Html exposing (..)
import Session
import Skeleton


type alias Model =
    { session : Session.Data
    , user : Session.User
    }


type Msg
    = Nop



-- INIT


init : Session.Data -> ( Model, Cmd msg )
init session =
    case session.user of
        Just user ->
            ( Model session user, Cmd.none )

        Nothing ->
            ( Model session (Session.User "" "" "" "" ""), Nav.pushUrl session.key "/login" )



-- UPDATE


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        Nop ->
            ( model, Cmd.none )



-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    let
        user =
            model.user
    in
    { title = user.name ++ " " ++ user.surname
    , body =
        [ div [] [ text "user" ]
        ]
    }
