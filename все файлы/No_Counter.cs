using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesLib.Modificators
{
    public class No_Counter : Imod
    {
        public string Name { get; set; }
        public int Term { get; set; }

        public No_Counter()
        {
            Name = "No_Counter";
            Term = -1;
        }
    }
}
