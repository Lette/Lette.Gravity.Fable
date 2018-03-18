### Lette.Gravity.Fable - To do

_(not in any specific order)_

- [ ] Make canvas stretch to window size
- [ ] Get rid of scroll bars
- [x] Use alpha channel to make objects blend nicer (and use a more interesting/nice color)
- [ ] Give objects individual colors
- [ ] Visualize field strength
- [ ] Make it possible to switch how objects behave at canvas' edges (pass-through, bounce, semi-stop, full-stop)
- [ ] Randomize number, position and initial velocity of objects
- [ ] Make `G` (the gravitational constant) user modifiable
- [ ] Make all initial conditions user modifiable
- [x] Make objects bounce/stop on border at object's edge instead of object's center
- [ ] Make objects bounce off each other (reflect their direction of movement with regards to the tangent of the point of collision, keeping the speed constant)
- [ ] Merge colliding objects when delta-V is sufficient (2 objects become one, with the sum of the two original masses)
- [ ] Split colliding objects when delta-V is sufficient (turn the larger object into a random number of smaller objects, while keeping the sum of all energies constant)
- [x] Extract all physics calculations to a Physics module
- [x] Extract all modifiable settings to a Settings module
- [x] 3D!
- [x] Remove `RequireQualifiedAccess` attribute to get type inference in method signatures working.
- [ ] Add bounce effect
- [ ] Pass the settings object around and make it immutable
