using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesLib
{
    public interface Ispell
    {

        void Doing(BattleUnitStack UsedUnit, BattleUnitStack OpposeUnit = null, Battle battle = null);
        string Name { get; set; }
        bool solo { get; set; }
        int cooldown { get; set; }
        int duration { get; set; }

        string typeofmagic { get; set; } // buff debuff invoke

    }
}
