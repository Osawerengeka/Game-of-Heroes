using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesLib.Modificators
{
    public class Ice_Imm : Imod
    {
        public string Name { get; set; }
        public int Term { get; set; }

        public Ice_Imm()
        {
            Name = "IceImm";
            Term = -1;
        }
    }
}
