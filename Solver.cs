using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    /// <summary>
    /// Inteface for a maze solver
    /// </summary>
    interface Solver
    {
        MazeNode solve(Maze maze);
    }
}
