namespace Lette.Gravity.Fable

module App =

    open System

    let adjustForBounds bound x =
        if x < 0. then
            bound - x
        else if x > bound then
            x - bound
        else
            x

    let rec updateBody x y dx dy (body : Domain.Body) =
        let min = body.Radius
        let maxX = Domain.width - body.Radius
        let maxY = Domain.height - body.Radius

        match x, y with
        | a, _ when a < min  -> updateBody min y 0. dy body
        | a, _ when a > maxX -> updateBody maxX y 0. dy body
        | _, b when b < min  -> updateBody x min dx 0. body
        | _, b when b > maxY -> updateBody x maxY dx 0. body
        | a, b               -> { body with Position = { x = a; y = b }; Velocity = { dx = dx; dy = dy } }

    let moveBody (body : Domain.Body) (otherBodies : Domain.Body list) =

        // F = ma  =>  a = F/m,  F = Gm1m2/(r^2), a = v'(t)  =>  a = dv/dt ~~ delta-V / delta-T,
        // assuming delta-T is 1 (can I?) ==> deltaV = Gm1m2/(r^2)/m

        let G = 0.2  // TODO: This must surely be adjusted!

        let distanceSquaredTo (otherBody : Domain.Body) =
            let dx = body.Position.x - otherBody.Position.x
            let dy = body.Position.y - otherBody.Position.y
            Math.Max (dx * dx + dy * dy, 20.0)

        let forceTowards (otherBody : Domain.Body) =
            G * body.Mass * otherBody.Mass / (distanceSquaredTo otherBody)

        let directionTo (otherBody : Domain.Body) =
            let dx = body.Position.x - otherBody.Position.x
            let dy = body.Position.y - otherBody.Position.y
            Math.Atan2 (dy, dx) + Math.PI

        let toVector (r, theta) =
            (r * (cos theta), r * (sin theta))

        let (newDeltaVX, newDeltaVY) =
             otherBodies
                |> List.map ((fun otherBody -> (forceTowards otherBody) / body.Mass, (directionTo otherBody)) >> toVector)
                |> List.fold (fun (sumdx, sumdy) (dx, dy) -> (sumdx + dx, sumdy + dy)) (0., 0.)

        let newVx = body.Velocity.dx + newDeltaVX
        let newVy = body.Velocity.dy + newDeltaVY
        let newX = body.Position.x + newVx
        let newY = body.Position.y + newVy

        updateBody newX newY newVx newVy body

    let moveBodies bodies =
        bodies |> List.map (fun body -> moveBody body (List.except [body] bodies))

    let rec runSimulation ctx bodies =
        async {
            let newBodies = moveBodies bodies

            Rendering.resetCanvas ctx
            Rendering.drawBodies ctx newBodies

            do! Async.Sleep (1)
            return! runSimulation ctx newBodies
        }

    let init () =
        async {
            let ctx = Rendering.init ()
            Rendering.resetCanvas ctx
            return! runSimulation ctx Domain.bodies
        }

    init () |> Async.StartImmediate
