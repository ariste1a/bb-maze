using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace MazeSolver
{
    class MazeSolver
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {          
                if (args.Length ==1 && args[0] == "-h")
                {
                    Console.WriteLine("Usage:");
                    Console.WriteLine("\tmaze.exe “source.[bmp,png,jpg]” “destination.[bmp,png,jpg] \"");
                    Console.WriteLine("\t\t  source: the input file");
                    Console.WriteLine("\t\t  destination: the output file");
                    return;
                }
                Console.WriteLine("Invalid number of arguments");
                printHelp();      
                return; 
            }
            string location = args[0];
            string newLoc = args[1];                          
            try
            {
                if (!File.Exists(location))
                    throw new FileNotFoundException(string.Format("File {0} not found", location));
                // Create a new bitmap.            
                Bitmap bmp = new Bitmap(location);
                bmp = new Bitmap(location);
                Console.WriteLine();
                Console.WriteLine("Solving....");
                Console.WriteLine();
                Maze maze = new Maze(bmp, location);
                Solver bfs = new BFS();
                //Solver astar = new AStar();                
                maze.solve(bfs, newLoc);
                System.Console.WriteLine("Done");
            }
            catch (Exception e)
            {
                Console.WriteLine();                
                Console.WriteLine(e.Message);                
            }            
            
        }
        public static void printHelp()
        {
            Console.WriteLine();           
            Console.WriteLine("Type -h for more help");
        }
      
    }

}