module Page.AdminTrainStops exposing (Model, Msg, init, update, view)

import Bootstrap.Alert as Alert
import Bootstrap.Button as Button
import Bootstrap.Form as Form
import Bootstrap.Form.Input as Input
import Bootstrap.Grid as Grid
import Bootstrap.Spinner as Spinner
import Bootstrap.Table as Table
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (onClick, onInput)
import Http
import Json.Decode as Decode
import Json.Encode as Encode
import Session
import Skeleton
import Jwt.Http


-- MODEL


type alias Error =
    String


type alias TrainStop =
    { id : Int
    , city : String
    , name : String
    }


type TrainStops
    = Failure
    | Loading
    | Success (List TrainStop)


type alias Model =
    { api : String
    , user : Session.User
    , stopList : TrainStops
    , stopToAdd : TrainStop
    , stopAddError : Maybe Error
    }



-- MSG


type StopToAddUpdate
    = NameUpdate String
    | CityUpdate String
    | Submit


type Msg
    = GotStops (Result Http.Error (List TrainStop))
    | AddStop (Result Http.Error ())
    | StopFormUpdate StopToAddUpdate



-- INIT


init : String -> Session.User -> ( Model, Cmd Msg )
init api user =
    ( Model api user Loading (TrainStop 0 "" "") Nothing
    , getStops api user.token
    )



-- UPDATE


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        GotStops result ->
            case result of
                Ok stops ->
                    let
                        oldStops =
                            case model.stopList of
                                Success s ->
                                    s

                                _ ->
                                    []
                    in
                    ( { model | stopList = Success (List.append oldStops stops) }, Cmd.none )

                Err _ ->
                    ( { model | stopList = Failure }, Cmd.none )

        AddStop result ->
            case result of
                Ok _ ->
                    let
                        newStopList =
                            case model.stopList of
                                Success stops ->
                                    List.append stops [ model.stopToAdd ]

                                _ ->
                                    [ model.stopToAdd ]

                        newStopToAdd =
                            TrainStop 0 "" ""
                    in
                    ( { model | stopToAdd = newStopToAdd, stopList = Success newStopList, stopAddError = Nothing }, Cmd.none )

                Err _ ->
                    ( { model | stopAddError = Just "Błąd podczas dodawania stacji" }, Cmd.none )

        StopFormUpdate updateMsg ->
            case updateMsg of
                NameUpdate newName ->
                    let
                        oldStopToAdd =
                            model.stopToAdd

                        newStopToAdd =
                            { oldStopToAdd | name = newName }
                    in
                    ( { model | stopToAdd = newStopToAdd }, Cmd.none )

                CityUpdate newCity ->
                    let
                        oldStopToAdd =
                            model.stopToAdd

                        newStopToAdd =
                            { oldStopToAdd | city = newCity }
                    in
                    ( { model | stopToAdd = newStopToAdd }, Cmd.none )

                Submit ->
                    if isValidInput model.stopToAdd then
                        ( model, addStop model.api model.stopToAdd model.user.token )

                    else
                        let
                            newStopToAdd =
                                TrainStop 0 "" ""
                        in
                        ( { model | stopToAdd = newStopToAdd, stopAddError = Just "Nieprawidłowy przystanek" }, Cmd.none )


isValidInput : TrainStop -> Bool
isValidInput stop =
    not (String.isEmpty stop.name || String.isEmpty stop.city)



-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    { title = "Przystanki - Admin"
    , body =
        [ div [ class "mt-3" ]
            [ Grid.row [] [ Grid.col [] [ viewStopsList model.stopList ] ]
            , Grid.row [] [ Grid.col [] [ viewInputStop model.stopToAdd model.stopAddError ] ]
            ]
        ]
    }


viewStopsList : TrainStops -> Html Msg
viewStopsList model =
    div [ class "mt-1" ]
        [ case model of
            Failure ->
                Alert.simpleWarning [] [ text "Błąd pobierania listy przystanków" ]

            Loading ->
                Spinner.spinner [] []

            Success stops ->
                Table.table
                    { options = [ Table.striped, Table.hover, Table.bordered, Table.responsive ]
                    , thead =
                        Table.simpleThead
                            [ Table.th [] [ text "Nazwa" ]
                            , Table.th [] [ text "Miasto" ]
                            ]
                    , tbody = Table.tbody [] (List.map (\s -> Table.tr [] [ Table.td [] [ text s.name ], Table.td [] [ text s.city ] ]) stops)
                    }
        ]


viewInputStop : TrainStop -> Maybe Error -> Html Msg
viewInputStop stopToAdd error =
    div [ class "mt-1" ]
        [ Form.form []
            [ Form.row []
                [ Form.col [] [ formInput "name" "Nazwa" stopToAdd.name NameUpdate ]
                , Form.col [] [ formInput "city" "Miasto" stopToAdd.city CityUpdate ]
                ]
            , Form.row [] [ Form.col [] [ Button.button [ Button.primary, Button.onClick (StopFormUpdate Submit) ] [ text "Dodaj" ] ] ]
            , formError error
            ]
        ]


formInput : String -> String -> String -> (String -> StopToAddUpdate) -> Html Msg
formInput id title value updateMsg =
    Form.group []
        [ Form.label [ for id ] [ text title ]
        , Input.text [ Input.id id, Input.value value, Input.attrs [ onInput (\v -> StopFormUpdate (updateMsg v)) ] ]
        ]


formError : Maybe Error -> Html Msg
formError error =
    case error of
        Just errorText ->
            Form.row [] [ Form.col [] [ Alert.simpleDanger [] [ text errorText ] ] ]

        Nothing ->
            div [] []



-- HTTP


getStops : String -> String -> Cmd Msg
getStops api token =
    Jwt.Http.get token
        { url = api ++ "admin/stops/get"
        , expect = Http.expectJson GotStops stopsDecoder
        }


addStop : String -> TrainStop -> String -> Cmd Msg
addStop api stop token =
    Jwt.Http.post token
        { body = Http.jsonBody (stopEncoder stop)
        , expect = Http.expectWhatever AddStop
        , url = api ++ "admin/stops/create"
        }



-- JSON


stopsDecoder : Decode.Decoder (List TrainStop)
stopsDecoder =
    Decode.list stopDecoder


stopDecoder : Decode.Decoder TrainStop
stopDecoder =
    Decode.map3 TrainStop
        (Decode.field "id" Decode.int)
        (Decode.field "city" Decode.string)
        (Decode.field "name" Decode.string)


stopEncoder : TrainStop -> Encode.Value
stopEncoder stop =
    Encode.object
        [ ( "id", Encode.int stop.id )
        , ( "city", Encode.string stop.city )
        , ( "name", Encode.string stop.name )
        ]
