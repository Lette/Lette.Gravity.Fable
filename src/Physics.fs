namespace Lette.Gravity.Fable

[<RequireQualifiedAccess>]
module Physics =

    open System

    // F = ma  <=>  a = F/m
    // F = Gm1m2/(r^2)
    // a = v'(t)  <=>  a = dv/dt  <=>  limit of (delta-V / delta-T) as delta-T approaches 0
    //
    // For the sake of the simulation, ignore delta-T, and simplify  ==>
    //    delta-V = a = Gm1m2 / (r^2) / m

    let distanceSquaredBetween (position1 : Domain.Point) (position2 : Domain.Point) =
        let delta = position1 - position2
        Math.Max (delta.dx * delta.dx + delta.dy * delta.dy, Settings.MinimumProximitySquared)

    let forceBetween (body1 : Domain.Body) (body2 : Domain.Body) =
        Settings.G * body1.Mass * body2.Mass / (distanceSquaredBetween body1.Position body2.Position)

    let fieldStrength (position : Domain.Point) (body : Domain.Body) =
        Settings.G * body.Mass / (distanceSquaredBetween position body.Position)

    let directionBetween (position1 : Domain.Point) (position2 : Domain.Point) =
        let delta = position2 - position1
        Math.Atan2 (delta.dy, delta.dx)

    let toVector (r, theta) : Domain.Vector =
        { dx = r * (cos theta); dy = r * (sin theta) }
