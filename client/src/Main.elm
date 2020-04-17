module Main exposing (..)

import Bootstrap.Navbar as Navbar
import Browser
import Browser.Navigation as Nav
import Html exposing (..)
import Html.Attributes exposing (..)
import Page.About as About
import Page.AdminTrainStops as AdminTrainStops
import Page.Home as Home
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
    | Home Home.Model
    | AdminTrainStops AdminTrainStops.Model


type Msg
    = NoOp
    | LinkClicked Browser.UrlRequest
    | UrlChanged Url.Url
    | NavbarMsg Navbar.State
    | AboutMsg About.Msg
    | AdminTrainStopsMsg AdminTrainStops.Msg


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
                (viewNavbar model)

        About about ->
            Skeleton.view AboutMsg (About.view about) (viewNavbar model)

        Home home ->
            Skeleton.view never (Home.view home) (viewNavbar model)

        AdminTrainStops adminTrainStops ->
            Skeleton.view AdminTrainStopsMsg (AdminTrainStops.view adminTrainStops) (viewNavbar model)


init : String -> Url.Url -> Nav.Key -> ( Model, Cmd Msg )
init api url key =
    let
        session =
            Session.Data api

        ( navbarState, navbarCmd ) =
            Navbar.initialState NavbarMsg

        ( model, routeCmd ) =
            changeRoute (Routes.fromUrl url) { nav = key, page = NotFound session, navbarState = navbarState }
    in
    ( model, Cmd.batch [ routeCmd, navbarCmd ] )


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

        AdminTrainStopsMsg msg ->
            case model.page of
                AdminTrainStops adminTrainStops ->
                    stepAdminTrainStops model (AdminTrainStops.update msg adminTrainStops)

                _ ->
                    ( model, Cmd.none )


exit : Model -> Session.Data
exit model =
    case model.page of
        NotFound session ->
            session

        About m ->
            m.session

        Home m ->
            m.session

        AdminTrainStops m ->
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
            stepAdminTrainStops model (AdminTrainStops.init session)

        Just Routes.SearchRoute ->
            ( { model | page = NotFound session }
            , Cmd.none
            )

        Just Routes.AboutRoute ->
            stepAbout model (About.init session)

        Just Routes.HomeRoute ->
            stepHome model (Home.init session)


stepAbout : Model -> ( About.Model, Cmd About.Msg ) -> ( Model, Cmd Msg )
stepAbout model ( about, cmds ) =
    ( { model | page = About about }
    , Cmd.map AboutMsg cmds
    )


stepHome : Model -> Home.Model -> ( Model, Cmd msg )
stepHome model home =
    ( { model | page = Home home }
    , Cmd.none
    )


stepAdminTrainStops : Model -> ( AdminTrainStops.Model, Cmd AdminTrainStops.Msg ) -> ( Model, Cmd Msg )
stepAdminTrainStops model ( adminTrainStops, cmds ) =
    ( { model | page = AdminTrainStops adminTrainStops }
    , Cmd.map AdminTrainStopsMsg cmds
    )



-- NAVBAR


viewNavbar : Model -> Html Msg
viewNavbar model =
    Navbar.config NavbarMsg
        |> Navbar.withAnimation
        |> Navbar.dark
        |> Navbar.brand [ href "/" ] [ text "SIwZ Trains" ]
        |> Navbar.items
            [ Navbar.itemLink [ href "/search", dynamicActive ( Routes.SearchRoute, model ) ] [ text "Wyszukaj połączenie" ]
            , Navbar.itemLink [ href "/about", dynamicActive ( Routes.AboutRoute, model ) ] [ text "O nas" ]
            , Navbar.dropdown
                { id = "admin-dropdown"
                , toggle = Navbar.dropdownToggle [] [ text "Panel admina" ]
                , items =
                    [ Navbar.dropdownItem [ href "/admin/users" ] [ text "Użytkownicy" ]
                    , Navbar.dropdownItem [ href "/admin/train" ] [ text "Pociągi" ]
                    , Navbar.dropdownItem [ href "/admin/stops", dynamicActive ( Routes.AdminTrainStopsRoute, model ) ] [ text "Przystanki" ]
                    , Navbar.dropdownItem [ href "/admin/routes" ] [ text "Trasy" ]
                    , Navbar.dropdownItem [ href "/admin/tickets" ] [ text "Bilety" ]
                    , Navbar.dropdownItem [ href "/admin/discounts" ] [ text "Zniżki" ]
                    ]
                }
            ]
        |> Navbar.view model.navbarState



-- UTILS


dynamicActive : ( Routes.Route, Model ) -> Html.Attribute msg
dynamicActive ( route, model ) =
    classList [ ( "active", isActive ( route, model.page ) ) ]


isActive : ( Routes.Route, Page ) -> Bool
isActive ( route, page ) =
    case ( route, page ) of
        ( Routes.AdminTrainStopsRoute, AdminTrainStops _ ) ->
            True

        ( Routes.AboutRoute, About _ ) ->
            True

        _ ->
            False
