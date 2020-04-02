module Utils exposing (dynamicActive)

import Html exposing (..)
import Html.Attributes exposing (..)
import Routes exposing (..)


dynamicActive : Route -> Html.Attribute msg
dynamicActive route =
    classList [ ( "active", isActive route ) ]


isActive : Route -> Bool
isActive route =
    case route of
        Routes.SearchRoute ->
            True

        Routes.AdminTrainStopsRoute ->
            True

        Routes.AboutRoute ->
            True

        _ ->
            False
