namespace WebSharper.Leaflet.Tests

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation

[<JavaScript>]
module Client =

    [<SPAEntryPoint>]
    let Main () =
        let coordinates = Var.Create ""
        div [] [
            div [
                attr.style "height: 600px;"
                on.afterRender (fun div ->
                    let map = Leaflet.Map(div)
                    map.SetView((47.49883, 19.0582), 14)
                    map.AddLayer(
                        Leaflet.TileLayer(
                            Leaflet.TileLayer.OpenStreetMap.UrlTemplate,
                            Leaflet.TileLayer.Options(
                                Attribution = Leaflet.TileLayer.OpenStreetMap.Attribution)))
                    map.AddLayer(
                        let m = Leaflet.Marker((47.48553, 19.07153))
                        m.BindPopup("IntelliFactory")
                        m)
                    map.On_mousemove(fun map ev ->
                        coordinates := "Position: " + ev.Latlng.ToString())
                    map.On_mouseout(fun map ev ->
                        coordinates := "")
                )
            ] []
            div [] [text coordinates.V]
        ]
        |> Doc.RunById "main"
