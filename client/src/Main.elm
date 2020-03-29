module Main exposing (..)

import Browser
import Browser.Navigation as Nav
import Html exposing (..)
import Html.Attributes exposing (class)
import Url
import Bootstrap.Grid as Grid
import Bootstrap.Navbar as Navbar

import Session exposing (..)
import TrainRoutes

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
    = TrainRoutes TrainRoutes.Model


init : String -> Url.Url -> Nav.Key -> ( Model, Cmd Msg )
init api url key =
    TrainRoutes.init (Session api key)
        |> convertResult TrainRoutes TrainRoutesUpdate



-- UPDATE


type Msg
    = LinkClicked Browser.UrlRequest
    | UrlChanged Url.Url
    | TrainRoutesUpdate TrainRoutes.Msg


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case ( msg, model ) of
        ( TrainRoutesUpdate trainMsg, TrainRoutes trainModel ) ->
            TrainRoutes.update trainMsg trainModel
                |> convertResult TrainRoutes TrainRoutesUpdate

        _ ->
            ( model, Cmd.none )


convertResult : (subModel -> Model) -> (subMsg -> Msg) -> ( subModel, Cmd subMsg ) -> ( Model, Cmd Msg )
convertResult toModel toMsg ( subModel, subCmd ) =
    ( toModel subModel, Cmd.map toMsg subCmd )



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
            [ viewHeader
            , viewContent model
            ]
        ]
    }

viewContent : Model -> Html Msg
viewContent model =
    case model of
        TrainRoutes trainModel ->
            TrainRoutes.view trainModel
                |> Html.map TrainRoutesUpdate

viewHeader : Html Msg
viewHeader =
    h2 [ class "text-center mt-2 mb-2 w-100" ] [ text "SIwZ trains" ]
