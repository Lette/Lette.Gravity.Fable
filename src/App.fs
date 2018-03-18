namespace Lette.Gravity.Fable

module App =

    open Fable.Import.Browser

    let rec updateBody position velocity body =
        let r = radius body
        let maxX = Width - r
        let maxY = Height - r
        let maxZ = Depth - r

        match position.x, position.y, position.z with
        | a, _, _ when a < r    -> updateBody { position with x = r    } { velocity with dx = -velocity.dx * Bounciness } body
        | a, _, _ when a > maxX -> updateBody { position with x = maxX } { velocity with dx = -velocity.dx * Bounciness } body
        | _, b, _ when b < r    -> updateBody { position with y = r    } { velocity with dy = -velocity.dy * Bounciness } body
        | _, b, _ when b > maxY -> updateBody { position with y = maxY } { velocity with dy = -velocity.dy * Bounciness } body
        | _, _, c when c < r    -> updateBody { position with z = r    } { velocity with dz = -velocity.dz * Bounciness } body
        | _, _, c when c > maxZ -> updateBody { position with z = maxZ } { velocity with dz = -velocity.dz * Bounciness } body
        | _                     -> { body with Position = position; Velocity = velocity }

    let moveBody body otherBodies =

        let forceTowards = forceBetween body

        let totalForce = otherBodies |> List.sumBy forceTowards
        
        // F = ma <=> a = F/m or a = (1/m) * F. F is a vector and .* is the scalar multiplication operator.
        let acceleration = (1. / body.Mass) |> scaleVector totalForce

        let newVelocity = scaleVector body.Velocity VelocityFactor + acceleration
        let newPosition = body.Position + newVelocity

        updateBody newPosition newVelocity body

    let moveBodies bodies =
        bodies |> List.map (fun body -> moveBody body (List.except [body] bodies))

    let rec runSimulation bodies _ =
        let newBodies = moveBodies bodies
        render newBodies

        FrameRequestCallback (runSimulation newBodies)
            |> window.requestAnimationFrame
            |> ignore

    runSimulation initialBodies 0.
