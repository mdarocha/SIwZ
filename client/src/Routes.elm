module Routes exposing (Route(..), fromUrl)

import Url exposing (Url)
import Url.Parser exposing ((</>), Parser, int, map, oneOf, s, string)


type Route
    = AdminTrainRoutes


parser : Parser (Route -> a) a
parser =
    oneOf
        [ map AdminTrainRoutes (s "admin" </> s "routes")
        ]


fromUrl : Url -> Maybe Route
fromUrl url =
    url |> Url.Parser.parse parser
