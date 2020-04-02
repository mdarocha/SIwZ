module Skeleton exposing (Details, Msg, view, viewFooter, viewHeader)

import Bootstrap.Navbar as Navbar
import Browser
import Html exposing (..)
import Html.Attributes exposing (href)
import Routes
import Utils exposing (..)


type Msg
    = NavbarMsg Navbar.State


type alias Details msg =
    { title : String
    , body : List (Html msg)
    }


view : (a -> msg) -> Details a -> Navbar.State -> Browser.Document msg
view toMsg details navbar =
    { title =
        details.title
    , body =
        -- [ viewHeader navbar
        -- , Html.map toMsg <| div [] details.body
        -- , viewFooter
        -- ]
        Debug.todo "Fix this"
    }


viewHeader : Navbar.State -> Html Msg
viewHeader navbarState =
    Navbar.config NavbarMsg
        |> Navbar.withAnimation
        |> Navbar.dark
        |> Navbar.brand [ href "/" ] [ text "SIwZ Trains" ]
        |> Navbar.items
            [ Navbar.itemLink [ href "/search", Utils.dynamicActive Routes.SearchRoute ] [ text "Wyszukaj połączenie" ]
            , Navbar.itemLink [ href "/about", Utils.dynamicActive Routes.AboutRoute ] [ text "O nas" ]
            , Navbar.dropdown
                { id = "admin-dropdown"
                , toggle = Navbar.dropdownToggle [] [ text "Panel admina" ]
                , items =
                    [ Navbar.dropdownItem [ href "/admin/users" ] [ text "Użytkownicy" ]
                    , Navbar.dropdownItem [ href "/admin/train" ] [ text "Pociągi" ]
                    , Navbar.dropdownItem [ href "/admin/stops", Utils.dynamicActive Routes.AdminTrainStopsRoute ] [ text "Przystanki" ]
                    , Navbar.dropdownItem [ href "/admin/routes" ] [ text "Trasy" ]
                    , Navbar.dropdownItem [ href "/admin/tickets" ] [ text "Bilety" ]
                    , Navbar.dropdownItem [ href "/admin/discounts" ] [ text "Zniżki" ]
                    ]
                }
            ]
        |> Navbar.view navbarState


viewFooter : Html msg
viewFooter =
    footer [] [ text "© 2137 lololololol" ]
