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
        
        let acceleration =
            // F = ma   <=>   a = F/m   <=>   a = (1/m) * F
            // where F and a are vectors and m is a scalar.
            (1. / body.Mass) |> scaleVector totalForce

        let newVelocity = scaleVector body.Velocity VelocityFactor + acceleration
        let newPosition = body.Position + newVelocity

        updateBody newPosition newVelocity body

    let mergeBodies bodies =
        // detect and merge colliding bodies, effectively reducing the number of bodies

        let isColliding a b = (vectorBetween a.Position b.Position).Length < (radius a + radius b)

        // TODO: Find out how to (recursively?) go through the list of bodies and find all collisions.
        // TODO: Only do collision detection and merging if Settings.MergeCollidingObjects is true.

        match bodies with
        | (b1::b2::rest) when isColliding b1 b2 -> { Position = barycenter b1 b2; Mass = b1.Mass + b2.Mass; Velocity = combinedVelocity b1 b2 } :: rest
        | _ -> bodies

    let moveBodies bodies =
        bodies
            |> List.map (fun body -> moveBody body (List.except [body] bodies))
            |> mergeBodies

    let rec runSimulation bodies _ =
        let newBodies = moveBodies bodies
        render newBodies

        FrameRequestCallback (runSimulation newBodies)
            |> window.requestAnimationFrame
            |> ignore

    runSimulation initialBodies 0.
