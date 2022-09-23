using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    /// <summary>
    /// Node class to be used for maze. Ideally 
    /// a solver would inherit this base class and add their own member variable and methods.
    /// </summary>
    class MazeNode
    {
        private int _x;
        private int _y;
        private bool _isWall;
        private bool _visited;
        private bool _searched; 
        private MazeNode _parent;
        private double _distance;
        private double _fscore; 
        public int x
        {
            set { _x = value;}
            get { return _x; }
        }

        public int y
        {
            set {  _y = value;}
            get { return _y; }
        }
        
        public MazeNode(int xCoord, int yCoord)
        {
            x = xCoord;
            y = yCoord;
            visited = false;
            parent = null;
            searched = false;
            isWall = false;
            heuristic = 0;
            distance = 99999999; 
        }
        public bool isWall
        {
            get { return _isWall; }
            set { _isWall = value; }
        }

        public bool visited
        {
            get { return _visited; }
            set { _visited = value; }
        }
        public MazeNode parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        public bool searched
        {
            get { return _searched; }
            set { _searched = value; }
        }

        public double distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        public double heuristic
        {
            get { return _fscore; }
            set { _fscore = value;  }
        }
        public string getString()
        {
            return this.x + " " + this.y; 
        }

    }

}

