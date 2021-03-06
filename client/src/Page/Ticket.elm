module Page.Ticket exposing (Model, Msg, init, update, view)

import Array exposing (..)
import Bootstrap.Button as Button
import Bootstrap.ButtonGroup as ButtonGroup
import Bootstrap.Form.Select as Select
import Bootstrap.Grid as Grid
import Bootstrap.Grid.Col as Col
import Bootstrap.Grid.Row as Row
import Bootstrap.Spinner as Spinner
import Browser.Navigation as Nav
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (..)
import Http
import ISO8601 as TimeIso
import Json.Decode as Decode
import Json.Encode as Encode
import Jwt.Http
import Session
import Skeleton
import Time
import Url.Builder as UrlBuilder



-- MODEL


type alias RideStop =
    { id : Int
    , arrivalTime : TimeIso.Time
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
    , discounts : Discounts
    , selectedDiscount : Maybe Discount
    , date : TimeIso.Time
    }


type alias PlaceSelector =
    { selectedWagon : Maybe Int
    , selectedSeat : Maybe Int
    , wagonShown : Maybe Int
    }


type DiscountType
    = Flat
    | Percentage


type alias Discount =
    { id : Int
    , name : String
    , value : Int
    , discountType : DiscountType
    }


type Discounts
    = DiscountsFailure
    | DiscountsLoading
    | DiscountsSuccess (List Discount)


type alias Ticket =
    { rideId : Int
    , discountId : Int
    , fromId : Int
    , toId : Int
    , wagonNumber : Int
    , seatNumber : Int
    , date : TimeIso.Time
    }



-- MSG


type Msg
    = GotRide (Result Http.Error Ride)
    | GotFreeSeats (Result Http.Error (Array TrainWagon))
    | GotDiscounts (Result Http.Error (List Discount))
    | WagonSelected Int
    | SeatSelected Int
    | DiscountSelected String
    | SubmitTicket
    | BookedTicket (Result Http.Error ())



-- INIT


init : Session.Data -> Maybe Int -> Maybe Int -> Maybe Int -> Maybe String -> ( Model, Cmd Msg )
init session from to ride date =
    let
        model =
            Model session RideLoading WagonLoading (PlaceSelector Nothing Nothing Nothing) DiscountsLoading Nothing

        emptyModel =
            model (TimeIso.fromPosix (Time.millisToPosix 0))
    in
    case ( from, to, ride ) of
        ( Just fromId, Just toId, Just rideId ) ->
            case ( session.user, date ) of
                ( Just _, Just rideDate ) ->
                    case TimeIso.fromString rideDate of
                        Ok time ->
                            let
                                rideCmd =
                                    getRide session.api fromId toId rideId time

                                seatsCmd =
                                    getFreeSeats session.api fromId toId rideId time

                                discountsCmd =
                                    getDiscounts session.api
                            in
                            ( model time, Cmd.batch [ rideCmd, seatsCmd, discountsCmd ] )

                        Err _ ->
                            ( emptyModel, Nav.pushUrl session.key "/error" )

                ( Nothing, Just rideDate ) ->
                    case TimeIso.fromString rideDate of
                        Ok time ->
                            let
                                returnUrl =
                                    UrlBuilder.relative [ "ticket" ]
                                        [ UrlBuilder.int "from" fromId
                                        , UrlBuilder.int "to" toId
                                        , UrlBuilder.int "ride" rideId
                                        , UrlBuilder.string "date" (TimeIso.toString time)
                                        ]

                                url =
                                    UrlBuilder.absolute [ "login" ] [ UrlBuilder.string "return" returnUrl ]
                            in
                            ( emptyModel, Nav.pushUrl session.key url )

                        Err _ ->
                            ( emptyModel, Nav.pushUrl session.key "/error" )

                ( _, _ ) ->
                    ( emptyModel, Nav.pushUrl session.key "/error" )

        ( _, _, _ ) ->
            ( emptyModel, Nav.pushUrl session.key "/error" )



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

        GotDiscounts result ->
            case result of
                Ok discounts ->
                    let
                        firstDiscount = List.head discounts
                    in
                    ( { model | discounts = DiscountsSuccess discounts, selectedDiscount = firstDiscount }, Cmd.none )

                Err _ ->
                    ( { model | discounts = DiscountsFailure }, Cmd.none )

        WagonSelected wagon ->
            let
                oldSelected =
                    model.selectedPlace

                newSelected =
                    { oldSelected | wagonShown = Just wagon }
            in
            ( { model | selectedPlace = newSelected }, Cmd.none )

        SeatSelected seat ->
            case model.selectedPlace.wagonShown of
                Just wagonShown ->
                    let
                        oldSelected =
                            model.selectedPlace

                        newSelected =
                            { oldSelected | selectedSeat = Just seat, selectedWagon = Just wagonShown }
                    in
                    ( { model | selectedPlace = newSelected }, Cmd.none )

                Nothing ->
                    ( model, Cmd.none )

        DiscountSelected discountIdString ->
            case model.discounts of
                DiscountsSuccess discounts ->
                    let
                        discountId =
                            String.toInt discountIdString
                                |> Maybe.withDefault -1

                        discount =
                            List.filter (\d -> d.id == discountId) discounts
                                |> List.head
                    in
                    ( { model | selectedDiscount = discount }, Cmd.none )

                _ ->
                    ( model, Cmd.none )

        SubmitTicket ->
            let
                ( selectedWagon, selectedSeat ) =
                    ( model.selectedPlace.selectedWagon, model.selectedPlace.selectedSeat )

                selectedDiscount =
                    model.selectedDiscount
            in
            case ( model.ride, model.session.user ) of
                ( RideSuccess ride, Just user ) ->
                    case ( selectedWagon, selectedSeat, selectedDiscount ) of
                        ( Just wagon, Just seat, Just discount ) ->
                            let
                                ticket =
                                    Ticket ride.id discount.id ride.from ride.to (wagon + 1) (seat + 1) model.date
                            in
                            ( model, bookTicket model.session.api user.token ticket )

                        ( _, _, _ ) ->
                            ( model, Cmd.none )

                ( _, _ ) ->
                    ( model, Nav.pushUrl model.session.key "/login" )

        BookedTicket result ->
            case result of
                Ok _ ->
                    ( model, Nav.pushUrl model.session.key "/user" )

                Err _ ->
                    ( model, Cmd.none )



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
            "Stycze??"

        Time.Feb ->
            "Luty"

        Time.Mar ->
            "Marzec"

        Time.Apr ->
            "Kwiecie??"

        Time.May ->
            "Maj"

        Time.Jun ->
            "Czerwiec"

        Time.Jul ->
            "Lipiec"

        Time.Aug ->
            "Sierpie??"

        Time.Sep ->
            "Wrzesie??"

        Time.Oct ->
            "Pa??dziernik"

        Time.Nov ->
            "Listopad"

        Time.Dec ->
            "Grudzie??"


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


