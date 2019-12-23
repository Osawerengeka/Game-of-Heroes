using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HeroesLib
{ 
        public class UnitStack
        {
            public Unit _Unit_ { get; }
            public int qty { get; }
            public UnitStack(Unit U, int qty_)
            {
                _Unit_ = U;
                qty = qty_;
            }

        }
    }


