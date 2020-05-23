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
import Bootstrap.ButtonGroup as ButtonGroup
import Bootstrap.Button as Button
import Array exposing (..)

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


type alias TrainWagon =
    { wagonNumber : Int
    , seats : List Bool
    }

type TrainWagons
    = WagonFailure
    | WagonLoading
    | WagonSuccess (Array TrainWagon)

type alias Model =
    { session : Session.Data
    , ride : RideData
    , seats : TrainWagons
    , selectedPlace : PlaceSelector
    }

type alias PlaceSelector =
    { selectedWagon : Maybe Int
    , selectedSeat : Maybe Int
    }

-- MSG


type Msg
    = GotRide (Result Http.Error Ride)
    | GotFreeSeats (Result Http.Error (Array TrainWagon))
    | WagonSelected Int

-- INIT


init : Session.Data -> Maybe Int -> Maybe Int -> Maybe Int -> ( Model, Cmd Msg )
init session from to ride =
    let
        model =
            Model session RideLoading WagonLoading (PlaceSelector Nothing Nothing)
    in
    case ( from, to, ride ) of
        ( Just fromId, Just toId, Just rideId ) ->
            case session.user of
                Just _ ->
                    let
                        rideCmd =
                            getRide session.api fromId toId rideId
                        seatsCmd =
                            getFreeSeats session.api fromId toId rideId
                    in
                    ( model, Cmd.batch [ rideCmd, seatsCmd ] )

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
        GotFreeSeats result ->
            case result of
                Ok seats ->
                    ( { model | seats = WagonSuccess seats }, Cmd.none )
                Err _ ->
                    ( { model | seats = WagonFailure }, Cmd.none )
        WagonSelected wagon ->
            let
                oldSelected = model.selectedPlace
                newSelected = { oldSelected | selectedWagon = Just wagon }
            in
            ( { model | selectedPlace = newSelected }, Cmd.none )

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
        Time.Jan ->
            "Styczeń"

        Time.Feb ->
            "Luty"

        Time.Mar ->
            "Marzec"

        Time.Apr ->
            "Kwiecień"

        Time.May ->
            "Maj"

        Time.Jun ->
            "Czerwiec"

        Time.Jul ->
            "Lipiec"

        Time.Aug ->
            "Sierpień"

        Time.Sep ->
            "Wrzesień"

        Time.Oct ->
            "Październik"

        Time.Nov ->
            "Listopad"

        Time.Dec ->
            "Grudzień"


rideStopToString : RideStop -> String
rideStopToString stop =
    stop.name ++ " - " ++ stop.city

selectedWagonSeats : Array TrainWagon -> Maybe Int -> Maybe (List Bool)
selectedWagonSeats wagons selectedWagon =
    case selectedWagon of
        Just selected ->
            case Array.get selected wagons of
                Just wagon ->
                    Just wagon.seats
                Nothing ->
                    Nothing
        Nothing ->
            Nothing

