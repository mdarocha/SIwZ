module Skeleton exposing (Details, view, viewFooter)

import Browser
import Bootstrap.Grid as Grid
import Html exposing (..)
import Html.Attributes exposing (href, class)


type alias Details msg =
    { title : String
    , body : List (Html msg)
    }


view : (a -> msg) -> Details a -> Html msg -> Browser.Document msg
view toMsg details navbar =
    { title =
        details.title
    , body =
        [ navbar
        , Html.map toMsg <| Grid.container [ class "pt-2" ] details.body
        , viewFooter
        ]
    }


viewFooter : Html msg
viewFooter =
    footer [] [ text "© 2137 lololololol" ]
