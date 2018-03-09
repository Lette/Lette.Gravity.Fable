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
        // { Position = { x = 200.; y = 401. }; Mass = 2.0; Velocity = { dx = -1.; dy = -0.2 } }
        // { Position = { x = 300.; y = 103. }; Mass = 0.5; Velocity = { dx = 1.2; dy = 0.6 } }
        // { Position = { x = 216.; y = 235. }; Mass = 10.0; Velocity = { dx = 0.15; dy = 0.1 } }
        // { Position = { x = 101.; y = 110. }; Mass = 0.1; Velocity = ZeroVelocity }
        // { Position = { x = 301.; y = 110. }; Mass = 0.5; Velocity = { dx = 1.2; dy = -1.01 } }
        // { Position = { x = 6.; y = 25. }; Mass = 1.0; Velocity = { dx = -1.2; dy = 0.1 } }
        // { Position = { x = 201.; y = 411. }; Mass = 2.0; Velocity = { dx = -1.; dy = -0.2 } }
        // { Position = { x = 301.; y = 123. }; Mass = 0.5; Velocity = { dx = 1.2; dy = 0.6 } }
        // { Position = { x = 217.; y = 245. }; Mass = 10.0; Velocity = { dx = 0.15; dy = 0.1 } }
        // { Position = { x = 102.; y = 120. }; Mass = 0.1; Velocity = ZeroVelocity }
        // { Position = { x = 302.; y = 120. }; Mass = 0.5; Velocity = { dx = 1.2; dy = -1.01 } }
        // { Position = { x = 7.; y = 35. }; Mass = 1.0; Velocity = { dx = -1.2; dy = 0.1 } }
        // { Position = { x = 202.; y = 431. }; Mass = 2.0; Velocity = { dx = -1.; dy = -0.2 } }
        // { Position = { x = 302.; y = 133. }; Mass = 0.5; Velocity = { dx = 1.2; dy = 0.6 } }
        // { Position = { x = 218.; y = 255. }; Mass = 10.0; Velocity = { dx = 0.15; dy = 0.1 } }
        // { Position = { x = 103.; y = 140. }; Mass = 0.1; Velocity = ZeroVelocity }
        // { Position = { x = 303.; y = 150. }; Mass = 0.5; Velocity = { dx = 1.2; dy = -1.01 } }
        // { Position = { x = 8.; y = 75. }; Mass = 1.0; Velocity = { dx = -1.2; dy = 0.1 } }
        // { Position = { x = 203.; y = 481. }; Mass = 2.0; Velocity = { dx = -1.; dy = -0.2 } }
        // { Position = { x = 303.; y = 193. }; Mass = 0.5; Velocity = { dx = 1.2; dy = 0.6 } }
    ]

    let width = 500.
    let height = 500.
