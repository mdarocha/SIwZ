module Page.Login exposing (Model, Msg, init, update, view)

import Bootstrap.Button as Button
import Bootstrap.Form as Form
import Bootstrap.Form.Input as Input
import Bootstrap.Grid as Grid
import Bootstrap.Alert as Alert
import Browser.Navigation as Nav
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
    , redirect : Maybe String
    , errorText : Maybe String
    , login : LoginData
    , register : RegisterData
    }


type alias LoginData =
    { email : String
    , password : String
    }

type alias RegisterData =
    { email : String
    , password : String
    , name : String
    , surname : String
    }

type Msg
    = LoginEmailUpdate String
    | LoginPasswordUpdate String
    | LoginSubmit
    | LoginPerformed (Result Http.Error Session.User)
    | RegisterNameUpdate String
    | RegisterSurnameUpdate String
    | RegisterPasswordUpdate String
    | RegisterEmailUpdate String
    | RegisterSubmit
    | RegisterPerformed (Result Http.Error Session.User)


-- INIT


init : Session.Data -> Maybe String -> ( Model, Cmd msg )
init session redirect =
    ( Model session redirect Nothing (LoginData "" "") (RegisterData "" "" "" ""), Cmd.none )



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
                    ( Model newSession model.redirect Nothing (LoginData "" "") model.register, redirectCommand model.session.key model.redirect)

                Err _ ->
                    ( { model | errorText = Just "Wystąpił błąd" }, Cmd.none )

        RegisterNameUpdate name ->
            let
                oldRegister = model.register
                newRegister = { oldRegister | name = name }
            in
            ( { model | register = newRegister }, Cmd.none )

        RegisterSurnameUpdate surname ->
            let
                oldRegister = model.register
                newRegister = { oldRegister | surname = surname }
            in
            ( { model | register = newRegister }, Cmd.none )

        RegisterEmailUpdate email ->
            let
                oldRegister = model.register
                newRegister = { oldRegister | email = email }
            in
            ( { model | register = newRegister }, Cmd.none )

        RegisterPasswordUpdate pass ->
            let
                oldRegister = model.register
                newRegister = { oldRegister | password = pass }
            in
            ( { model | register = newRegister }, Cmd.none )

        RegisterSubmit ->
            ( model, performRegister model.session.api model.register )

        RegisterPerformed result ->
            case result of
                Ok user ->
                    let
                        oldSession = model.session
                        newSession = { oldSession | user = Just user }
                    in
                    ( Model newSession model.redirect Nothing model.login (RegisterData "" "" "" ""), redirectCommand model.session.key model.redirect)
                Err _ ->
                    ( { model | errorText = Just "Wystąpił błąd" }, Cmd.none )

redirectCommand : Nav.Key -> Maybe String -> Cmd Msg
redirectCommand key redirect =
    case redirect of
        Just url ->
            Nav.pushUrl key ("/" ++ url)
        Nothing ->
            Cmd.none

-- VIEW


view : Model -> Skeleton.Details Msg
view model =
    { title = "Zaloguj się"
    , body =
        [ Grid.row []
            [ Grid.col []
                [ case model.errorText of
                    Just err ->
                        Alert.simpleDanger [] [ text err ]
                    Nothing ->
                        text ""
                ]
            ]
        , Grid.row []
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


viewRegister : Model -> Html Msg
viewRegister model =
    Form.form []
        [ Form.row []
            [ Form.col []
                [ Form.label [ for "register-name" ] [ text "Imię" ]
                , Input.text [ Input.id "register-name", Input.value model.register.name, Input.attrs [ onInput (\v -> RegisterNameUpdate v) ] ]
                ]
            , Form.col []
                [ Form.label [ for "register-surname" ] [ text "Nazwisko" ]
                , Input.text [ Input.id "register-surname", Input.value model.register.surname, Input.attrs [ onInput (\v -> RegisterSurnameUpdate v) ] ]
                ]
            ]
        , Form.group []
            [ Form.label [ for "register-email" ] [ text "Adres e-mail" ]
            , Input.email [ Input.id "register-email", Input.value model.register.email, Input.attrs [ onInput (\v -> RegisterEmailUpdate v) ] ]
            ]
        , Form.group []
            [ Form.label [ for "register-password" ] [ text "Hasło" ]
            , Input.password [ Input.id "register-password", Input.value model.register.password, Input.attrs [ onInput (\v -> RegisterPasswordUpdate v) ] ]
            ]
        , Button.button [ Button.primary, Button.onClick RegisterSubmit ] [ text "Zarejestruj się" ]
        ]


-- HTTP


performLogin : String -> LoginData -> Cmd Msg
performLogin api login =
    Http.post
        { body = Http.jsonBody (loginEncoder login)
        , expect = Http.expectJson LoginPerformed userDecoder
        , url = api ++ "user/login"
        }

performRegister : String -> RegisterData -> Cmd Msg
performRegister api register =
    Http.post
        { body = Http.jsonBody (registerEncoder register)
        , expect = Http.expectJson RegisterPerformed userDecoder
        , url = api ++ "user/register"
        }

-- JSON


loginEncoder : LoginData -> Encode.Value
loginEncoder login =
    Encode.object
        [ ( "email", Encode.string login.email )
        , ( "password", Encode.string login.password )
        ]

registerEncoder : RegisterData -> Encode.Value
registerEncoder register =
    Encode.object
        [ ( "name", Encode.string register.name )
        , ( "surname", Encode.string register.surname )
        , ( "email", Encode.string register.email )
        , ( "password", Encode.string register.password )
        ]

userDecoder : Decode.Decoder Session.User
userDecoder =
    Decode.map5 Session.User
        (Decode.field "id" Decode.string)
        (Decode.field "email" Decode.string)
        (Decode.field "name" Decode.string)
        (Decode.field "surname" Decode.string)
        (Decode.field "token" Decode.string)
