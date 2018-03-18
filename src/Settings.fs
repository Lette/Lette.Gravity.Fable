namespace Lette.Gravity.Fable

[<AutoOpen>]
module Settings =

    // Gravitational constant
    let mutable G = 2.

    // Dimensions of the simulation universe
    let mutable Width = 500.
    let mutable Height = 500.
    let mutable Depth = 500.

    // Reduces slingshot effect when objects are really close, and avoids
    // division by zero in force calculations.
    let mutable MinimumProximity = 4.
