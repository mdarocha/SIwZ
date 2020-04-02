module Session exposing (Session)

import Bootstrap.Navbar as Navbar
import Browser.Navigation as Nav
import Url


type alias Session =
    { api : String
    , nav : Nav.Key
    , navbarState : Navbar.State
    }
