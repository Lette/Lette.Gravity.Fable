namespace Lette.Gravity.Fable

[<RequireQualifiedAccess>]
module Domain =

    type Position = { x : float; y : float }
    type Velocity = { dx : float; dy : float }
    type Body = {
        Position : Position;
        Mass : float;
        Velocity : Velocity
    }

    let bodies = [ 
        { Position = { x = 5.; y = 5. }; Mass = 1.0; Velocity = { dx = 0.2; dy = 0.1 } }
        { Position = { x = 700.; y = 400. }; Mass = 2.0; Velocity = { dx = -1.; dy = 0.1 } }
        { Position = { x = 300.; y = 100. }; Mass = 0.5; Velocity = { dx = 1.2; dy = -1.01 } }
    ]

    let width = 800.
    let height = 450.
