module Page.Ticket exposing (Model, Msg, init, update, view)

import Html exposing (..)
import Http
import Iso8601 as TimeIso
import Json.Decode as Decode
import Session
import Skeleton
import Time
import Url.Builder as UrlBuilder
import Browser.Navigation as Nav


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
    }



-- MSG


type Msg
    = GotRide (Result Http.Error Ride)



-- INIT


init : Session.Data -> Maybe Int -> Maybe Int -> Maybe Int -> ( Model, Cmd Msg )
init session from to ride =
    case ( from, to, ride ) of
        (Just fromId, Just toId, Just rideId ) ->
            case session.user of
                Just user ->
                    ( Model session, getRide session.api fromId toId rideId )
                Nothing ->
                    let
                        returnUrl = UrlBuilder.relative [ "ticket" ]
                            [ UrlBuilder.int "from" fromId
                            , UrlBuilder.int "to" toId
                            , UrlBuilder.int "ride" rideId
                            ]
                        url = UrlBuilder.absolute [ "login" ] [ UrlBuilder.string "return" returnUrl ]
                    in
                        ( Model session, Nav.pushUrl session.key url )
        (_, _, _) ->
            ( Model session, Nav.pushUrl session.key "/" )


-- UPDATE


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        GotRide result ->
            ( model, Cmd.none )



-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    { title = "Kup bilet"
    , body = [ text "ticket" ]
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
