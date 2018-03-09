namespace Lette.Gravity.Fable

[<AutoOpen>]
module Physics =

    open System

    // F = ma  <=>  a = F/m
    // F = Gm1m2/(r^2)
    // a = v'(t)  <=>  a = dv/dt  <=>  limit of (delta-V / delta-T) as delta-T approaches 0
    //
    // For the sake of the simulation, ignore delta-T, and simplify  ==>
    //    delta-V = a = Gm1m2 / (r^2) / m

    let distanceSquaredBetween position1 position2 =
        let delta = vectorBetween position1 position2
        Math.Max (delta.dx * delta.dx + delta.dy * delta.dy, MinimumProximitySquared)

    let forceBetween body1 body2 =
        G * body1.Mass * body2.Mass / (distanceSquaredBetween body1.Position body2.Position)

    let fieldStrength position body =
        G * body.Mass / (distanceSquaredBetween position body.Position)

    let directionBetween position1 position2 =
        let delta = vectorBetween position1 position2
        Math.Atan2 (delta.dy, delta.dx)

    let toVector (r, theta) =
        { dx = r * (cos theta); dy = r * (sin theta) }
