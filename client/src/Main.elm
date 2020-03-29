module Main exposing (..)

import Bootstrap.Grid as Grid
import Bootstrap.Navbar as Navbar
import Browser
import Browser.Navigation as Nav
import Html exposing (..)
import Html.Attributes exposing (class)
import Routes exposing (..)
import Session exposing (..)
import TrainRoutes
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
    = TrainRoutes Session TrainRoutes.Model
    | NotFound Session


toSession : Model -> Session
toSession model =
    case model of
        TrainRoutes session _ ->
            session

        NotFound session ->
            session



-- INIT


init : String -> Url.Url -> Nav.Key -> ( Model, Cmd Msg )
init api url key =
    let
        session =
            Session api key

        route =
            fromUrl url
    in
    changeRoute route (NotFound session)



-- UPDATE


type Msg
    = LinkClicked Browser.UrlRequest
    | UrlChanged Url.Url
    | TrainRoutesUpdate TrainRoutes.Msg


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case ( msg, model ) of
        ( TrainRoutesUpdate trainMsg, TrainRoutes session trainModel ) ->
            TrainRoutes.update trainMsg trainModel
                |> convertResult (TrainRoutes session) TrainRoutesUpdate

        ( _, NotFound _ ) ->
            ( model, Cmd.none )

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

        Just Routes.AdminTrainRoutes ->
            TrainRoutes.init session.api
                |> convertResult (TrainRoutes session) TrainRoutesUpdate



-- SUBSCRIPTIONS


subscriptions : Model -> Sub Msg
subscriptions _ =
    Sub.none



-- VIEW


view : Model -> Browser.Document Msg
view model =
    { title = "Trains"
    , body =
        [ Grid.container []
            [ viewHeader model
            , viewContent model
            ]
        ]
    }


viewContent : Model -> Html Msg
viewContent model =
    case model of
        TrainRoutes _ trainModel ->
            TrainRoutes.view trainModel
                |> Html.map TrainRoutesUpdate

        NotFound _ ->
            text "404 not found"


viewHeader : Model -> Html Msg
viewHeader model =
    let
        session =
            toSession model
    in
    h2 [ class "text-center mt-2 mb-2 w-100" ] [ text "SIwZ trains" ]
