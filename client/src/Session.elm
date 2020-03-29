module Session exposing (Session)

import Browser.Navigation as Nav


type alias Session =
    { api : String
    , nav : Nav.Key
    }
