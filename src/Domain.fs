namespace Lette.Gravity.Fable

[<AutoOpen>]
module Domain =

    open System

    type Vector = { dx : float; dy : float; dz : float } with
        static member (+) (v1, v2) = { dx = v1.dx + v2.dx; dy = v1.dy + v2.dy; dz = v1.dz + v2.dz }
        static member Zero         = { dx = 0.;            dy = 0.;            dz = 0. }
        member this.Length         = sqrt (this.dx ** 2. + this.dy ** 2. + this.dz ** 2.)

    let scaleVector v k = { dx = k * v.dx; dy = k * v.dy; dz = k * v.dz }

    type Point = { x : float; y : float; z : float } with
        static member (+) (p, v) = { x = p.x + v.dx; y = p.y + v.dy; z = p.z + v.dz }

    let vectorBetween p1 p2 = { dx = p2.x - p1.x; dy = p2.y - p1.y; dz = p2.z - p1.z }

    type Body = { Position : Point; Mass : float; Velocity : Vector }

    let radius { Mass = mass } =
        mass ** (1. / 3.) * 5.

    let initialBodies =

        let numberOfInitialBodies = 10

        let rnd = Random (int System.DateTime.Now.Ticks)
        let nextRandom max = rnd.NextDouble () * max

        let randomBody _ = {
            Position = { x = nextRandom Width; y = nextRandom Height; z = nextRandom Depth };
            Mass = nextRandom 1. ** 5. * 100.; // Favors more smaller bodies
            Velocity = { dx = nextRandom 2. - 1.; dy = nextRandom 2. - 1.; dz = nextRandom 2. - 1. }
        }

        List.init numberOfInitialBodies randomBody 
