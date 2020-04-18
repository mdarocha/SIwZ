module Session exposing (Data, User)


type alias User =
    { id : String
    , email : String
    , name : String
    , surname : String
    , token : String
    }


type alias Data =
    { api : String
    , user : Maybe User
    }
