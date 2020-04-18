module Page.Login exposing (Model, Msg, init, update, view)

import Bootstrap.Button as Button
import Bootstrap.Form as Form
import Bootstrap.Form.Input as Input
import Bootstrap.Grid as Grid
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (onInput)
import Http
import Json.Decode as Decode
import Json.Encode as Encode
import Session
import Skeleton


type alias Model =
    { session : Session.Data
    , login : LoginData
    }


type alias LoginData =
    { email : String
    , password : String
    }


type Msg
    = LoginEmailUpdate String
    | LoginPasswordUpdate String
    | LoginSubmit
    | LoginPerformed (Result Http.Error Session.User)



-- INIT


init : Session.Data -> ( Model, Cmd msg )
init session =
    ( Model session (LoginData "" ""), Cmd.none )



-- UPDATE


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        LoginEmailUpdate email ->
            let
                oldLogin =
                    model.login

                newLogin =
                    { oldLogin | email = email }
            in
            ( { model | login = newLogin }, Cmd.none )

        LoginPasswordUpdate pass ->
            let
                oldLogin =
                    model.login

                newLogin =
                    { oldLogin | password = pass }
            in
            ( { model | login = newLogin }, Cmd.none )

        LoginSubmit ->
            ( model, performLogin model.session.api model.login )

        LoginPerformed result ->
            case result of
                Ok user ->
                    let
                        oldSession =
                            model.session

                        newSession =
                            { oldSession | user = Just user }
                    in
                    ( Model newSession (LoginData "" ""), Cmd.none )

                Err _ ->
                    ( model, Cmd.none )



-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    { title = "Zaloguj się"
    , body =
        [ Grid.row []
            [ Grid.col [] [ h2 [] [ text "Zaloguj się" ], viewLogin model ]
            , Grid.col [] [ h2 [] [ text "Zarejestruj się" ], viewRegister model ]
            ]
        ]
    }


viewLogin : Model -> Html Msg
viewLogin model =
    Form.form []
        [ Form.group []
            [ Form.label [ for "login-email" ] [ text "Adres e-mail" ]
            , Input.email [ Input.id "login-email", Input.value model.login.email, Input.attrs [ onInput (\v -> LoginEmailUpdate v) ] ]
            ]
        , Form.group []
            [ Form.label [ for "login-password" ] [ text "Hasło" ]
            , Input.password [ Input.id "login-password", Input.value model.login.password, Input.attrs [ onInput (\v -> LoginPasswordUpdate v) ] ]
            ]
        , Button.button [ Button.primary, Button.onClick LoginSubmit ] [ text "Zaloguj się" ]
        ]


viewRegister : Model -> Html msg
viewRegister model =
    text "register"



-- HTTP


performLogin : String -> LoginData -> Cmd Msg
performLogin api login =
    Http.post
        { body = Http.jsonBody (loginEncoder login)
        , expect = Http.expectJson LoginPerformed userDecoder
        , url = api ++ "user/login"
        }



-- JSON


loginEncoder : LoginData -> Encode.Value
loginEncoder login =
    Encode.object
        [ ( "email", Encode.string login.email )
        , ( "password", Encode.string login.password )
        ]


userDecoder : Decode.Decoder Session.User
userDecoder =
    Decode.map5 Session.User
        (Decode.field "id" Decode.string)
        (Decode.field "email" Decode.string)
        (Decode.field "name" Decode.string)
        (Decode.field "surname" Decode.string)
        (Decode.field "token" Decode.string)
