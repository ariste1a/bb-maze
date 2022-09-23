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
    /// <summary>
    /// Maze Object. 
    /// Holds object representation of maze image using pixels as elements of a grid 
    /// Currently uses pixels to MazeNode objects, however a better idea would be to 
    /// somehow transform the image into bigger block elements. 
    /// </summary>
    class Maze
    {
        //black
        private static Color wallColor = Color.FromArgb(255, 0, 0, 0);
        //white
        private static Color spaceColor = Color.FromArgb(255, 255, 255, 255);
        //red
        private static Color startColor = Color.FromArgb(255, 255, 0, 0);
        //blue
        private static Color endColor = Color.FromArgb(255, 0, 0, 255);
        //green
        private static Color solutionColor = Color.FromArgb(255, 0, 255, 0); 

        private int _height;
        private int _width;
        private MazeNode[,] _grid; 
        private MazeNode _end; 
        private MazeNode _begin;
        private Bitmap _image;
        private string _filePath; 
        //height of the image
        public int height
        {
            set { _height = value;  }
            get { return _height;  }
        }
        //width of the image
        public int width
        {
            set { _width = value; }
            get { return _width; }
        } 
       
        public MazeNode[,] grid
        {
            set { _grid = value; }
            get { return _grid;  }
        }
        //start node
        public MazeNode begin
        {
            set { _begin = value;  }
            get { return _begin; }
        }
        //end node
        public MazeNode end
        {
            set { _end = value;  }
            get { return _end;  }
        }
        //Bitmap object representation of the image
        public Bitmap image
        {
            set { _image = value; }
            get { return _image; }
        }
        //Filepath of the image
        public string filePath
        {
            set { _filePath = value; }
            get { return _filePath; }
        }


        /// <summary>
        /// Maze Constructor. Initializes the grid of MazeNodes by looking at the color of pixels. 
        /// Uses LockBits instead of getPixel()/setPixel() for faster accesssing/writing.
        /// </summary>
        public Maze(Bitmap image, string filePath)
        {
            //initializing member variables
            this.image = image;
            this.filePath = filePath; 
            this.grid = new MazeNode[image.Width, image.Height];
            this.height = image.Height;
            this.width = image.Width;
            
            //Locking the bitmap into system memory and filling byte array with image data. 
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                image.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes  = Math.Abs(bmpData.Stride) * image.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            //traversing the image pixel by pixel and creating the grid. 
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //fancy math that finds the current position of the pixel in the byte array
                    int position = (j * bmpData.Stride) + (i * System.Drawing.Image.GetPixelFormatSize(bmpData.PixelFormat) / 8);
                    if (position + 2 < bytes)
                    {                        
                        byte blue = rgbValues[position];
                        byte green = rgbValues[position + 1];
                        byte red = rgbValues[position + 2];
                        //If a wall
                        if (blue == wallColor.B && green == wallColor.G && red == wallColor.R)
                        {
                            grid[i, j] = new MazeNode(i, j);
                            grid[i, j].isWall = true;
                            grid[i, j].visited = true;
                            grid[i, j].searched = true;
                            continue;
                        }
                        //If white
                        else if (blue == spaceColor.B && green == spaceColor.G && red == spaceColor.R)
                        {
                            grid[i, j] = new MazeNode(i, j);
                            grid[i, j].isWall = false;
                            grid[i, j].visited = false;
                            continue;
                        }
                        //If start
                        else if (blue == startColor.B && green == startColor.G && red == startColor.R)
                        {
                            grid[i, j] = new MazeNode(i, j);
                            grid[i, j].visited = false;
                            if (begin == null)
                                begin = grid[i, j];
                            continue;
                        }
                        //If end
                        else if (blue == endColor.B && green == endColor.G && red == endColor.R)
                        {
                            grid[i, j] = new MazeNode(i, j);
                            grid[i, j].visited = false;
                            if (end == null)
                                end = grid[i, j];
                            continue;
                        }
                        else
                        {
                            grid[i, j] = new MazeNode(i, j);
                            grid[i, j].isWall = false;
                            grid[i, j].visited = false;
                        }
                        throw new Exception(string.Format("Unrecognized color {0} ", Color.FromArgb(255,blue, green, red))); 
                    }
                }
            }
            if (begin == null)
            {
                throw new Exception(string.Format("Could not find starting color. Should be {0}", startColor));
            }
            if (end == null)
                throw new Exception(string.Format("Could not find goal color. Should be {0}", endColor));  
            //remember to unlock the data
            image.UnlockBits(bmpData);
        }

        /// <summary>
        /// Returns the surrounding 8 adjacent nodes for a given node. 
        /// </summary>
        public List<MazeNode> getAdjacentNodes(MazeNode curr)
        {
            //position of pixel
            int x = curr.x;
            int y = curr.y;
            List<MazeNode> adjacent = new List<MazeNode>();
            if (y < 0 || y >= this.height || x < 0 || x >= this.width)
                return adjacent;
            //search 8 squares around it
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    //edge cases
                    if (i < 0 || i >= width || j < 0 || j >= width || (i == x && j == y))
                        continue;
                    adjacent.Add(grid[i, j]);
                }
            }
            return adjacent;
        }
        
        /// <summary>
        /// Method that is called to solve the maze. 
        /// </summary>
        /// <param name="solver"> Pass in the type of solver (E.g. BFS, Astar, Dijkstra, etc)</param>
        /// <param name="resultPath">The path to save the resulting image</param>
        
        public void solve(Solver solver, String resultPath)
        {
            drawRoute(solver.solve(this), resultPath); 
        }

        /// <summary>
        /// Draws the solution path to an image and saves to disk. 
        /// </summary>
        /// <param name="end">The node which is returned by the solver, containing the path of the solution from pointers to parent nodes</param>
        /// <param name="resultPath">Filepath of the resulting image</param>
        public void drawRoute(MazeNode end, String resultPath)
        {
            Bitmap bmp = new Bitmap(this.filePath);
            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            //Traverse the parent nodes to draw the path in green
            MazeNode temp = end;            
            while(temp != null)
            {
                int position = (temp.y * bmpData.Stride) + (temp.x* Image.GetPixelFormatSize(bmpData.PixelFormat) / 8);                
                rgbValues[position] = 0;
                rgbValues[position + 1] = 255;
                rgbValues[position + 2] = 0;
                temp = temp.parent;
            }
            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);            
            bmp.UnlockBits(bmpData);
            bmp.Save(resultPath);
        }
    }
}
