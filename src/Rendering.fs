namespace Lette.Gravity.Fable

[<AutoOpen>]
module Rendering =

    open Fable.Core
    open Fable.Core.JsInterop
    open Fable.Import
    open System

    let private black : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> = !^"rgb(0,0,0)"
    let private white : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> = !^"rgb(255,255,255)"
    let private planetColor : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> = !^"rgba(255,100,0,0.4)"
    let private markerColorF : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> = !^"rgba(255,100,0,1.0)"
    let private markerColorB : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> = !^"rgba(0,0,255,0.55)"
    let private transparent : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> = !^"transparent"

    let private dashStyle = ResizeArray<_> [|2.; 3.|]
    let private noDash = ResizeArray<float> [||]

    let private canvas = Browser.document.getElementsByTagName_canvas().[0]
    canvas.width <- Width
    canvas.height <- Height
    let private ctx = canvas.getContext_2d ()

    let private ellipse x y rx ry startAngle endAngle =
        ctx.save ()
        ctx.beginPath ()
        ctx.translate (x - rx, y - ry)
        ctx.scale (rx, ry)
        ctx.arc (1., 1., 1., startAngle, endAngle, false)
        ctx.restore ()
        ctx.stroke ()

    let private drawBody body =

        let drawGreatCircle x y rx ry startAngle middleAngle endAngle =
            ctx.save ()

            ctx.fillStyle <- transparent

            ctx.strokeStyle <- markerColorB
            ctx.setLineDash (dashStyle)
            ellipse x y rx ry middleAngle endAngle

            ctx.strokeStyle <- markerColorF
            ctx.setLineDash (noDash)
            ellipse x y rx ry startAngle middleAngle

            ctx.restore ()

        let drawEquator x y rx ry =
            drawGreatCircle x y rx ry 0. Math.PI (2. * Math.PI)

        let drawMeridian x y rx ry =
            drawGreatCircle x y rx ry (-Math.PI / 2.) (Math.PI / 2.) (3. * Math.PI / 2.)

        let drawBody x y r =
            ctx.beginPath ()
            ctx.fillStyle <- planetColor
            ctx.arc (x, y, r, 0., 2. * Math.PI, false)
            ctx.fill ()

        let viewingAngle d dMax =
            let h = dMax / 2.
            let maxViewingAngle = Math.PI / 9.
            maxViewingAngle * (d - h) / h

        let drawLines x y r =
            let verticalViewingAngle = viewingAngle y Height
            drawEquator x y r (r * verticalViewingAngle)

            let horizontalViewingAngle = viewingAngle x Width
            drawMeridian x y (r * horizontalViewingAngle) r

        let draw x y =
            let r = radius body
            drawBody x y r
            drawLines x y (r - 1.)

        let drawWithDeltas dx dy =
            draw (body.Position.x + dx) (body.Position.y + dy)

        //let isLeftOfEdge = body.Position.x - radius < 0.
        //let isRightOfEdge = body.Position.x + radius > Width
        //let isAboveEdge = body.Position.y - radius < 0.
        //let isBelowEdge = body.Position.y + radius > Height

        let dxs = seq {
                yield 0.
                //if isLeftOfEdge then yield Width
                //if isRightOfEdge then yield -Width
            }

        let dys = seq {
                yield 0.
                //if isAboveEdge then yield Height
                //if isBelowEdge then yield -Height
            }

        // TODO: Use this when "Seq.allPairs" are supported by fable
        // Seq.allPairs dxs dys |> Seq.iter (fun p -> p ||> drawWithDeltas)

        dxs |> Seq.iter (fun dx -> dys |> Seq.iter (fun dy -> drawWithDeltas dx dy))

    let drawBodies bodies =
        bodies |> List.iter drawBody

    let resetCanvas () =
        ctx.beginPath ()
        ctx.fillStyle <- black
        ctx.clearRect (0., 0., Width, Height)
        ctx.strokeStyle <- white
        ctx.rect (0.5, 0.5, Width - 1., Height - 1.)
        ctx.stroke ()
