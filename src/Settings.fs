namespace Lette.Gravity.Fable

[<RequireQualifiedAccess>]
module Settings =

    // Gravitational constant
    let mutable G = 0.2

    // Dimensions of the simulation universe
    let mutable Width = 500.
    let mutable Heigth = 500.

    // Reduces slingshot effect when objects are really close, and avoids
    // division by zero in force calculations.
    let mutable MinimumProximitySquared = 20.