shouldShowDiscountSelector : Model -> Bool
shouldShowDiscountSelector model =
    case ( model.selectedPlace.selectedWagon, model.selectedPlace.selectedSeat ) of
        ( Just _, Just _ ) ->
            True

        ( _, _ ) ->
            False


discountText : Discount -> String
discountText discount =
    case discount.discountType of
        Flat ->
            discount.name ++ " - " ++ String.fromInt discount.value ++ " z??"

        Percentage ->
            discount.name ++ " - " ++ String.fromInt discount.value ++ " %"


getPrice : RideData -> Maybe Discount -> String
getPrice rideData discount =
    case rideData of
        RideSuccess ride ->
            let
                price =
                    case discount of
                        Just dis ->
                            case dis.discountType of
                                Flat ->
                                    ride.price - dis.value

                                Percentage ->
                                    round (toFloat ride.price * (1 - 0.01 * toFloat dis.value))

                        Nothing ->
                            ride.price
            in
            String.fromInt price ++ " z??"

        _ ->
            "ERROR"



-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    { title = "Kup bilet"
    , body =
        case ( model.ride, model.seats, model.discounts ) of
            ( RideSuccess ride, WagonSuccess seats, DiscountsSuccess discounts ) ->
                let
                    emptyStop =
                        RideStop 0 (TimeIso.fromPosix (Time.millisToPosix 0)) 0 "ERROR" "ERROR"

                    escapeMaybe =
                        Maybe.withDefault emptyStop

                    fromStop =
                        (List.head <| List.filter (\s -> s.id == ride.from) ride.stops) |> escapeMaybe

                    toStop =
                        (List.head <| List.filter (\s -> s.id == ride.to) ride.stops) |> escapeMaybe
                in
                [ Grid.row [] [ Grid.col [ Col.attrs [ class "text-center ticket-header" ] ] [ text <| niceDate (TimeIso.toPosix fromStop.arrivalTime) ] ]
                , Grid.row [] [ Grid.col [ Col.attrs [ class "text-center ticket-header" ] ] [ text ride.train.name ] ]
                , Grid.row [] [ Grid.col [ Col.attrs [ class "text-center ticket-header ticket-header-last" ] ] [ viewHeaderFromTo fromStop toStop ] ]
                , Grid.row [] [ Grid.col [ Col.attrs [ class "text-center ticket-info" ] ] [ viewHelpText model ] ]
                , Grid.row [ Row.attrs [ class "justify-content-center" ] ]
                    [ Grid.col [ Col.xsAuto ] [ viewWagonSelector seats model.selectedPlace.wagonShown model.selectedPlace.selectedWagon ]
                    ]
                , Grid.row [ Row.attrs [ class "justify-content-center" ] ]
                    [ Grid.col [ Col.xsAuto ]
                        [ viewSeatSelector (selectedWagonSeats seats model.selectedPlace.wagonShown) model.selectedPlace.selectedSeat model.selectedPlace.wagonShown model.selectedPlace.selectedWagon ]
                    ]
                , Grid.row [ Row.attrs [ class "mt-3 justify-content-center" ] ]
                    [ Grid.col [ Col.xsAuto ]
                        [ viewDiscountSelector discounts model.selectedDiscount (shouldShowDiscountSelector model) ]
                    ]
                , Grid.row [ Row.attrs [ class "mt-3 justify-content-center" ] ]
                    [ Grid.col [ Col.xsAuto ]
                        [ viewNextButton model ]
                    ]
                ]

            ( RideFailure, _, _ ) ->
                [ div [ class "text-center" ]
                    [ h2 [] [ text "Wyst??pi?? b????d" ]
                    , h3 [] [ text "Spr??buj od??wie??y?? stron??" ]
                    ]
                ]

            ( _, WagonFailure, _ ) ->
                [ div [ class "text-center" ]
                    [ h2 [] [ text "Wyst??pi?? b????d" ]
                    , h3 [] [ text "Spr??buj od??wie??y?? stron??" ]
                    ]
                ]

            ( _, _, DiscountsFailure ) ->
                [ div [ class "text-center" ]
                    [ h2 [] [ text "Wyst??pi?? b????d" ]
                    , h3 [] [ text "Spr??buj od??wie??y?? stron??" ]
                    ]
                ]

            ( _, _, _ ) ->
                [ Grid.row [ Row.attrs [ class "justify-content-center" ] ]
                    [ Grid.col [ Col.xsAuto ]
                        [ Spinner.spinner [] [] ]
                    ]
                ]
    }


