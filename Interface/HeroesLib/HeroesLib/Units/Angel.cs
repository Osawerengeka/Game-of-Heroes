using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesLib.Units
{
    public class Angel : Unit
    {
        public Angel() : base("ANGEL", 180, 27, 27, (45, 45), 11)
        {
            mod.Add(new Modificators.No_Counter());
            mod.Add(new Modificators.Fire_Imm());
        }
    }
}
