namespace Lette.Gravity.Fable

[<RequireQualifiedAccess>]
module Domain =

    open System

    type Position = { x : float; y : float }
    type Velocity = { dx : float; dy : float }
    type Body = {
        Position : Position;
        Mass : float;
        Velocity : Velocity }
        with member this.Radius with get() = Math.Pow (this.Mass, 1./2.) * 4.

    let ZeroVelocity = { dx = 0.; dy = 0. }

    let bodies = [ 
        { Position = { x = 400.; y = 225. }; Mass = 50.0; Velocity = { dx = -0.2; dy = 0.1 } }
        { Position = { x = 380.; y = 225. }; Mass = 10.0; Velocity = { dx = 0.4; dy = 0.1 } }
        { Position = { x = 360.; y = 225. }; Mass = 8.0; Velocity = { dx = 0.2; dy = -0.5 } }
        { Position = { x = 215.; y = 225. }; Mass = 10.0; Velocity = { dx = 0.15; dy = 0.1 } }
        { Position = { x = 100.; y = 100. }; Mass = 0.1; Velocity = ZeroVelocity }
        { Position = { x = 300.; y = 100. }; Mass = 0.5; Velocity = { dx = 0.2; dy = -1.01 } }
        { Position = { x = 5.; y = 15. }; Mass = 6.0; Velocity = { dx = -1.2; dy = 0.1 } }
    ]