viewHelpText : Model -> Html Msg
viewHelpText model =
    case ( model.selectedPlace.wagonShown, model.selectedPlace.selectedSeat, model.selectedDiscount ) of
        ( Nothing, Nothing, _ ) ->
            text "Wybierz wagon w poci??gu"

        ( Just _, Nothing, _ ) ->
            text "Wybierz miejsce w wagonie"

        ( Just _, Just _, Nothing ) ->
            text "Wybierz zni??k??, je??li aplikuje si??"

        ( Just _, Just _, Just _ ) ->
            text "Bilet gotowy - wybierz zni??ke, je??li si?? aplikuje i potwierd?? kupno"

        ( _, _, _ ) ->
            text "??\\_(???)_/??"


viewHeaderFromTo : RideStop -> RideStop -> Html Msg
viewHeaderFromTo from to =
    div [ class "ticket-from-to d-flex flex-wrap justify-content-center" ]
        [ div [ class "from-station" ]
            [ div [ class "station-time" ]
                [ span [ class "oi oi-clock" ] []
                , span [] [ text <| niceTime (TimeIso.toPosix from.arrivalTime) ]
                ]
            , span [] [ text <| rideStopToString from ]
            ]
        , div []
            [ span [ class "oi oi-arrow-right" ] [] ]
        , div [ class "to-station" ]
            [ div [ class "station-time" ]
                [ span [ class "oi oi-clock" ] []
                , span [] [ text <| niceTime (TimeIso.toPosix to.arrivalTime) ]
                ]
            , span [] [ text <| rideStopToString to ]
            ]
        ]


viewWagonSelector : Array TrainWagon -> Maybe Int -> Maybe Int -> Html Msg
viewWagonSelector seats selected seatSelected =
    let
        isSelected i =
            case selected of
                Just s ->
                    i == s

                Nothing ->
                    False

        buttonType i =
            case seatSelected of
                Just s ->
                    if i == s then
                        Button.primary

                    else
                        Button.secondary

                Nothing ->
                    Button.secondary

        button num _ =
            ButtonGroup.radioButton (isSelected num)
                [ buttonType num, Button.attrs [ onClick (WagonSelected num) ] ]
                [ text <| String.fromInt (num + 1) ]
    in
    ButtonGroup.radioButtonGroup [] <|
        (Array.indexedMap button seats |> Array.toList)


