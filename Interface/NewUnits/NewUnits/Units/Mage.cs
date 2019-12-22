using System;
using System.Collections.Generic;
using System.Text;
using HeroesLib.Units;
using HeroesLib;
using NewUnits.Abilities;
namespace NewUnits.Units
{
    public class Mage : Unit
    {
        public Mage() : base("MAGE", 3, 5, 1, (10, 10), 19,2,4)
        {
            abl.Add(new InvokeWolf());

        }
    }

}
