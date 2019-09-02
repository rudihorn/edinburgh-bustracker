module BusTracker.Data.BusApi

open FSharp.Data

let url = "https://tfeapp.com/api/website"


type StopTimes = JsonProvider<"https://tfeapp.com/api/website/stop_times.php?stop_id=6200208460">

type CommonStops =
    | DukeStreetNW = 6200242000UL
    | DukeStreetSE = 6200207590UL

let stop_id_of_common_stop stop =
    uint64 stop

let stop_times_url(stop:uint64) =
    sprintf "%s/stop_times.php?stop_id=%u" url stop

let stop_times(stop) =
    StopTimes.AsyncLoad(stop_times_url stop)

let stop_times_common_stop(cstop) =
    stop_id_of_common_stop cstop |> stop_times

let get_service (data : StopTimes.Root) (service : string) =
    data.Services
    |> Seq.tryFind (fun s -> s.ServiceName.String.Value = service)

let service_get_service_minutes (service : StopTimes.Servicis) =
    service.Departures
    |> Seq.map (fun dep -> dep.Minutes)
    |> Seq.sort
    |> Seq.cache

let get_service_minutes data service =
    let service = get_service data service
    service |> Option.map service_get_service_minutes |> Option.defaultValue Seq.empty
