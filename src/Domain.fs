namespace Lette.Gravity.Fable

[<AutoOpen>]
module Domain =

    type Vector = { dx : float; dy : float; dz : float } with
        static member (+) (v1, v2) = { dx = v1.dx + v2.dx; dy = v1.dy + v2.dy; dz = v1.dz + v2.dz }
        static member Zero         = { dx = 0.;            dy = 0.;            dz = 0. }
        static member (.*) (k, v)  = { dx = k * v.dx;      dy = k * v.dy;      dz = k * v.dz }
        member this.Length         = sqrt (this.dx ** 2. + this.dy ** 2. + this.dz ** 2.)

    type Point = { x : float; y : float; z : float } with
        static member (+) (p, v) = { x = p.x + v.dx; y = p.y + v.dy; z = p.z + v.dz }

    let vectorBetween p1 p2 = { dx = p2.x - p1.x; dy = p2.y - p1.y; dz = p2.z - p1.z }

    type Body = { Position : Point; Mass : float; Velocity : Vector }

    let radius { Mass = mass } =
        sqrt mass * 4.

    let initialBodies = [
        { Position = { x = 400.; y = 225.; z = 100. }; Mass = 50.0; Velocity = { dx = -0.2; dy = 0.1; dz = 0. } }
        // { Position = { x = 200.; y = 225.; z = 200. }; Mass = 50.0; Velocity = { dx = 0.2; dy = 0.1; dz = 0. } }
        // { Position = { x = 300.; y = 325.; z = 300. }; Mass = 50.0; Velocity = { dx = 0.2; dy = -0.1; dz = 0.1 } }
        { Position = { x = 380.; y = 225.; z = 20. }; Mass = 10.0; Velocity = { dx = 0.4; dy = 0.1; dz = 0. } }
        { Position = { x = 360.; y = 225.; z = 220. }; Mass = 8.0; Velocity = { dx = 0.2; dy = -0.5; dz = 0. } }
        { Position = { x = 215.; y = 225.; z = 30. }; Mass = 10.0; Velocity = { dx = 0.15; dy = 0.1; dz = 0. } }
        { Position = { x = 100.; y = 100.; z = 330. }; Mass = 0.1; Velocity = Vector.Zero }
        { Position = { x = 300.; y = 100.; z = 40. }; Mass = 0.5; Velocity = { dx = 0.2; dy = -1.01; dz = 0. } }
        { Position = { x = 5.; y = 15.; z = 0. }; Mass = 6.0; Velocity = { dx = -1.2; dy = 0.1; dz = 0. } }
    ]
