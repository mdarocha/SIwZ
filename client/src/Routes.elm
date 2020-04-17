module Routes exposing (Route(..), fromUrl)

import Url exposing (Url)
import Url.Parser exposing ((</>), Parser, map, oneOf, s, top)


type Route
    = AdminTrainStopsRoute
    | AboutRoute
    | SearchRoute
    | HomeRoute
    | LoginRoute


parser : Parser (Route -> a) a
parser =
    oneOf
        [ map HomeRoute top
        , map SearchRoute (s "search")
        , map AboutRoute (s "about")
        , map AdminTrainStopsRoute (s "admin" </> s "stops")
        , map LoginRoute (s "login")
        ]


fromUrl : Url -> Maybe Route
fromUrl url =
    url |> Url.Parser.parse parser
