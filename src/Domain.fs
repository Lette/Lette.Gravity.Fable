namespace Lette.Gravity.Fable

[<AutoOpen>]
module Domain =

    open System

    type Vector = { dx : float; dy : float } with
        static member (+) (v1, v2) = { dx = v1.dx + v2.dx; dy = v1.dy + v2.dy }
        static member Zero         = { dx = 0.; dy = 0. }

    type Point = { x : float; y : float } with
        static member (+) (p, v) = { x = p.x + v.dx; y = p.y + v.dy }

    let vectorBetween p1 p2 = { dx = p2.x - p1.x; dy = p2.y - p1.y }

    type Body = { Position : Point; Mass : float; Velocity : Vector }

    let radius { Mass = mass } =
        Math.Pow (mass, 1./2.) * 4.

    let initialBodies = [
        { Position = { x = 400.; y = 225. }; Mass = 50.0; Velocity = { dx = -0.2; dy = 0.1 } }
        { Position = { x = 380.; y = 225. }; Mass = 10.0; Velocity = { dx = 0.4; dy = 0.1 } }
        { Position = { x = 360.; y = 225. }; Mass = 8.0; Velocity = { dx = 0.2; dy = -0.5 } }
        { Position = { x = 215.; y = 225. }; Mass = 10.0; Velocity = { dx = 0.15; dy = 0.1 } }
        { Position = { x = 100.; y = 100. }; Mass = 0.1; Velocity = Vector.Zero }
        { Position = { x = 300.; y = 100. }; Mass = 0.5; Velocity = { dx = 0.2; dy = -1.01 } }
        { Position = { x = 5.; y = 15. }; Mass = 6.0; Velocity = { dx = -1.2; dy = 0.1 } }
    ]
