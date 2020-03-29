module TrainRoutes exposing (Model, Msg, init, update, view)

import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (onClick, onInput)
import Http
import Json.Decode as Decode
import Json.Encode as Encode
import Session exposing (..)



-- MODEL


type alias Error =
    String


type alias TrainStop =
    { id : String
    , city : String
    , name : String
    }


type TrainStops
    = Failure
    | Loading
    | Success (List TrainStop)


type alias Model =
    { session : Session
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


init : Session -> ( Model, Cmd Msg )
init session =
    ( Model session Loading (TrainStop "" "" "") Nothing
    , getStops session.api
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
                            TrainStop "" "" ""
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
                    ( model, addStop model.session.api model.stopToAdd )



-- VIEW


view : Model -> Html Msg
view model =
    div [] [ viewStopsList model.stopList, viewInputStop model.stopToAdd ]


viewStopsList : TrainStops -> Html Msg
viewStopsList model =
    div []
        [ case model of
            Failure ->
                text "Error"

            Loading ->
                text "Loading"

            Success stops ->
                ul [] (List.map (\s -> li [] [ text (s.name ++ " Miasto: " ++ s.city) ]) stops)
        ]


viewInputStop : TrainStop -> Html Msg
viewInputStop stopToAdd =
    div []
        [ input [ type_ "text", placeholder "nazwa", value stopToAdd.name, onInput (\v -> StopFormUpdate (NameUpdate v)) ] []
        , input [ type_ "text", placeholder "miasto", value stopToAdd.city, onInput (\v -> StopFormUpdate (CityUpdate v)) ] []
        , button [ onClick (StopFormUpdate Submit) ] [ text "Dodaj miasto" ]
        ]



-- HTTP


getStops : String -> Cmd Msg
getStops api =
    Http.get
        { url = api ++ "trainstop/get"
        , expect = Http.expectJson GotStops stopsDecoder
        }


addStop : String -> TrainStop -> Cmd Msg
addStop api stop =
    Http.post
        { body = Http.jsonBody (stopEncoder stop)
        , expect = Http.expectWhatever AddStop
        , url = api ++ "trainstop/create"
        }



-- JSON


stopsDecoder : Decode.Decoder (List TrainStop)
stopsDecoder =
    Decode.list stopDecoder


stopDecoder : Decode.Decoder TrainStop
stopDecoder =
    Decode.map3 TrainStop
        (Decode.field "id" Decode.string)
        (Decode.field "city" Decode.string)
        (Decode.field "name" Decode.string)


stopEncoder : TrainStop -> Encode.Value
stopEncoder stop =
    Encode.object
        [ ( "id", Encode.string stop.id )
        , ( "city", Encode.string stop.city )
        , ( "name", Encode.string stop.name )
        ]
