module Main exposing (..)

import Bootstrap.Navbar as Navbar
import Browser
import Browser.Navigation as Nav
import Html exposing (..)
import Html.Attributes exposing (..)
import Page.About as About
import Routes
import Session
import Skeleton
import Url exposing (Url)
import Url.Parser exposing (map)


type alias Model =
    { nav : Nav.Key
    , page : Page
    , navbarState : Navbar.State
    }


type Page
    = NotFound Session.Data
    | About About.Model


type Msg
    = NoOp
    | LinkClicked Browser.UrlRequest
    | UrlChanged Url.Url
    | NavbarMsg Navbar.State
    | AboutMsg About.Msg


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


subscriptions : Model -> Sub Msg
subscriptions model =
    Navbar.subscriptions model.navbarState NavbarMsg


view : Model -> Browser.Document Msg
view model =
    case model.page of
        NotFound _ ->
            Skeleton.view never
                { title = "Nie znaleziono"
                , body = [ text "Nie znaleziono" ]
                }
                model.navbarState

        About about ->
            Skeleton.view never (About.view about) model.navbarState


init : String -> Url.Url -> Nav.Key -> ( Model, Cmd Msg )
init api url key =
    let
        session =
            Session.Data api

        ( navbarState, navbarCmd ) =
            Navbar.initialState NavbarMsg

        ( model, routeCmd ) =
            changeRoute (Routes.fromUrl url) { nav = key, page = NotFound (Session.Data api), navbarState = navbarState }
    in
    ( model, Cmd.batch [ routeCmd ] )


update : Msg -> Model -> ( Model, Cmd Msg )
update message model =
    case message of
        NoOp ->
            ( model, Cmd.none )

        LinkClicked urlRequest ->
            case urlRequest of
                Browser.Internal url ->
                    ( model
                    , Nav.pushUrl model.nav (Url.toString url)
                    )

                Browser.External href ->
                    ( model
                    , Nav.load href
                    )

        UrlChanged url ->
            changeRoute (Routes.fromUrl url) model

        NavbarMsg msg ->
            ( { model | navbarState = msg }
            , Cmd.map NavbarMsg Cmd.none
            )

        AboutMsg msg ->
            case model.page of
                About about ->
                    stepAbout model (About.update msg about)

                _ ->
                    ( model, Cmd.none )


exit : Model -> Session.Data
exit model =
    case model.page of
        NotFound session ->
            session

        About m ->
            m.session


changeRoute : Maybe Routes.Route -> Model -> ( Model, Cmd Msg )
changeRoute route model =
    let
        session =
            exit model
    in
    case route of
        Nothing ->
            ( { model | page = NotFound session }
            , Cmd.none
            )

        Just Routes.AdminTrainStopsRoute ->
            Debug.todo ""

        Just Routes.SearchRoute ->
            Debug.todo ""

        Just Routes.AboutRoute ->
            stepAbout model (About.init session)

        Just Routes.HomeRoute ->
            Debug.todo ""


stepAbout : Model -> ( About.Model, Cmd About.Msg ) -> ( Model, Cmd Msg )
stepAbout model ( about, cmds ) =
    ( { model | page = About about }
    , Cmd.map AboutMsg cmds
    )
