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
        public Mage() : base("Mage", 3, 5, 1, (10, 10), 19)
        {
            abl.Add(new InvokeWolf());

        }
    }

}
