module BusTracker.Data.ServiceColours

open FSharp.Data

type File = JsonProvider<"service_colours.json">

type Colour = {
    Foreground: string;
    Background: string;
    }

let default_colour = { Foreground = "White"; Background = "Pink" }

let get_colours () =
    let s = File.GetSamples()
    s
    |> Seq.map (fun v ->
      v.ServiceName.String.Value, { Foreground = v.TextColor; Background = v.Color })
    |> Map.ofSeq
