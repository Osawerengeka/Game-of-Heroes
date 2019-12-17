using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLib
{
        class Army
        {

            private UnitStack spot1 { get; }
            private UnitStack spot2 { get; }
            private UnitStack spot3 { get; }
            private UnitStack spot4 { get; }
            private UnitStack spot5 { get; }
            private UnitStack spot6 { get; }
            public Army(UnitStack spot1_, UnitStack spot2_, UnitStack spot3_, UnitStack spot4_, UnitStack spot5_, UnitStack spot6_)
            {
                spot1 = spot1_;
                spot2 = spot2_;
                spot3 = spot3_;
                spot4 = spot4_;
                spot5 = spot5_;
                spot6 = spot6_;
            }

        }
    }
