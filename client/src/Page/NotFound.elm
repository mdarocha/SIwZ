module Page.NotFound exposing (view)

import Html exposing (..)



-- VIEW


view : { title : String, content : Html msg }
view =
    { title = "Not Found"
    , content =
        text "Not Found"
    }
