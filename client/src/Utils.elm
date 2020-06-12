module Utils exposing (..)

import Time


niceTime : Time.Posix -> String
niceTime time =
    String.padLeft 2 '0' <|
        String.fromInt (Time.toHour Time.utc time)
            ++ ":"
            ++ (String.padLeft 2 '0' <| String.fromInt (Time.toMinute Time.utc time))
