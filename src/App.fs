namespace Lette.Gravity.Fable

module App =

    let rec updateBody (position : Domain.Point) (velocity : Domain.Vector) (body : Domain.Body) =
        let min = body.Radius
        let maxX = Settings.Width - body.Radius
        let maxY = Settings.Heigth - body.Radius

        match (position.x, position.y) with
        | a, _ when a < min  -> updateBody { position with x = min }  { velocity with dx = 0. } body
        | a, _ when a > maxX -> updateBody { position with x = maxX } { velocity with dx = 0. } body
        | _, b when b < min  -> updateBody { position with y = min }  { velocity with dy = 0. } body
        | _, b when b > maxY -> updateBody { position with y = maxY } { velocity with dy = 0. } body
        | _                  -> { body with Position = position; Velocity = velocity }

    let moveBody (body : Domain.Body) (otherBodies : Domain.Body list) =

        let forceTowards = Physics.forceBetween body
        let directionTo = Physics.directionBetween body.Position

        let createForceVector otherBody =
            ((forceTowards otherBody) / body.Mass, (directionTo otherBody.Position)) |> Physics.toVector

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
