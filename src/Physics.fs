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

        G * body1.Mass * body2.Mass / (d ** 3.) |> scaleVector v

    let momentum body =
        // Momentum is equal to mass times velocity. Momentum and velocity are vectors, mass is a scalar.
        // p = mv
        body.Mass |> scaleVector body.Velocity

    let momentumSum body1 body2 =
        // The sum of the momentums of two objects that collide are preserved.
        // p_total = p_1 + p_2
        momentum body1 + momentum body2

    let combinedVelocity body1 body2 =
        // No mass is lost in a collision.
        // m_total = m_1 + m_2
        // p = mv <=> v = p/m <=> v_total = p_total / m_total <=> (1 / m_total) * p_total

        let mTotal = body1.Mass + body2.Mass
        let pTotal = momentumSum body1 body2
        
        (1. / mTotal) |> scaleVector pTotal

    let barycenter body1 body2 =
        // The barycenter (center of mass) "p" of two bodies is a function of the masses and positions
        // of the two bodies, as follows:
        //
        //    p = 1 / (m_1 + m_2) * (m_1 * p_1 + m_2 * p_2)
        //
        // To simplify, translate the entire system by -p_1 before calculating, which gives:
        //
        //    p' = m_2 / (m_1 + m_2) * (p_2 - p_1)
        //
        // Then the final result is given by translating back:
        //
        //    p = p_1 + p'

        let p' = body2.Mass / (body1.Mass + body2.Mass) |> scaleVector (vectorBetween body1.Position body2.Position)
        body1.Position + p'
