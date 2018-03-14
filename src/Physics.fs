namespace Lette.Gravity.Fable

[<AutoOpen>]
module Physics =

    // F = ma  <=>  a = F/m
    // F = Gm1m2/(r^2)
    // a = v'(t)  <=>  a = dv/dt  <=>  limit of (delta-V / delta-T) as delta-T approaches 0
    //
    // For the sake of the simulation, ignore delta-T, and simplify  ==>
    //    delta-V = a = Gm1m2 / (r^2) / m

    let forceBetween body1 body2 =
        let v = vectorBetween body1.Position body2.Position
        let d = max v.Length MinimumProximity

        // Vector form of the gravity force equation:
        //     G * m1 * m2 / (d^2) * (v / d)
        // where
        //     d = |v| is the distance between the objects,
        //     so effectively the magnitude of the force multiplied by a unit vector pointing towards the other object

        G * body1.Mass * body2.Mass / (d ** 3.) .* v
