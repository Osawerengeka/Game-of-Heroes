using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesLib.Modificators
{
    public class Fire_Imm : Imod
    {
        public string Name { get; set; }
        public int Term { get; set; }

        public Fire_Imm()
        {
            Name = "FireImm";
            Term = -1;
        }
    }
}
