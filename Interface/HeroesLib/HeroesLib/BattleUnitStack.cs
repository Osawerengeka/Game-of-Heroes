using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLib
{

        public class BattleUnitStack
        {
            public int positionX;
            public int positionY;
            public bool canBeUse = true;
            public int army;


            public struct BUnitStack
            {
                public (int, int) Damage;
                public string Type;
                public int Hitpoints;
                public int StandardHitpoints;
                public int Attack;
                public int Defence;
                public int StandardInitiative;
                public int Initiative;
                public int qty;
                public int StandardDefence;

            }
            public BUnitStack bus;
            //public Posibilities mdf;
            public List<Ispell> abl;
            public List<Imod> mod;
            public BattleUnitStack(UnitStack b)
            {
                bus.qty = b.qty;
                bus.Damage = b._Unit_.Damage;
                bus.Type = b._Unit_.Type;
                bus.Hitpoints = 0;
                bus.StandardHitpoints = b._Unit_.Hitpoints;
                bus.Attack = b._Unit_.Attack;
                bus.Defence = b._Unit_.Defence;
                bus.StandardDefence = b._Unit_.Defence;
                bus.Initiative = b._Unit_.Initiative;
                bus.StandardInitiative = b._Unit_.Initiative;
                abl = b._Unit_.abl;
                mod = b._Unit_.mod;
            }
            public void changeParametrs((int, int) damch, int hitpointsch, int attch, int defch, int inich)
            {
                this.bus.Damage.Item1 += damch.Item1;
                this.bus.Damage.Item2 += damch.Item2;
                this.bus.Hitpoints += hitpointsch;
                this.bus.Initiative += inich;
                this.bus.Attack += attch;
                this.bus.Defence += defch;
            }
        }
    }