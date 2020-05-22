port module Ports exposing (..)

import Json.Encode as Encode

port setUserSession : Encode.Value -> Cmd msg
