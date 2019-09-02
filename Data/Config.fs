module BusTracker.Data.Config

open FSharp.Data

type ConfigProvider = JsonProvider<"config.json">

let stops (config:ConfigProvider.Root) =
    config.Destinations
    |> Seq.map (fun dest -> dest.Stops)
    |> Seq.concat
    |> Seq.distinctBy (fun v -> v.Id)
    |> Seq.toList

