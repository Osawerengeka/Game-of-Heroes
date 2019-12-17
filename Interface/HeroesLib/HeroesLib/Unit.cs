using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLib
{
        public class Unit
        {
            public List<Imod> mod = new List<Imod>();
            public List<Ispell> abl = new List<Ispell>();
            public (int, int) Damage { get; }
            public string Type { get; }
            public int Hitpoints { get; }
            public int Attack { get; }
            public int Defence { get; }
            public int Initiative { get; }

            public Unit(string typ, int hit, int att, int def, (int, int) dam, int Ini)
            {
                Damage = dam;
                Type = typ;
                Hitpoints = hit;
                Attack = att;
                Defence = def;
                Initiative = Ini;

            }
        }
    }
