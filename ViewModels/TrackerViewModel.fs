namespace BusTracker.ViewModels

open System.IO
open BusTracker.Data
open Avalonia.Media

type TrackerViewModelService() =
    inherit ViewModelBase()

    member val Name = 0 with get, set
    member val Next = "-" with get, set
    member val Further = "" with get, set
    member val Colours = ServiceColours.default_colour with get, set
    member self.Background = SolidColorBrush(Avalonia.Media.Color.Parse self.Colours.Background)
    member self.Foreground = SolidColorBrush(Avalonia.Media.Color.Parse self.Colours.Foreground)

type TrackerViewModelStop() =
    inherit ViewModelBase()

    member val Name = "" with get, set
    member val Direction = "" with get, set
    member val WalkingDistance = 0 with get, set
    member val Services : TrackerViewModelService seq = Seq.empty with get, set

type TrackerViewModelDestination() =
    inherit ViewModelBase()

    member val Name = "" with get, set
    member val Stops : TrackerViewModelStop seq = Seq.empty with get, set

type TrackerViewModel() =
    inherit ViewModelBase()

    member val Destinations : TrackerViewModelDestination seq = Seq.empty with get, set

    static member FromConfigAsync config colours =
        let stopIds =
            BusTracker.Data.Config.stops config
            |> Seq.map (fun x -> uint64 x.Id)
        async {
            let stopTasks =
              stopIds
              |> Seq.map BusApi.stop_times
              |> Array.ofSeq
            let! stopsData = Async.Parallel stopTasks
            let stopsData = Map.ofSeq (Seq.zip stopIds stopsData)
            let destinations =
              config.Destinations
              |> Seq.map (fun dest ->
                 let name = dest.Name
                 let stops =
                  dest.Stops
                  |> Seq.map (fun stop ->
                      let stopData = stopsData.[uint64 stop.Id]
                      let name = stopData.Stop.Name
                      let direction = sprintf "(%s)" stopData.Stop.Direction
                      let services =
                        stop.Services
                        |> Seq.map (fun service ->
                          let minutes = BusApi.get_service_minutes stopData service
                          let next = Seq.tryHead minutes |> Option.map (sprintf "%d min") |> Option.defaultValue "-"
                          use tw = new StringWriter()
                          let further = if Seq.isEmpty minutes then [] else Seq.tail minutes |> Seq.toList
                          if List.isEmpty further |> not then
                            fprintf tw "and "
                            further |> Seq.iteri (fun i s ->
                                                  if i > 0 then fprintf tw ", "
                                                  fprintf tw "%d" s)
                            fprintf tw " min"
                          let colours = Map.tryFind (string service) colours |> Option.defaultValue ServiceColours.default_colour in
                          TrackerViewModelService(Name = service, Next = next, Further = tw.ToString(), Colours = colours)
                        )
                        |> Seq.cache
                      TrackerViewModelStop(Name = name, Direction = direction, Services = services, WalkingDistance = stop.Wd)
                  )
                  |> Seq.cache
                 TrackerViewModelDestination(Name = name, Stops = stops))
              |> Seq.cache
            return TrackerViewModel(Destinations = destinations)
        }

