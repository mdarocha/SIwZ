module Page.Ticket exposing (Model, Msg, init, update, view)

import Bootstrap.Grid as Grid
import Bootstrap.Grid.Col as Col
import Bootstrap.Grid.Row as Row
import Bootstrap.Spinner as Spinner
import Browser.Navigation as Nav
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (..)
import Http
import Iso8601 as TimeIso
import Json.Decode as Decode
import Session
import Skeleton
import Time
import Url.Builder as UrlBuilder



-- MODEL


type alias RideStop =
    { id : Int
    , arrivalTime : Time.Posix
    , stopNumber : Int
    , name : String
    , city : String
    }


type alias RideTrain =
    { id : Int
    , name : String
    , trainType : Int
    , seats : Int
    , wagons : Int
    }


type alias Ride =
    { id : Int
    , from : Int
    , to : Int
    , stops : List RideStop
    , train : RideTrain
    , price : Int
    }


type RideData
    = RideFailure
    | RideLoading
    | RideSuccess Ride


type alias Model =
    { session : Session.Data
    , ride : RideData
    }



-- MSG


type Msg
    = GotRide (Result Http.Error Ride)



-- INIT


init : Session.Data -> Maybe Int -> Maybe Int -> Maybe Int -> ( Model, Cmd Msg )
init session from to ride =
    let
        model =
            Model session RideLoading
    in
    case ( from, to, ride ) of
        ( Just fromId, Just toId, Just rideId ) ->
            case session.user of
                Just _ ->
                    ( model, getRide session.api fromId toId rideId )

                Nothing ->
                    let
                        returnUrl =
                            UrlBuilder.relative [ "ticket" ]
                                [ UrlBuilder.int "from" fromId
                                , UrlBuilder.int "to" toId
                                , UrlBuilder.int "ride" rideId
                                ]

                        url =
                            UrlBuilder.absolute [ "login" ] [ UrlBuilder.string "return" returnUrl ]
                    in
                    ( model, Nav.pushUrl session.key url )

        ( _, _, _ ) ->
            ( model, Nav.pushUrl session.key "/" )



-- UPDATE


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        GotRide result ->
            case result of
                Ok ride ->
                    ( { model | ride = RideSuccess ride }, Cmd.none )

                Err _ ->
                    ( { model | ride = RideFailure }, Cmd.none )



-- UTIL


niceTime : Time.Posix -> String
niceTime time =
    String.padLeft 2 '0' <|
        String.fromInt (Time.toHour Time.utc time)
            ++ ":"
            ++ (String.padLeft 2 '0' <| String.fromInt (Time.toMinute Time.utc time))

niceDate : Time.Posix -> String
niceDate time =
    String.fromInt (Time.toDay Time.utc time)
    ++ " "
    ++ String.toLower (toMonth <| Time.toMonth Time.utc time)
    ++ " "
    ++ String.fromInt (Time.toYear Time.utc time)

toMonth : Time.Month -> String
toMonth month =
    case month of
        Time.Jan -> "Styczeń"
        Time.Feb -> "Luty"
        Time.Mar -> "Marzec"
        Time.Apr -> "Kwiecień"
        Time.May -> "Maj"
        Time.Jun -> "Czerwiec"
        Time.Jul -> "Lipiec"
        Time.Aug -> "Sierpień"
        Time.Sep -> "Wrzesień"
        Time.Oct -> "Październik"
        Time.Nov -> "Listopad"
        Time.Dec -> "Grudzień"

rideStopToString : RideStop -> String
rideStopToString stop =
    stop.name ++ " - " ++ stop.city



-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    { title = "Kup bilet"
    , body =
        case model.ride of
            RideSuccess ride ->
                let
                    emptyStop =
                        RideStop 0 (Time.millisToPosix 0) 0 "ERROR" "ERROR"

                    escapeMaybe =
                        Maybe.withDefault emptyStop

                    fromStop =
                        (List.head <| List.filter (\s -> s.id == ride.from) ride.stops) |> escapeMaybe

                    toStop =
                        (List.head <| List.filter (\s -> s.id == ride.to) ride.stops) |> escapeMaybe
                in
                [ Grid.row [ Row.attrs [ class "mt-1 mt-md-3" ] ]
                    [ Grid.col [ Col.attrs [ class "text-center ticket-header-date" ] ]
                        [ text <| niceDate fromStop.arrivalTime ] ]
                , Grid.row []
                    [ Grid.col []
                        [ div [ class "d-flex justify-content-center flex-wrap ticket-header" ]
                            [ div [ class "from-station" ]
                                [ span [ class "oi oi-clock" ] []
                                , span [] [ text <| niceTime fromStop.arrivalTime ]
                                , span [] [ text <| rideStopToString fromStop ]
                                ]
                            , div [ class "text-center ml-1 mr-1 ml-md-3 mr-md-3" ]
                                [ span [ class "oi oi-arrow-right" ] [] ]
                            , div [ class "to-station" ]
                                [ span [ class "oi oi-clock" ] []
                                , span [] [ text <| niceTime toStop.arrivalTime ]
                                , span [] [ text <| rideStopToString toStop ]
                                ]
                            ]
                        ]
                    ]
                ]

            RideLoading ->
                [ Grid.row []
                    [ Grid.col []
                        [ Spinner.spinner [ Spinner.attrs [ class "text-center" ] ] [] ]
                    ]
                ]

            RideFailure ->
                [ div [ class "text-center" ]
                    [ h2 [] [ text "Wystąpił błąd" ]
                    , h3 [] [ text "Spróbuj odświeżyć stronę" ]
                    ]
                ]
    }



-- HTTP


getRide : String -> Int -> Int -> Int -> Cmd Msg
getRide api fromId toId rideId =
    let
        url =
            UrlBuilder.relative
                [ "rides", String.fromInt rideId ]
                [ UrlBuilder.int "from" fromId
                , UrlBuilder.int "to" toId
                ]
    in
    Http.get
        { url = api ++ url
        , expect = Http.expectJson GotRide rideDecoder
        }



-- JSON


rideDecoder : Decode.Decoder Ride
rideDecoder =
    Decode.map6 Ride
        (Decode.field "id" Decode.int)
        (Decode.field "from" Decode.int)
        (Decode.field "to" Decode.int)
        (Decode.field "trainStops" rideStopDecoder)
        (Decode.field "train" rideTrainDecoder)
        (Decode.field "price" Decode.int)


rideStopDecoder : Decode.Decoder (List RideStop)
rideStopDecoder =
    Decode.list <|
        Decode.map5 RideStop
            (Decode.field "stopId" Decode.int)
            (Decode.field "arrivalTime" TimeIso.decoder)
            (Decode.field "stopNo" Decode.int)
            (Decode.field "name" Decode.string)
            (Decode.field "city" Decode.string)


rideTrainDecoder : Decode.Decoder RideTrain
rideTrainDecoder =
    Decode.map5 RideTrain
        (Decode.field "id" Decode.int)
        (Decode.field "name" Decode.string)
        (Decode.field "type" Decode.int)
        (Decode.field "seats" Decode.int)
        (Decode.field "wagons" Decode.int)
