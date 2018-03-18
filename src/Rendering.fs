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

    let private hw = Width / 2.
    let private hh = Height / 2.

    let private ctx = canvas.getContext_2d ()
    ctx.translate (hw, hh)

    let private transformFactor depth =
        ViewingDistance / (ViewingDistance + depth)

    let private transformToScreen point =
        let t = transformFactor point.z
        { x = point.x * t; y = point.y * t; z = 0. }

    let private p1 = { x = -hw + 0.5; y = -hh + 0.5; z = 0. }
    let private p1' = transformToScreen { p1 with z = Depth }
    let private p2 = { x = hw - 0.5; y = hh - 0.5; z = 0. }
    let private p2' = transformToScreen { p2 with z = Depth }

    let private ellipse x y rx ry startAngle endAngle =
        ctx.save ()
        ctx.beginPath ()
        ctx.translate (x - rx, y - ry)
        ctx.scale (rx, ry)
        ctx.arc (1., 1., 1., startAngle, endAngle, false)
        ctx.restore ()
        ctx.stroke ()

    let private drawGreatCircle x y rx ry startAngle middleAngle endAngle =
        ctx.save ()

        ctx.fillStyle <- transparent

        ctx.strokeStyle <- markerColorB
        ctx.setLineDash (dashStyle)
        ellipse x y rx ry middleAngle endAngle

        ctx.strokeStyle <- markerColorF
        ctx.setLineDash (noDash)
        ellipse x y rx ry startAngle middleAngle

        ctx.restore ()

    let private drawBody body =

        let drawEquator x y rx ry =
            drawGreatCircle x y rx ry 0. Math.PI (2. * Math.PI)

        let drawMeridian x y rx ry =
            drawGreatCircle x y rx ry (-Math.PI / 2.) (Math.PI / 2.) (3. * Math.PI / 2.)

        let createPlanetColor z : U3<string, Browser.CanvasGradient, Browser.CanvasPattern> =
            !^ (sprintf "rgba(255,100,0,%f" (0.1 + 0.3 * (Depth - z) / Depth))

        let drawFace x y z r =
            ctx.beginPath ()
            ctx.fillStyle <- (createPlanetColor z)
            ctx.arc (x, y, r, 0., 2. * Math.PI, false)
            ctx.fill ()

        let viewingAngle offset depth =
            offset / (ViewingDistance + depth) |> atan

        let drawLines x y z r =
            let ry = r * sin (viewingAngle y z)
            drawEquator x y r ry

            let rx = r * sin (viewingAngle x z)
            drawMeridian x y rx r

        let p = { body.Position with x = body.Position.x - hw; y = body.Position.y - hh }
        let c = transformToScreen p

        let r = radius body
        let pr1 = transformToScreen { body.Position with x = body.Position.x - r / 2. }
        let pr2 = transformToScreen { body.Position with x = body.Position.x + r / 2. }
        let transformedRadius = pr2.x - pr1.x

        drawFace c.x c.y body.Position.z transformedRadius
        drawLines c.x c.y body.Position.z (transformedRadius - 1.)

    let private drawBodies bodies =
        bodies |> List.iter drawBody

    let private clearCanvas () =
        ctx.save ()
        ctx.setTransform (1., 0., 0., 1., 0., 0.)
        ctx.fillStyle <- black
        ctx.fillRect (0., 0., canvas.width, canvas.height)
        ctx.restore ()

    let private drawBoundingBox () =
        ctx.save ()
        ctx.strokeStyle <- white

        ctx.rect (p1.x, p1.y, p2.x - p1.x, p2.y - p1.y)
        ctx.rect (p1'.x, p1'.y, p2'.x - p1'.x, p2'.y - p1'.y)
        ctx.moveTo (p1.x, p1.y)
        ctx.lineTo (p1'.x, p1'.y)
        ctx.moveTo (p1.x, p2.y)
        ctx.lineTo (p1'.x, p2'.y)
        ctx.moveTo (p2.x, p1.y)
        ctx.lineTo (p2'.x, p1'.y)
        ctx.moveTo (p2.x, p2.y)
        ctx.lineTo (p2'.x, p2'.y)

        ctx.stroke ()
        ctx.restore ()

    let render bodies =
        clearCanvas ()
        drawBoundingBox ()
        drawBodies bodies
