module Session exposing (Data, User, userDecode, userEncode, isUserAdmin)

import Browser.Navigation as Nav
import Json.Decode as Decode
import Json.Encode as Encode
import Jwt

type alias User =
    { id : String
    , email : String
    , name : String
    , surname : String
    , token : String
    }

type alias UserToken =
    { isAdmin : Bool
    }


isUserAdmin : Maybe User -> Bool
isUserAdmin maybeUser =
    case maybeUser of
        Just user ->
            case Jwt.decodeToken userTokenDecoder user.token of
                Ok decodedToken ->
                    decodedToken.isAdmin
                _ ->
                    False
        Nothing ->
            False


userTokenDecoder : Decode.Decoder UserToken
userTokenDecoder =
    Decode.map UserToken
        (Decode.field "IsAdmin" Decode.bool)

userEncode : User -> Encode.Value
userEncode user =
    Encode.object
        [ ( "id", Encode.string user.id )
        , ( "email", Encode.string user.email )
        , ( "name", Encode.string user.name )
        , ( "surname", Encode.string user.surname )
        , ( "token", Encode.string user.token )
        ]


userDecode : Decode.Decoder User
userDecode =
    Decode.map5 User
        (Decode.field "id" Decode.string)
        (Decode.field "email" Decode.string)
        (Decode.field "name" Decode.string)
        (Decode.field "surname" Decode.string)
        (Decode.field "token" Decode.string)


type alias Data =
    { api : String
    , key : Nav.Key
    , user : Maybe User
    }
