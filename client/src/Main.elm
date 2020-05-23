module Main exposing (..)

import Bootstrap.Grid as Grid
import Bootstrap.Navbar as Navbar
import Browser
import Browser.Navigation as Nav
import Html exposing (..)
import Html.Attributes exposing (..)
import Json.Decode as Decode
import Json.Encode as Encode
import Page.About as About
import Page.AdminTrainStops as AdminTrainStops
import Page.Login as Login
import Page.Search as Search
import Page.Ticket as Ticket
import Routes
import Session
import Skeleton
import Url exposing (Url)
import Url.Parser exposing (map)


type alias Model =
    { page : Page
    , navbarState : Navbar.State
    }


type Page
    = NotFound Session.Data
    | About About.Model
    | Search Search.Model
    | AdminTrainStops AdminTrainStops.Model
    | Login Login.Model
    | Ticket Ticket.Model


type Msg
    = LinkClicked Browser.UrlRequest
    | UrlChanged Url.Url
    | NavbarMsg Navbar.State
    | AboutMsg About.Msg
    | AdminTrainStopsMsg AdminTrainStops.Msg
    | LoginMsg Login.Msg
    | SearchMsg Search.Msg
    | TicketMsg Ticket.Msg


main : Program ( String, Encode.Value ) Model Msg
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
            Skeleton.view SearchMsg
                { title = "Nie znaleziono"
                , body = [ img [ src "https://http.cat/404.jpg", class "mx-auto d-block img-fluid" ] [] ]
                }
                (viewNavbar model)

        About about ->
            Skeleton.view AboutMsg (About.view about) (viewNavbar model)

        Search home ->
            Skeleton.view SearchMsg (Search.view home) (viewNavbar model)

        AdminTrainStops adminTrainStops ->
            Skeleton.view AdminTrainStopsMsg (AdminTrainStops.view adminTrainStops) (viewNavbar model)

        Login login ->
            Skeleton.view LoginMsg (Login.view login) (viewNavbar model)

        Ticket ticket ->
            Skeleton.view TicketMsg (Ticket.view ticket) (viewNavbar model)


init : ( String, Encode.Value ) -> Url.Url -> Nav.Key -> ( Model, Cmd Msg )
init ( api, userValue ) url key =
    let
        session =
            case Decode.decodeValue Session.userDecode userValue of
                Ok user ->
                    Session.Data api key (Just user)

                Err _ ->
                    Session.Data api key Nothing

        ( navbarState, navbarCmd ) =
            Navbar.initialState NavbarMsg

        ( model, routeCmd ) =
            changeRoute (Routes.fromUrl url) { page = NotFound session, navbarState = navbarState }
    in
    ( model, Cmd.batch [ routeCmd, navbarCmd ] )


update : Msg -> Model -> ( Model, Cmd Msg )
update message model =
    let
        session =
            toSession model
    in
    case message of
        LinkClicked urlRequest ->
            case urlRequest of
                Browser.Internal url ->
                    ( model
                    , Nav.pushUrl session.key (Url.toString url)
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

        LoginMsg msg ->
            case model.page of
                Login login ->
                    stepLogin model (Login.update msg login)

                _ ->
                    ( model, Cmd.none )

        SearchMsg msg ->
            case model.page of
                Search search ->
                    stepSearch model (Search.update msg search)

                _ ->
                    ( model, Cmd.none )

        TicketMsg msg ->
            case model.page of
                Ticket ticket ->
                    stepTicket model (Ticket.update msg ticket)

                _ ->
                    ( model, Cmd.none )


toSession : Model -> Session.Data
toSession model =
    case model.page of
        NotFound session ->
            session

        About m ->
            m.session

        Search m ->
            m.session

        AdminTrainStops m ->
            m.session

        Login m ->
            m.session

        Ticket m ->
            m.session


changeRoute : Maybe Routes.Route -> Model -> ( Model, Cmd Msg )
changeRoute route model =
    let
        session =
            toSession model
    in
    case route of
        Nothing ->
            ( { model | page = NotFound session }
            , Cmd.none
            )

        Just Routes.AdminTrainStopsRoute ->
            stepAdminTrainStops model (AdminTrainStops.init session)

        Just (Routes.SearchRoute from to) ->
            stepSearch model (Search.init session from to)

        Just Routes.AboutRoute ->
            stepAbout model (About.init session)

        Just Routes.RootRoute ->
            ( model, Nav.replaceUrl session.key "/search" )

        Just (Routes.LoginRoute redirect) ->
            stepLogin model (Login.init session redirect)

        Just (Routes.TicketRoute from to ride) ->
            stepTicket model (Ticket.init session from to ride)


stepAbout : Model -> ( About.Model, Cmd About.Msg ) -> ( Model, Cmd Msg )
stepAbout model ( about, cmds ) =
    ( { model | page = About about }
    , Cmd.map AboutMsg cmds
    )


stepAdminTrainStops : Model -> ( AdminTrainStops.Model, Cmd AdminTrainStops.Msg ) -> ( Model, Cmd Msg )
stepAdminTrainStops model ( adminTrainStops, cmds ) =
    ( { model | page = AdminTrainStops adminTrainStops }
    , Cmd.map AdminTrainStopsMsg cmds
    )


stepLogin : Model -> ( Login.Model, Cmd Login.Msg ) -> ( Model, Cmd Msg )
stepLogin model ( login, cmds ) =
    ( { model | page = Login login }
    , Cmd.map LoginMsg cmds
    )


stepSearch : Model -> ( Search.Model, Cmd Search.Msg ) -> ( Model, Cmd Msg )
stepSearch model ( search, cmds ) =
    ( { model | page = Search search }
    , Cmd.map SearchMsg cmds
    )


stepTicket : Model -> ( Ticket.Model, Cmd Ticket.Msg ) -> ( Model, Cmd Msg )
stepTicket model ( ticket, cmds ) =
    ( { model | page = Ticket ticket }
    , Cmd.map TicketMsg cmds
    )



-- NAVBAR


viewNavbar : Model -> Html Msg
viewNavbar model =
    Navbar.config NavbarMsg
        |> Navbar.withAnimation
        |> Navbar.dark
        |> Navbar.brand [ href "/" ] [ span [ id "brand-text" ] [ text "SIwZ Trains" ] ]
        |> Navbar.items
            [ Navbar.itemLink [ href "/search", dynamicActive ( Routes.SearchRoute Nothing Nothing, model ) ] [ text "Wyszukaj połączenie" ]
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
            , Navbar.itemLink [ href "/login", dynamicActive ( Routes.LoginRoute Nothing, model ) ] [ text (dynamicUserText model) ]
            ]
        |> Navbar.view model.navbarState



-- UTILS


dynamicUserText : Model -> String
dynamicUserText model =
    let
        session =
            toSession model

        user =
            session.user
    in
    case user of
        Just u ->
            u.name ++ " " ++ u.surname

        Nothing ->
            "Zaloguj się"


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

        ( Routes.LoginRoute _, Login _ ) ->
            True

        ( Routes.SearchRoute _ _, Search _ ) ->
            True

        _ ->
            False
