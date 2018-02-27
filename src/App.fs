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

    let moveBody (body : Domain.Body) (otherBodies : Domain.Body list) =

        // F = ma  =>  a = F/m,  F = Gm1m2/(r^2), a = v'(t)  =>  a = dv/dt ~~ delta-V / delta-T,
        // assuming delta-V is 1 ==> deltaV = Gm1m2/(r^2)/m

        let G = 80.  // TODO: This must surely be adjusted!

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
                |> List.fold (fun (sumdx, sumdy) (dx, dy) -> (sumdx + dx, sumdy + dy)) (body.Velocity.dx, body.Velocity.dy)


        let newX = body.Position.x + newDeltaVX
        let newY = body.Position.y + newDeltaVY
        { body with Position = { x = adjustForBounds Domain.width newX; y = adjustForBounds Domain.height newY } }

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
            return! runSimulation ctx Domain.bodies
        }

    init () |> Async.StartImmediate
