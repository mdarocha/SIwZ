module Routes exposing (Route(..), fromUrl)

import Url exposing (Url)
import Url.Parser exposing ((</>), (<?>), Parser, map, oneOf, s, top)
import Url.Parser.Query as Query

type Route
    = AdminTrainStopsRoute
    | AboutRoute
    | SearchRoute
    | HomeRoute
    | LoginRoute (Maybe String)


parser : Parser (Route -> a) a
parser =
    oneOf
        [ map HomeRoute top
        , map SearchRoute (s "search")
        , map AboutRoute (s "about")
        , map AdminTrainStopsRoute (s "admin" </> s "stops")
        , map LoginRoute (s "login" <?> Query.string "return")
        ]


fromUrl : Url -> Maybe Route
fromUrl url =
    url |> Url.Parser.parse parser
