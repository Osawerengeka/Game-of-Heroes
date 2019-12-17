using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace HeroesLib
{    
        public interface Imod
        {
            //void doing(BattleUnitStack UsedUnit, BattleUnitStack OpposeUnit = null);
            string Name { get; set; }
            int Term { get; set; }

        }

}