-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    { title = "Kup bilet"
    , body =
        case ( model.ride, model.seats ) of
            ( RideSuccess ride, WagonSuccess seats ) ->
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
                [ Grid.row [] [ Grid.col [ Col.attrs [ class "text-center ticket-header" ] ] [ text <| niceDate fromStop.arrivalTime ] ]
                , Grid.row [] [ Grid.col [ Col.attrs [ class "text-center ticket-header" ] ] [ text ride.train.name ] ]
                , Grid.row [] [ Grid.col [ Col.attrs [ class "text-center ticket-header ticket-header-last" ] ] [ viewHeaderFromTo fromStop toStop ] ]
                , Grid.row [] [ Grid.col [ Col.attrs [ class "text-center ticket-info" ] ] [ text "Wybierz miejsce" ] ]
                , Grid.row [ Row.attrs [ class "justify-content-center" ] ]
                    [ Grid.col [ Col.xsAuto ] [ viewWagonSelector seats model.selectedPlace.selectedWagon ]
                    ]
                , Grid.row [ Row.attrs [ class "justify-content-center" ] ]
                    [ Grid.col [ Col.xsAuto ]
                        [ viewSeatSelector (selectedWagonSeats seats model.selectedPlace.selectedWagon) model.selectedPlace.selectedSeat ] ]
                ]

            ( RideFailure, _ ) ->
                [ div [ class "text-center" ]
                    [ h2 [] [ text "Wystąpił błąd" ]
                    , h3 [] [ text "Spróbuj odświeżyć stronę" ]
                    ]
                ]

            ( _, WagonFailure ) ->
                [ div [ class "text-center" ]
                    [ h2 [] [ text "Wystąpił błąd" ]
                    , h3 [] [ text "Spróbuj odświeżyć stronę" ]
                    ]
                ]

            ( _, _ ) ->
                [ Grid.row [ Row.attrs [ class "justify-content-center" ] ]
                    [ Grid.col [ Col.xsAuto ]
                        [ Spinner.spinner [] [] ]
                    ]
                ]
    }


viewHeaderFromTo : RideStop -> RideStop -> Html Msg
viewHeaderFromTo from to =
    div [ class "ticket-from-to d-flex flex-wrap justify-content-center" ]
        [ div [ class "from-station" ]
            [ div [ class "station-time" ]
                [ span [ class "oi oi-clock" ] []
                , span [] [ text <| niceTime from.arrivalTime ]
                ]
            , span [] [ text <| rideStopToString from ]
            ]
        , div []
            [ span [ class "oi oi-arrow-right" ] [] ]
        , div [ class "to-station" ]
            [ div [ class "station-time" ]
                [ span [ class "oi oi-clock" ] []
                , span [] [ text <| niceTime to.arrivalTime ]
                ]
            , span [] [ text <| rideStopToString to ]
            ]
        ]

viewWagonSelector : Array TrainWagon -> Maybe Int -> Html Msg
viewWagonSelector seats selected =
    let
        isSelected i =
            case selected of
                Just s ->
                    i == s
                Nothing ->
                    False

        button num _ =
            ButtonGroup.radioButton (isSelected num)
                [ Button.secondary, Button.attrs [ onClick (WagonSelected num) ] ]
                [ text <| String.fromInt (num + 1) ]
    in
    ButtonGroup.radioButtonGroup []
        <| (Array.indexedMap button seats |> Array.toList)

viewSeatSelector : Maybe (List Bool) -> Maybe Int -> Html Msg
viewSeatSelector maybeSeats maybeSelected =
    let
        isSelected num =
            case maybeSelected of
                Just selected ->
                    selected == num
                Nothing ->
                    False

        seat num avaible =
            div [ classList [("enabled", avaible), ("selected", isSelected num)]
                , attribute "tabindex" (String.fromInt num) ]
                [ text <| String.fromInt (num + 1) ]
    in
    case maybeSeats of
        Just seats ->
            div [ class "seat-selector"
                , attribute "data-column-size" <| String.fromInt (List.length seats // 4) ]
                <| List.indexedMap seat seats
        Nothing ->
            div [ class "ticket-info text-center mt-3" ] [ text "Wybierz wagon w pociągu" ]

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


getFreeSeats : String -> Int -> Int -> Int -> Cmd Msg
getFreeSeats api fromId toId rideId =
    let
        url =
            UrlBuilder.relative
                [ "rides", String.fromInt rideId, "freeSeats" ]
                [ UrlBuilder.int "from" fromId
                , UrlBuilder.int "to" toId
                ]
    in
    Http.get
        { url = api ++ url
        , expect = Http.expectJson GotFreeSeats freeSeatsDecoder
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

freeSeatsDecoder : Decode.Decoder (Array TrainWagon)
freeSeatsDecoder =
    Decode.array <|
        Decode.map2 TrainWagon
            (Decode.field "wagonNo" Decode.int)
            (Decode.field "seats" <| Decode.list Decode.bool)
