The Priority Queue implementation is by BlueRaja, released under MIT license.

https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp

I am just using SimplePriorityQueue, instead of the very optimized Fast. 
According to BlueRaja, it's ~10-15% slower. I can live with that for two reasons:

1. I want to encourage others (like my students) to hack at my code, so readability and editability are more important than normal.
2. Resetting the nodes, for frequent searches of very small graphs (which is my use-case), takes away some of the speed benefit. Although I should profile it.
3. fastpathfinder needs max node count. We would have to be sure to recreate the pathfinder if we change the size of the map. Which can be done easily, but just one more thing to consider.
It would not be terribly difficult to refactor the code to use the faster queue, but thread-safe is good, and it's still pretty fast. 


---
I've just included the source files instead of the nuget package in order to have everything contained in this project for release, and convenience for those hacking away at it, who may be completely unfamiliar with nuget.


