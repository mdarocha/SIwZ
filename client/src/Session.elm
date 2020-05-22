module Session exposing (Data, User, userEncode, userDecode)

import Browser.Navigation as Nav
import Json.Decode as Decode
import Json.Encode as Encode

type alias User =
    { id : String
    , email : String
    , name : String
    , surname : String
    , token : String
    }

userEncode : User -> Encode.Value
userEncode user =
    Encode.object
        [ ("id", Encode.string user.id)
        , ("email", Encode.string user.email)
        , ("name", Encode.string user.name)
        , ("surname", Encode.string user.surname)
        , ("token", Encode.string user.token)
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
