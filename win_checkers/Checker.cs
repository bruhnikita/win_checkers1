using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace win_checkers
{
    public class Checker
    {
        public bool IsKing {  get; set; }
        public Color Color { get; set; }

        public Checker(Color color)
        {
            IsKing = false;
            Color = color;
        }
    }
}
