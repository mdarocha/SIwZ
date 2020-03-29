module Session exposing (Session)

import Browser.Navigation as Nav
import Url


type alias Session =
    { api : String
    , nav : Nav.Key
    }
