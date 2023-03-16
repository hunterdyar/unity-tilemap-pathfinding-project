# Tilemap Pathfinding
Extending Unity's Tilemap system to support pathfinding, and other tools used in grid based games. 

![Example](Documentation~/path1.gif)

The goal is to allow the user to seamlessly draw/paint maps with Unity's built-in tilemap system, and have it _just work_ with pathfinding. 

## Features
- A*, Dijkstra's, and bread-first pathfinding algorithms.
- Pathfinding can be extended and used with other systems (Just implement IGraph and INode interfaces)
- Tiles with different costs
- All types of tilemap layouts (rectangular grid, isometic grid, hexagonal grid)

## Pros and Cons
- Pro: No object-initialization per tile. Many solutions read the tilemap and spawn some kind of 'node' object for each one. The advantage is easily doing hover effects, but the disadvantage is that our map basically exists twice.
- Pro: Just use the tilemap system like normal. Lots of existing solutions are just prefab painting on a grid, with lots of editor convenience tools. That's fine, but I want to use Unity's tools that we already know.
- Con: NavTiles. One has to use the NavTile type that extends from tile, which stores information like weight. It's _absolutely_ possible not to have to use this, from a systems point of view, but I don't plan on supporting "default tiles are assumed walkable with cost of 1". If we are going to do it, we should do it right.
- Con: If you are using ScriptableTiles, which NavTile basically is, like the Road example un unity's manual, that will need to adjust it to extend from NavTile instead of Tile. Everything else should still work, NavTile just adds fields, it doesn't override RefreshTile or so on.
 
## Not Supported (Yet)
- Hex and iso grids not yet tested.
- Obstacles & Agents. NavTiles that know that objects are on top of them, blocking the way or adding cost for pathfinding.
- Encapsulate pathfinding in coroutine for single-thread non-blocking (er... less blocking?) searches.
- Example movement and usage scripts

Playground for testing adding pathfinding to unity tilemaps. This is a 2DURP project, and when done, I will probably move it to a proper "package" project.


