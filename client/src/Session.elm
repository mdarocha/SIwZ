module Session exposing (Data, User)
import Browser.Navigation as Nav

type alias User =
    { id : String
    , email : String
    , name : String
    , surname : String
    , token : String
    }


type alias Data =
    { api : String
    , key : Nav.Key
    , user : Maybe User
    }
