[<RequireQualifiedAccess>]
module Lette.Gravity.Fable.Rendering

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import

let private black : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> = !^"rgb(0,0,0)"
let private white : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> = !^"rgb(255,255,255)"

let private drawBody (ctx : Browser.CanvasRenderingContext2D) (body : Domain.Body) =

    let radius = body.Mass * 5.

    let draw x y =
        ctx.beginPath ()
        ctx.fillStyle <- white
        ctx.arc (x, y, radius, 0., 2. * System.Math.PI, false)
        ctx.fill ()

    let drawWithDeltas dx dy =
        draw (body.Position.x + dx) (body.Position.y + dy)

    let isLeftOfEdge = body.Position.x - radius < 0.
    let isRightOfEdge = body.Position.x + radius > Domain.width
    let isAboveEdge = body.Position.y - radius < 0.
    let isBelowEdge = body.Position.y + radius > Domain.height

    let dxs = seq {
            yield 0.
            if isLeftOfEdge then yield Domain.width
            if isRightOfEdge then yield -Domain.width
        }

    let dys = seq {
            yield 0.
            if isAboveEdge then yield Domain.height
            if isBelowEdge then yield -Domain.height
        }

    // TODO: Use this when "Seq.allPairs" are supported by fable
    // Seq.allPairs dxs dys |> Seq.iter (fun p -> p ||> drawWithDeltas)

    dxs |> Seq.iter (fun dx -> dys |> Seq.iter (fun dy -> drawWithDeltas dx dy))

let drawBodies ctx bodies =
    bodies |> List.iter (drawBody ctx)

let resetCanvas (ctx : Browser.CanvasRenderingContext2D) =
    ctx.fillStyle <- black
    ctx.clearRect (0., 0., Domain.width, Domain.height)
    ctx.strokeStyle <- white
    ctx.rect (0.5, 0.5, Domain.width - 1., Domain.height - 1.)
    ctx.stroke ()

let init () =
    let canvas = Browser.document.getElementsByTagName_canvas().[0]
    canvas.width <- Domain.width
    canvas.height <- Domain.height
    canvas.getContext_2d()
