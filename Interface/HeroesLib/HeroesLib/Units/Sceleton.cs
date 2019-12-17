using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesLib.Units
{
    public class Sceleton : Unit
    {

        public Sceleton() : base("SCELETON", 5, 1, 2, (1, 1), 10)
        {
            mod.Add(new Modificators.Ice_Imm());
            mod.Add(new Modificators.Fire_Imm());
        }
    }
}
