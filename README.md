# Tilemap Pathfinding
Extending Unity's Tilemap system to support pathfinding, and other tools used in grid based games. 

The goal is to allow the user to seamlessly draw/paint with the tile system that Unity has. 

## Pros and Cons
- Pro: No object-initialization per tile. Many solutions read the tilemap and spawn some kind of 'node' object for each one. The advantage is easily doing hover effects, but the disadvantage is that our map basically exists twice.
- Pro: Just use the tilemap system like normal. Lots of existing solutions are just prefab painting on a grid, with lots of editor convenience tools. That's fine, but I want to use Unity's tools that we already know.
- Con: NavTiles. One has to use the NavTile type that extends from tile, which stores information like weight. It's _absolutely_ possible not to have to use this, from a systems point of view, but I don't plan on supporting "walk on default tiles with cost of 1" because if we are going to do it right.
- Con: If you are using ScriptableTiles, which NavTile basically is, like the Road example un unity's manual, that will need to extend NavTile instead of Tile. Everything else should still work, NavTile just adds data, it doesn't override RefreshTile or such.
 
## Not Supported (Yet)
- Obstacles & Agents. NavTiles that know that objects are on top of them, blocking the way or adding cost for pathfinding.
- Encapsulate pathfinding in coroutine for single-thread non-blocking (er... less blocking?) searches.
- Example movement and usage scripts

Playground for testing adding pathfinding to unity tilemaps. This is a 2DURP project, and when done, I will probably move it to a proper "package" project.


