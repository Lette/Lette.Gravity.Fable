namespace Lette.Gravity.Fable

[<AutoOpen>]
module Settings =

    // Gravitational constant
    let mutable G = 15.

    // Dimensions of the simulation universe
    let mutable Width = 1000.
    let mutable Height = 500.
    let mutable Depth = 500.

    // Reduces slingshot effect when objects are really close, and avoids
    // division by zero in force calculations.
    let mutable MinimumProximity = 20.

    // Viewing distance, used for 3D projection
    let mutable ViewingDistance = 1500.

    // Show helper lines between projections and objects
    let mutable ShowVerticalHelperLines = true
    let mutable ShowHorizontalHelperLines = false

    // Bounciness (energy/velocity preservation when body hits a wall)
    //    1.0 = full bounce, no energy/velocity loss in the direction of the bounce
    //    0.0 = no bounce, the velocity component of the direction of the wall is set to zero
    let mutable Bounciness = 0.8

    // Preservation of velocity (ie. friction effect)
    //    1.0  = full preservation, no slowdown/speedup
    //    0.99 = loses 1 % of velocity every frame
    let mutable VelocityFactor = 1.0

    // When object "collide" (ie. the distance between them is less than their combined radii)
    // they can optionally merge into a single object.
    let mutable MergeCollidingObjects = true
