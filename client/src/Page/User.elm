module Page.User exposing (Model, Msg, init, update, view)

import Bootstrap.Button as Button
import Bootstrap.Card as Card
import Bootstrap.Card.Block as Block
import Bootstrap.Grid as Grid
import Bootstrap.Grid.Col as Col
import Bootstrap.Spinner as Spinner
import Bootstrap.Text as Text
import Browser.Navigation as Nav
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (onClick)
import Http
import ISO8601 as TimeIso
import Json.Decode as Decode
import Jwt.Http
import Ports
import Session
import Skeleton
import Time
import Set exposing (Set)


type alias Model =
    { session : Session.Data
    , user : Session.User
    , tickets : Tickets
    , openCancelQuestions : Set Int
    }

type alias Ticket =
    { id : Int
    , from : String
    , to : String
    , train : String
    , price : Int
    , wagon : Int
    , seat : Int
    }


type Tickets
    = Error
    | Loading
    | Success (List Ticket)


type Msg
    = LogoutClicked
    | GotTickets (Result Http.Error (List Ticket))
    | CancelTicket Int
    | CancelTicketQuestion Int
    | CloseQuestion Int
    | RevokedTicket (Result Http.Error ())


-- INIT


init : Session.Data -> ( Model, Cmd Msg )
init session =
    case session.user of
        Just user ->
            ( Model session user Loading Set.empty, getTickets session.api user.token )

        Nothing ->
            ( Model session (Session.User "" "" "" "" "") Error Set.empty, Nav.pushUrl session.key "/login" )



-- UPDATE


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        LogoutClicked ->
            let
                oldSession =
                    model.session

                newSession =
                    { oldSession | user = Nothing }

                key =
                    newSession.key

                setSessionCmd =
                    Ports.clearUserSession ()

                redirectCmd =
                    Nav.pushUrl key "/"
            in
            ( { model | session = newSession }, Cmd.batch [ redirectCmd, setSessionCmd ] )

        GotTickets result ->
            case result of
                Ok tickets ->
                    ( { model | tickets = Success tickets }, Cmd.none )

                Err _ ->
                    ( { model | tickets = Error }, Cmd.none )

        CancelTicket id ->
            ( model, revokeTicket model.session.api model.user.token id )


        CancelTicketQuestion id ->
            let
                newSet = Set.insert id model.openCancelQuestions
            in
            ( { model | openCancelQuestions = newSet }, Cmd.none )

        CloseQuestion id ->
            let
                newSet = Set.remove id model.openCancelQuestions
            in
            ( { model | openCancelQuestions = newSet }, Cmd.none )

        RevokedTicket result ->
            case result of
                Ok _ ->
                    ( model, getTickets model.session.api model.user.token )

                Err _ ->
                    ( { model | tickets = Error }, Cmd.none )


-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    let
        user =
            model.user
    in
    { title = user.name ++ " " ++ user.surname
    , body =
        [ Grid.row []
            [ Grid.col [ Col.md8 ] [ h2 [ class "mt-2" ] [ text (user.name ++ " " ++ user.surname) ] ]
            , Grid.col [ Col.pushMd8, Col.md4, Col.middleMd ]
                [ Button.button [ Button.danger, Button.small, Button.onClick LogoutClicked ] [ text "Wyloguj się" ] ]
            ]
        , viewTicketsList model
        ]
    }


viewTicketsList : Model -> Html Msg
viewTicketsList model =
    case model.tickets of
        Error ->
            h4 [ class "text-center" ] [ text "Wystąpił błąd" ]

        Loading ->
            div [ class "mt-4 text-center" ] [ Spinner.spinner [] [] ]

        Success tickets ->
            div [] <| List.map (viewTicket model) tickets


viewTicket : Model -> Ticket -> Html Msg
viewTicket model ticket =
    Card.config [ Card.attrs [ class "mt-5 ticket-card" ] ]
        |> Card.headerH4 [ class "d-flex flex-wrap" ]
            [ div []
                [ span [ class "font-italic pr-2" ] [ text ticket.train ]
                , span [] [ text (ticket.from ++ " > " ++ ticket.to) ]
                ]
            ]
        |> Card.block [ Block.attrs [ class "seat-info" ] ]
            [ Block.text [] [ text ("Wagon " ++ String.fromInt ticket.wagon) ]
            , Block.text [] [ text ("Miejsce " ++ String.fromInt ticket.seat) ]
            ]
        |> Card.block [ Block.align Text.alignXsRight ]
            [ Block.custom <| viewCancelForm model ticket ]
        |> Card.view


viewCancelForm : Model -> Ticket -> Html Msg
viewCancelForm model ticket =
    if Set.member ticket.id model.openCancelQuestions then
        div [ class "ticket-question" ]
            [ span [] [ text "Czy na pewno chcesz anulować bilet?" ]
            , Button.button [ Button.danger, Button.attrs [ class "align-baseline", onClick (CancelTicket ticket.id) ] ]
                [ text "Tak" ]
            , Button.button [ Button.outlineSecondary, Button.attrs [ class "align-baseline", onClick (CloseQuestion ticket.id) ] ]
                [ text "Nie" ]
            ]
    else
        div [ class "ride-price" ]
            [ span [ class "oi oi-paperclip" ] []
            , span [] [ text <| String.fromInt ticket.price ++ " zł" ]
            , Button.button
                [ Button.danger
                , Button.attrs [ class "align-baseline" , onClick (CancelTicketQuestion ticket.id) ]
                ]
                [ text "Anuluj bilet" ]
            ]


-- HTTP


getTickets : String -> String -> Cmd Msg
getTickets api token =
    Jwt.Http.get token
        { url = api ++ "tickets"
        , expect = Http.expectJson GotTickets ticketsDecoder
        }


revokeTicket : String -> String -> Int -> Cmd Msg
revokeTicket api token id =
    Jwt.Http.delete token
        { url = api ++ "tickets/" ++ (String.fromInt id)
        , expect = Http.expectWhatever RevokedTicket
        }

-- JSON


ticketsDecoder : Decode.Decoder (List Ticket)
ticketsDecoder =
    Decode.list <|
        Decode.map7 Ticket
            (Decode.field "id" Decode.int)
            (Decode.field "from" Decode.string)
            (Decode.field "to" Decode.string)
            (Decode.field "trainName" Decode.string)
            (Decode.field "price" Decode.int)
            (Decode.field "wagonNo" Decode.int)
            (Decode.field "seatNo" Decode.int)
