namespace Lette.Gravity.Fable

module App =

    let adjustForBounds bound x =
        if x < 0. then
            bound - x
        else if x > bound then
            x - bound
        else
            x

    let moveBody (body : Domain.Body) =
        let newX = body.Position.x + body.Velocity.dx
        let newY = body.Position.y + body.Velocity.dy
        { body with Position = { x = adjustForBounds Domain.width newX; y = adjustForBounds Domain.height newY } }

    let moveBodies bodies =
        bodies |> List.map moveBody

    let rec runSimulation ctx bodies =
        async {
            let newBodies = moveBodies bodies

            Rendering.resetCanvas ctx
            Rendering.drawBodies ctx newBodies

            do! Async.Sleep (1)
            return! runSimulation ctx newBodies
        }

    let init () =
        async {
            let ctx = Rendering.init ()
            return! runSimulation ctx Domain.bodies
        }

    init() |> Async.StartImmediate