viewSeatSelector : Maybe (List Bool) -> Maybe Int -> Maybe Int -> Maybe Int -> Html Msg
viewSeatSelector maybeSeats maybeSelected maybeSelectedWagon maybeShownWagon =
    let
        isSelected num =
            case ( maybeSelected, maybeSelectedWagon, maybeShownWagon ) of
                ( Just selected, Just selectedWagon, Just shownWagon ) ->
                    selected == num && selectedWagon == shownWagon

                ( _, _, _ ) ->
                    False

        seat num avaible =
            div
                [ classList [ ( "enabled", avaible ), ( "selected", isSelected num ) ]
                , attribute "tabindex" (String.fromInt num)
                , if avaible then
                    onClick (SeatSelected num)

                  else
                    class ""
                ]
                [ text <| String.fromInt (num + 1) ]
    in
    case ( maybeSeats, maybeSelectedWagon ) of
        ( Just seats, Just wagon ) ->
            div
                [ class "seat-selector seat-selector-open"
                , attribute "data-column-size" <| String.fromInt (List.length seats // 4)
                , attribute "data-wagon-n" <| String.fromInt (wagon + 1)
                ]
            <|
                List.indexedMap seat seats

        ( _, _ ) ->
            div [] []


viewDiscountSelector : List Discount -> Maybe Discount -> Bool -> Html Msg
viewDiscountSelector discounts selected shouldShow =
    case shouldShow of
        True ->
            let
                item discount =
                    Select.item [ value <| String.fromInt discount.id ]
                        [ text <| discountText discount ]
            in
            Select.select [ Select.onChange DiscountSelected ] <|
                List.map item discounts

        False ->
            div [] []


viewNextButton : Model -> Html Msg
viewNextButton model =
    case ( model.selectedPlace.selectedWagon, model.selectedPlace.selectedSeat, model.selectedDiscount ) of
        ( Just _, Just _, Just _ ) ->
            Button.button
                [ Button.primary
                , Button.large
                , Button.attrs [ onClick SubmitTicket ]
                ]
                [ text <| "Kup - " ++ getPrice model.ride model.selectedDiscount ]

        ( _, _, _ ) ->
            div [] []



-- HTTP


getRide : String -> Int -> Int -> Int -> TimeIso.Time -> Cmd Msg
getRide api fromId toId rideId date =
    let
        url =
            UrlBuilder.relative
                [ "rides", String.fromInt rideId ]
                [ UrlBuilder.int "from" fromId
                , UrlBuilder.int "to" toId
                , UrlBuilder.string "date" (TimeIso.toString date)
                ]
    in
    Http.get
        { url = api ++ url
        , expect = Http.expectJson GotRide rideDecoder
        }


getFreeSeats : String -> Int -> Int -> Int -> TimeIso.Time -> Cmd Msg
getFreeSeats api fromId toId rideId date =
    let
        url =
            UrlBuilder.relative
                [ "rides", String.fromInt rideId, "freeSeats" ]
                [ UrlBuilder.int "from" fromId
                , UrlBuilder.int "to" toId
                , UrlBuilder.string "date" (TimeIso.toString date)
                ]
    in
    Http.get
        { url = api ++ url
        , expect = Http.expectJson GotFreeSeats freeSeatsDecoder
        }


getDiscounts : String -> Cmd Msg
getDiscounts api =
    Http.get
        { url = api ++ "discounts"
        , expect = Http.expectJson GotDiscounts discountDecoder
        }


bookTicket : String -> String -> Ticket -> Cmd Msg
bookTicket api token ticket =
    Jwt.Http.post token
        { body = Http.jsonBody (ticketEncoder ticket)
        , expect = Http.expectWhatever BookedTicket
        , url = api ++ "tickets"
        }



-- JSON


ticketEncoder : Ticket -> Encode.Value
ticketEncoder ticket =
    Encode.object
        [ ( "rideId", Encode.int ticket.rideId )
        , ( "discountId", Encode.int ticket.discountId )
        , ( "fromId", Encode.int ticket.fromId )
        , ( "toId", Encode.int ticket.toId )
        , ( "wagonNo", Encode.int ticket.wagonNumber )
        , ( "seatNo", Encode.int ticket.seatNumber )
        , ( "rideDate", encodeTime ticket.date )
        ]


encodeTime : TimeIso.Time -> Encode.Value
encodeTime =
    TimeIso.toString >> Encode.string


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
            (Decode.field "arrivalTime" TimeIso.decode)
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


discountDecoder : Decode.Decoder (List Discount)
discountDecoder =
    Decode.list <|
        Decode.map4 Discount
            (Decode.field "id" Decode.int)
            (Decode.field "type" Decode.string)
            (Decode.field "value" Decode.int)
            (Decode.field "valueType" discountTypeDecoder)


discountTypeDecoder : Decode.Decoder DiscountType
discountTypeDecoder =
    Decode.int
        |> Decode.andThen
            (\i ->
                case i of
                    0 ->
                        Decode.succeed Percentage

                    1 ->
                        Decode.succeed Flat

                    somethingElse ->
                        Decode.fail <| "Unknown discount type: " ++ String.fromInt somethingElse
            )
