using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    /// <summary>
    /// Do to the fact that the maze is beging taken in as a grid of pixels rather than an actual grid, 
    /// A* performs worse than BFS due to the calculations involved with every step. 
    /// </summary>
    class AStar : Solver
    {
        /// <summary>
        /// AStar implementation of the Solver interface. 
        /// </summary>
        /// <param name="maze">Maze to solve</param>
        /// <returns></returns>
        public MazeNode solve(Maze maze)
        {
            List<MazeNode> closedset = new List<MazeNode>();
            PriorityQueue<double, MazeNode> openset = new PriorityQueue<double, MazeNode>();
            List<MazeNode> openset2 = new List<MazeNode>();//using two openset lists because this priority queue doesn't support Contains()
            maze.begin.distance = 0;
            openset.Enqueue(0, maze.begin);
            openset2.Add(maze.begin);
            while (!openset.IsEmpty && openset2.Count > 0)
            {
                MazeNode current = openset.Dequeue();
                openset2.Remove(current);
                if (current.x == maze.end.x && current.y == maze.end.y)
                {
                    return current;
                }
                closedset.Add(current);
                Console.WriteLine(current.x + " " + current.y);
                foreach (MazeNode adjacent in maze.getAdjacentNodes(current))
                {                    
                    if (!adjacent.isWall)
                    {
                        if (closedset.Contains(adjacent))
                            continue;
                        double nextMoveCost = current.distance + distance(current, adjacent);
                        if (nextMoveCost < adjacent.distance)
                        {
                            if (openset2.Contains(adjacent))
                            {
                                openset2.Remove(adjacent);
                                continue;
                            }
                            if (closedset.Contains(adjacent))
                            {
                                closedset.Remove(adjacent);
                                continue;
                            }
                        }
                        if (!openset2.Contains(adjacent) && !closedset.Contains(adjacent))
                        {
                            adjacent.parent = current;
                            adjacent.distance = nextMoveCost;
                            //adjacent.heuristic = nextMoveCost+distance(end, adjacent);                            
                            adjacent.heuristic = distance(maze.end, adjacent);                            
                            openset.Enqueue(adjacent.heuristic, adjacent);
                            openset2.Add(adjacent);
                        }
                    }
                    else
                        closedset.Add(adjacent); 
                }
            }
            return null;

        }   

        public double distance(MazeNode curr, MazeNode end)
        {
            int x = Math.Abs(curr.x - end.x);
            int y = Math.Abs(curr.y - end.y);
            return Math.Sqrt((x * x) + (y * y));
        }
        public double heuristic(MazeNode curr, MazeNode end)
        {
            double dx = Math.Abs(curr.x - end.x);
            double dy = Math.Abs(curr.y - end.y);
            return Math.Max(dx, dy);
        }
        public double heuristic2(MazeNode curr, MazeNode end)
        {
            double dx = Math.Abs(curr.x - end.x);
            double dy = Math.Abs(curr.y - end.y);
            return 1 * (dx + dy) + (1.4 - 2 * 1.4) * Math.Min(dx, dy);
        }
        public double manhattan(MazeNode curr, MazeNode end)
        {
            return Math.Abs(curr.x - end.x) + Math.Abs(curr.y - end.y); 
        }
    }
}