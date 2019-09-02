namespace BusTracker

open System
open Avalonia.Controls
open Avalonia.Controls.Templates
open BusTracker.ViewModels

type ViewLocator() =
    interface IDataTemplate with
        member val SupportsRecycling = false

        member this.Build(data) =
            let name = data.GetType().FullName.Replace("ViewModel", "View")
            let typ = Type.GetType(name)

            if isNull typ
            then (TextBlock(Text = "Not found:" + name) :> IControl)
            else (Activator.CreateInstance(typ) :?> IControl)

        member this.Match(data:obj) =
            data :? ViewModelBase
