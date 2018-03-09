namespace Lette.Gravity.Fable

[<RequireQualifiedAccess>]
module Rendering =

    open Fable.Core
    open Fable.Core.JsInterop
    open Fable.Import
    open System

    let private black : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> = !^"rgb(0,0,0)"
    let private white : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> = !^"rgb(255,255,255)"
    let private planetColor : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> = !^"rgba(255,100,100,0.4)"

    let private drawBody (ctx : Browser.CanvasRenderingContext2D) (body : Domain.Body) =

        let draw x y =
            ctx.beginPath ()
            ctx.fillStyle <- planetColor
            ctx.arc (x, y, body.Radius, 0., 2. * Math.PI, false)
            ctx.fill ()

        let drawWithDeltas dx dy =
            draw (body.Position.x + dx) (body.Position.y + dy)

        //let isLeftOfEdge = body.Position.x - radius < 0.
        //let isRightOfEdge = body.Position.x + radius > Domain.width
        //let isAboveEdge = body.Position.y - radius < 0.
        //let isBelowEdge = body.Position.y + radius > Domain.height

        let dxs = seq {
                yield 0.
                //if isLeftOfEdge then yield Domain.width
                //if isRightOfEdge then yield -Domain.width
            }

        let dys = seq {
                yield 0.
                //if isAboveEdge then yield Domain.height
                //if isBelowEdge then yield -Domain.height
            }

        // TODO: Use this when "Seq.allPairs" are supported by fable
        // Seq.allPairs dxs dys |> Seq.iter (fun p -> p ||> drawWithDeltas)

        dxs |> Seq.iter (fun dx -> dys |> Seq.iter (fun dy -> drawWithDeltas dx dy))

    let drawBodies ctx bodies =
        bodies |> List.iter (drawBody ctx)

    let resetCanvas (ctx : Browser.CanvasRenderingContext2D) =
        ctx.beginPath ()
        ctx.fillStyle <- black
        ctx.clearRect (0., 0., Settings.Width, Settings.Heigth)
        ctx.strokeStyle <- white
        ctx.rect (0.5, 0.5, Settings.Width - 1., Settings.Heigth - 1.)
        ctx.stroke ()

    let init () =
        let canvas = Browser.document.getElementsByTagName_canvas().[0]
        canvas.width <- Settings.Width
        canvas.height <- Settings.Heigth
        canvas.getContext_2d()
