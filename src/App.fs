namespace Lette.Gravity.Fable

module App =

    let rec updateBody position velocity (body : Body) =
        let min = body.Radius
        let maxX = Width - body.Radius
        let maxY = Heigth - body.Radius

        match (position.x, position.y) with
        | a, _ when a < min  -> updateBody { position with x = min }  { velocity with dx = 0. } body
        | a, _ when a > maxX -> updateBody { position with x = maxX } { velocity with dx = 0. } body
        | _, b when b < min  -> updateBody { position with y = min }  { velocity with dy = 0. } body
        | _, b when b > maxY -> updateBody { position with y = maxY } { velocity with dy = 0. } body
        | _                  -> { body with Position = position; Velocity = velocity }

    let moveBody body otherBodies =

        let forceTowards = forceBetween body
        let directionTo = directionBetween body.Position

        let createForceVector otherBody =
            ((forceTowards otherBody) / body.Mass, (directionTo otherBody.Position)) |> toVector

        let newDeltaV =
             otherBodies |> List.sumBy createForceVector

        let newVelocity = body.Velocity + newDeltaV
        let newPosition = body.Position + newVelocity

        updateBody newPosition newVelocity body

    let moveBodies bodies =
        bodies |> List.map (fun body -> moveBody body (List.except [body] bodies))

    let rec runSimulation ctx bodies =
        async {
            let newBodies = moveBodies bodies

            resetCanvas ctx
            drawBodies ctx newBodies

            do! Async.Sleep (1)
            return! runSimulation ctx newBodies
        }

    let init () =
        async {
            let canvas = initializeCanvas ()
            let ctx = getDrawingContext canvas
            resetCanvas ctx
            return! runSimulation ctx initialBodies
        }

    init () |> Async.StartImmediate
