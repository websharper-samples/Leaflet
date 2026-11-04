namespace Samples

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Templating
open WebSharper.UI.Client
open WebSharper.Leaflet

/// An interactive OpenStreetMap map, rendered with WebSharper.Leaflet.
[<JavaScript>]
module Client =

    type Template = Templating.Template<"wwwroot/index.html", ClientLoad.FromDocument>

    /// The map is centered on Budapest; the marker points at IntelliFactory.
    let private center = 47.49883, 19.0582
    let private location = 47.48553, 19.07153

    let private tileUrl = "https://tile.openstreetmap.org/{z}/{x}/{y}.png"
    let private attribution = "&copy; OpenStreetMap contributors"

    let private initMap (element: Dom.Element) =
        let map = Map(element)
        map.SetView(center, 14)
        map.AddLayer(TileLayer(tileUrl, TileLayer.Options(Attribution = attribution)))
        map.AddLayer(
            let marker = Marker(location)
            marker.BindPopup("IntelliFactory")
            marker)
        map

    [<SPAEntryPoint>]
    let Main () =
        let coordinates = Var.Create "—"
        Template()
            .Map(
                div [
                    attr.``class`` "map-canvas"
                    on.afterRender (fun element ->
                        let map = initMap element
                        map.On_mousemove(fun _ ev ->
                            coordinates.Value <- sprintf "%.5f, %.5f" ev.Latlng.Lat ev.Latlng.Lng)
                        map.On_mouseout(fun _ _ ->
                            coordinates.Value <- "—"))
                ] []
            )
            .Coordinates(coordinates.View)
            .Bind()
