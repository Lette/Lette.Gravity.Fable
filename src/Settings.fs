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

    // Bounciness (energy/velocity preservation when body hits a wall)
    //    1.0 = full bounce, no energy/velocity loss in the direction of the bounce
    //    0.0 = no bounce, the velocity component of the direction of the wall is set to zero
    let mutable Bounciness = 0.8
