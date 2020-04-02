module Main exposing (..)

import Bootstrap.Grid as Grid
import Bootstrap.Navbar as Navbar
import Browser
import Browser.Navigation as Nav
import Html exposing (..)
import Html.Attributes exposing (class, classList, href, id)
import Routes exposing (..)
import Session exposing (..)
import AdminTrainStops
import Url


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
    = AdminTrainStops Session AdminTrainStops.Model
    | TrainSearch Session
    | About Session
    | NotFound Session


toSession : Model -> Session
toSession model =
    case model of
        AdminTrainStops session _ ->
            session

        NotFound session ->
            session

        TrainSearch session ->
            session

        About session ->
            session


setSession : Model -> Session -> Model
setSession model session =
    case model of
        AdminTrainStops _ trainModel ->
            AdminTrainStops session trainModel

        TrainSearch _ ->
            TrainSearch session

        About _ ->
            About session

        NotFound _ ->
            NotFound session



-- INIT


init : String -> Url.Url -> Nav.Key -> ( Model, Cmd Msg )
init api url key =
    let
        ( navbarState, navbarCmd ) =
            Navbar.initialState NavbarMsg

        session =
            Session api key navbarState

        ( model, routeCmd ) =
            changeRoute (fromUrl url) (NotFound session)
    in
    ( model, Cmd.batch [ routeCmd, navbarCmd ] )



-- UPDATE


type Msg
    = LinkClicked Browser.UrlRequest
    | UrlChanged Url.Url
    | NavbarMsg Navbar.State
    | AdminTrainStopsUpdate AdminTrainStops.Msg


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case ( msg, model ) of
        ( LinkClicked request, _ ) ->
            case request of
                Browser.Internal url ->
                    let
                        session =
                            toSession model
                    in
                    ( model, Nav.pushUrl session.nav (Url.toString url) )

                Browser.External href ->
                    ( model, Nav.load href )

        ( UrlChanged url, _ ) ->
            changeRoute (fromUrl url) model

        ( NavbarMsg state, _ ) ->
            let
                oldSession =
                    toSession model

                newSession =
                    { oldSession | navbarState = state }
            in
            ( setSession model newSession, Cmd.none )

        ( AdminTrainStopsUpdate trainMsg, AdminTrainStops session trainModel ) ->
            AdminTrainStops.update trainMsg trainModel
                |> convertResult (AdminTrainStops session) AdminTrainStopsUpdate

        ( _, NotFound _ ) ->
            ( model, Cmd.none )

        ( _, About _ ) ->
            ( model, Cmd.none )

        ( _, TrainSearch _ ) ->
            ( model, Cmd.none )


convertResult : (subModel -> Model) -> (subMsg -> Msg) -> ( subModel, Cmd subMsg ) -> ( Model, Cmd Msg )
convertResult toModel toMsg ( subModel, subCmd ) =
    ( toModel subModel, Cmd.map toMsg subCmd )


changeRoute : Maybe Route -> Model -> ( Model, Cmd Msg )
changeRoute route model =
    let
        session =
            toSession model
    in
    case route of
        Nothing ->
            ( NotFound session, Cmd.none )

        Just Routes.AdminTrainStops ->
            AdminTrainStops.init session.api
                |> convertResult (AdminTrainStops session) AdminTrainStopsUpdate

        Just Routes.SearchRoute ->
            ( TrainSearch session, Cmd.none )

        Just Routes.AboutRoute ->
            ( About session, Cmd.none )

        Just Routes.Root ->
            ( model, Nav.replaceUrl session.nav "/search" )

-- SUBSCRIPTIONS


subscriptions : Model -> Sub Msg
subscriptions model =
    let
        session =
            toSession model
    in
    Navbar.subscriptions session.navbarState NavbarMsg



-- VIEW


view : Model -> Browser.Document Msg
view model =
    { title = "Trains"
    , body = 
        [ viewHeader model
        , div [ id "wrap" ] [ Grid.container [] [ viewContent model ] ]
        , viewFooter
        ]
    }


viewContent : Model -> Html Msg
viewContent model =
    case model of
        AdminTrainStops _ trainModel ->
            AdminTrainStops.view trainModel
                |> Html.map AdminTrainStopsUpdate

        NotFound _ ->
            text "404 not found"

        About _ ->
            text "About page"

        TrainSearch _ ->
            text "Train search page"

viewFooter : Html Msg
viewFooter =
    footer [] [ text "© 2137 lololololol" ]

-- TODO make this dynamic
viewHeader : Model -> Html Msg 
viewHeader model =
    let
        session =
            toSession model

        navbarState =
            session.navbarState
    in
    Navbar.config NavbarMsg
        |> Navbar.withAnimation
        |> Navbar.dark
        |> Navbar.brand [ href "/" ] [ text "SIwZ Trains" ]
        |> Navbar.items
            [ Navbar.itemLink [ href "/search", dynamicActive Routes.SearchRoute model ] [ text "Wyszukaj połączenie" ]
            , Navbar.itemLink [ href "/about", dynamicActive Routes.AboutRoute model ] [ text "O nas" ]
            , Navbar.dropdown
                { id = "admin-dropdown"
                , toggle = Navbar.dropdownToggle [] [ text "Panel admina" ]
                , items =
                    [ Navbar.dropdownItem [ href "/admin/users" ] [ text "Użytkownicy" ]
                    , Navbar.dropdownItem [ href "/admin/train" ] [ text "Pociągi" ]
                    , Navbar.dropdownItem [ href "/admin/stops", dynamicActive Routes.AdminTrainStops model ] [ text "Przystanki" ]
                    , Navbar.dropdownItem [ href "/admin/routes"] [ text "Trasy" ]
                    , Navbar.dropdownItem [ href "/admin/tickets" ] [ text "Bilety" ]
                    , Navbar.dropdownItem [ href "/admin/discounts" ] [ text "Zniżki" ]
                    ]
                }
            ]
        |> Navbar.view navbarState


dynamicActive : Route -> Model -> Html.Attribute Msg
dynamicActive route model =
    classList [ ( "active", isActive route model ) ]


isActive : Route -> Model -> Bool
isActive route model =
    case ( route, model ) of
        ( Routes.SearchRoute, TrainSearch _ ) ->
            True

        ( Routes.AdminTrainStops, AdminTrainStops _ _ ) ->
            True

        ( Routes.AboutRoute, About _ ) ->
            True

        ( _, _ ) ->
            False
