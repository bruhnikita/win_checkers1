using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace win_checkers
{
    public class Square
    {
        public Color Color { get; set; }
        public Checker Checker { get; set; }

        public Square(Color color) 
        {
            Color = color;
            Checker = null;
        }
    }
}
