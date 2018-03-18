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

    // Viewing distance, used for 3D projection
    let mutable ViewingDistance = 1500.

    // Show helper lines between projections and objects
    let mutable ShowVerticalHelperLines = true
    let mutable ShowHorizontalHelperLines = false
