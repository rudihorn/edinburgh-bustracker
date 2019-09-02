namespace BusTracker.Views

open Avalonia.Controls
open Avalonia.Interactivity
open Avalonia.Markup.Xaml
open BusTracker.Data
open BusTracker.ViewModels

type Context = {
    Destinations : string
}

type TrackerWindow() as this =
    inherit Window()

    let config = BusTracker.Data.Config.ConfigProvider.GetSample ()
    let colours = BusTracker.Data.ServiceColours.get_colours ()

    do this.InitializeComponent()

    member this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
        this.Update() |> Async.StartImmediate

    member this.Update() =
        async {
            //this.DataContext <- { Destinations = "loading" }
            while true do
                let! v = this.GetStops()
                printf "%A" v
                this.DataContext <- v
                do! Async.Sleep 10000
        }

    member this.GetStops() =        async {
            let! data = TrackerViewModel.FromConfigAsync config colours
            return data
        }
