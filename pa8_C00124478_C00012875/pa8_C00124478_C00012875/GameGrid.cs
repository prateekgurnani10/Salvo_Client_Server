using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pa8_C00124478_C00012875
{
    class GameGrid
    {
        Random RNG = new Random();
        public enum Outcomes { Occupied, Empty, Hit, Miss }; //Determines the state of a grid space
        private Outcomes[,] grid;
        private int size;  
        public int Size { get { return size; } }

        public GameGrid(int size)
        {
            this.size = size;   
            grid = new Outcomes[size, size]; //creates grid based on desired size 

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                    grid[i, j] = Outcomes.Empty;
            }
        }

        //Targets an empty grid space, assigns hit or miss value depending on contents
        //With the server/client logic, this will receive coordinates from StreamReader
        public void TakeShot(int x, int y)
        {
            if (ValidCoords(x,y))
            {
                if (Hits(x, y))
                {
                    grid[x, y] = Outcomes.Hit;
                }
                else
                {
                    grid[x, y] = Outcomes.Miss;
                }
            }
        }

        //Determines if coordinates are valid on this grid (index starts at 0. So A1 == [0,0])
        private bool ValidCoords(int x, int y)
        {
            return (x < size && x >= 0 && y < size && y >= 0);
        }

        //Helper Method: Determines if a coordinate contains a ship
        private bool Hits(int x, int y)
        {
            if (ValidCoords(x, y)) 
                return (grid[x, y] == Outcomes.Occupied);
            return false;
        }

        //Helper Method: Sets the GridSpace to whatever desired outcome
        private void SetGridSpace(int x, int y, Outcomes outcome)
        {
            if (ValidCoords(x, y))
                grid[x, y] = outcome;
        }

        //Returns the state of a grid space
        public Outcomes CheckGridSpace(int x, int y)
        {
            if(ValidCoords(x,y))
            return grid[x, y];
            Console.WriteLine("Invalid Coordinates");
            return grid[0, 0];
        }

        //Algorithm to generate random horizontal ships
        public void GenerateShip()
        {
            int headX = RNG.Next(0, size);    //Random starting X-coordinate
            int headY = RNG.Next(0,size);    //Random starting Y-coordinate
            bool isHorizontal = (RNG.Next(1,4)==2);  //Random switch to represent vertical or horizontal generation
            SetGridSpace(headX, headY, Outcomes.Occupied);

            if (isHorizontal)   //Make horizontal ship
            {
                if (headX < size-1)
                {
                    SetGridSpace(headX + 1, headY, Outcomes.Occupied);

                    if (headX + 1 < size-1)
                    {
                        SetGridSpace(headX + 2, headY, Outcomes.Occupied);
                    }
                    else
                    {
                        SetGridSpace(headX - 1, headY, Outcomes.Occupied);
                    }
                }
                else
                {
                    SetGridSpace(headX - 1, headY, Outcomes.Occupied);
                    SetGridSpace(headX - 2, headY, Outcomes.Occupied);
                }
            }
            else           //Make vertical ship
            {
                if (headY < size-1)
                {
                    SetGridSpace(headX, headY + 1, Outcomes.Occupied);

                    if (headY + 1 < size-1)
                    {
                        SetGridSpace(headX, headY + 2, Outcomes.Occupied);
                    }
                    else
                    {
                        SetGridSpace(headX, headY - 1, Outcomes.Occupied);
                    }
                }
                else
                {
                    SetGridSpace(headX, headY - 1, Outcomes.Occupied);
                    SetGridSpace(headX, headY - 2, Outcomes.Occupied);
                }
            }
            
        }

        //Turns the Grid State into a list, which interacts with the form
        public List<Outcomes> toList()
        {
            List<Outcomes> list = new List<Outcomes>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    list.Add(grid[i, j]);
                }
            }
            return list;
        }
    }
}
