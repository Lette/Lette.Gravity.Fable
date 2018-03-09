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

    let distanceSquaredBetween (position1 : Domain.Position) (position2 : Domain.Position) =
        let deltaX = position1.x - position2.x
        let deltaY = position1.y - position2.y
        Math.Max (deltaX * deltaX + deltaY * deltaY, Settings.MinimumProximitySquared)

    let forceBetween (body1 : Domain.Body) (body2 : Domain.Body) =
        Settings.G * body1.Mass * body2.Mass / (distanceSquaredBetween body1.Position body2.Position)

    let fieldStrength (position : Domain.Position) (body : Domain.Body) =
        Settings.G * body.Mass / (distanceSquaredBetween position body.Position)

    let directionBetween (position1 : Domain.Position) (position2 : Domain.Position) =
        let deltaX = position2.x - position1.x
        let deltaY = position2.y - position1.y
        Math.Atan2 (deltaY, deltaX)

    let toVector (r, theta) =
        (r * (cos theta), r * (sin theta))
