module Routes exposing (Route(..), fromUrl)

import Url exposing (Url)
import Url.Parser exposing ((</>), Parser, int, map, oneOf, s, string, top)


type Route
    = AdminTrainStops
    | AboutRoute
    | SearchRoute
    | Root


parser : Parser (Route -> a) a
parser =
    oneOf
        [ map Root top
        , map SearchRoute (s "search")
        , map AboutRoute (s "about")
        , map AdminTrainStops (s "admin" </> s "stops")
        ]


fromUrl : Url -> Maybe Route
fromUrl url =
    url |> Url.Parser.parse parser
