using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLib.Units
{
    public class Warrior : Unit
    {

        public Warrior() : base("WARRIOR", 16, 5, 3, (5, 7), 16,3,2)
        {
            abl.Add(new Abilities.Double_Attack());

        }
    }
}
