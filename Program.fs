module BusTracker.Program

open Avalonia
open Avalonia.Controls
open Avalonia.Logging.Serilog
open System

[<EntryPoint>]
let main argv =
    AppBuilder.Configure<BusTracker.App>()
      .UsePlatformDetect()
      .LogToDebug()
      .Start<BusTracker.Views.TrackerWindow>()
    0 // return an integer exit code
