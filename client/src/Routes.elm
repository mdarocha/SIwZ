module Routes exposing (Route(..), fromUrl)

import Url exposing (Url)
import Url.Parser exposing ((</>), (<?>), Parser, map, oneOf, s, top)
import Url.Parser.Query as Query


type Route
    = AdminTrainStopsRoute
    | AboutRoute
    | SearchRoute (Maybe Int) (Maybe Int)
    | TicketRoute (Maybe Int) (Maybe Int) (Maybe Int) (Maybe String)
    | LoginRoute (Maybe String)
    | UserRoute
    | RootRoute


parser : Parser (Route -> a) a
parser =
    oneOf
        [ map RootRoute top
        , map SearchRoute (s "search" <?> Query.int "from" <?> Query.int "to")
        , map TicketRoute (s "ticket" <?> Query.int "from" <?> Query.int "to" <?> Query.int "ride" <?> Query.string "date")
        , map AboutRoute (s "about")
        , map AdminTrainStopsRoute (s "admin" </> s "stops")
        , map LoginRoute (s "login" <?> Query.string "return")
        , map UserRoute (s "user")
        ]


fromUrl : Url -> Maybe Route
fromUrl url =
    url |> Url.Parser.parse parser
