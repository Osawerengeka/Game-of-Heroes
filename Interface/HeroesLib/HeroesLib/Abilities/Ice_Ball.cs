using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesLib.Abilities
{
    class Ice_Ball : Ispell
    {
        public string typeofmagic { get; set; }
        public int duration { get; set; }
        public bool solo { get; set; }

        public string Name { get; set; }
        public Ice_Ball()
        {
            Name = "Ice Ball";
            solo = false;
            cooldown = 0;
        }
        public int cooldown { get; set; }
        public void Doing(BattleUnitStack UsedUnit, BattleUnitStack OpposeUnit = null, Battle battle = null)
        {
            if (!OpposeUnit.mod.Contains(new Modificators.Ice_Imm()))
            {
                int beat = OpposeUnit.bus.Hitpoints + OpposeUnit.bus.qty * OpposeUnit.bus.StandardHitpoints;
                int power = UsedUnit.bus.qty * 50;
                if (beat - power > 0)
                {
                    OpposeUnit.bus.qty = (beat - power) / OpposeUnit.bus.StandardHitpoints;
                    OpposeUnit.bus.Hitpoints = (beat - power) % OpposeUnit.bus.StandardHitpoints;
                    cooldown = 8;
                }
                else
                {
                    string defender = OpposeUnit.bus.Type;
                    battle.Kill(OpposeUnit);

                }
                UsedUnit.canBeUse = false;

            }
            else
            {
                cooldown = 8;
                UsedUnit.canBeUse = false;

            }

        }
    }
}
