namespace Lette.Gravity.Fable

module App =

    let rec updateBody x y dx dy (body : Domain.Body) =
        let min = body.Radius
        let maxX = Settings.Width - body.Radius
        let maxY = Settings.Heigth - body.Radius

        match x, y with
        | a, _ when a < min  -> updateBody min y 0. dy body
        | a, _ when a > maxX -> updateBody maxX y 0. dy body
        | _, b when b < min  -> updateBody x min dx 0. body
        | _, b when b > maxY -> updateBody x maxY dx 0. body
        | a, b               -> { body with Position = { x = a; y = b }; Velocity = { dx = dx; dy = dy } }

    let moveBody (body : Domain.Body) (otherBodies : Domain.Body list) =

        let forceTowards = Physics.forceBetween body
        let directionTo = Physics.directionBetween body.Position

        let (newDeltaVX, newDeltaVY) =
             otherBodies
                |> List.map ((fun otherBody -> (forceTowards otherBody) / body.Mass, (directionTo otherBody.Position)) >> Physics.toVector)
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
