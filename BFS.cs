using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    class BFS: Solver
    {
        /// <summary>
        /// BFS implementation of the Solver interface
        /// </summary>
        /// <param name="maze">Maze object to pass in</param>
        /// <returns></returns>
        public MazeNode solve(Maze maze)
        {                        
            Queue<MazeNode> queue = new Queue<MazeNode>();
            maze.begin.searched = true;
            queue.Enqueue(maze.begin); 
            while(queue.Count() > 0)
            {
                MazeNode currNode = queue.Dequeue(); 
                if(currNode.x == maze.end.x && currNode.y == maze.end.y)
                {
                    return currNode; 
                }
                List<MazeNode> adjacents = maze.getAdjacentNodes(currNode); 
                foreach(MazeNode adjacent in adjacents)
                {
                    if(!adjacent.visited && !adjacent.searched)
                    {
                        adjacent.searched = true;
                        adjacent.parent = currNode;
                        queue.Enqueue(adjacent);                        
                    }
                }
                currNode.visited = true;
                currNode.searched = true;
            }
            return null;
        }
    }
}