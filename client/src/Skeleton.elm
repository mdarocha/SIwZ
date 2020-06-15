module Skeleton exposing (Details, view, viewFooter)

import Bootstrap.Grid as Grid
import Browser
import Html exposing (..)
import Html.Attributes exposing (class, href, id)


type alias Details msg =
    { title : String
    , body : List (Html msg)
    }


view : (a -> msg) -> Details a -> Html msg -> Browser.Document msg
view toMsg details navbar =
    { title =
        details.title ++ " | SIwZ Trains"
    , body =
        [ navbar
        , Html.map toMsg <| Grid.container [ id "wrap", class "pt-2" ] details.body
        , viewFooter
        ]
    }


viewFooter : Html msg
viewFooter =
    footer [] [ text "© 2020 Politechnika Krakowska" ]
