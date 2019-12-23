using System;
using System.Collections.Generic;
using System.Text;
using HeroesLib.Units;
using HeroesLib;
using NewUnits.Units;
namespace NewUnits.Abilities
{
    class InvokeWolf : HeroesLib.Ispell
    {
        public string Name { get; set; }
        public int duration { get; set; }
        public bool solo { get; set; }

        public string typeofmagic { get; set; }
        public int cooldown { get; set; }

        public InvokeWolf()
        {
            solo = false;
            Name = "Invoke Wolf";
            typeofmagic = "invoke";
        }

        public void Doing(BattleUnitStack UsedUnit, BattleUnitStack OpposeUnit = null, HeroesLib.Battle battle = null)
        {
            if (battle.player[0].army.Contains(UsedUnit))
            {
                battle.player[0].army.Add(new BattleUnitStack(new UnitStack(new Wolf(), 10)));
            }
            else
            {
                battle.player[1].army.Add(new BattleUnitStack(new UnitStack(new Wolf(), 10)));
            }
            UsedUnit.canBeUse = false;
        }
    }
}
