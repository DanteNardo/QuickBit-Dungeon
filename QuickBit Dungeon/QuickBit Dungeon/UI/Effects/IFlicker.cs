using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickBit_Dungeon.UI.EFFECTS
{
    public interface IFlicker
    {
        double FlickerMin { get; set; }
        double FlickerFraction { get; set; }
        double FlickerStart { get; set; }
        double FlickerCurrent { get; set; }
        double FlickerNext { get; set; }

        void BeginFlicker();
        void Flicker();
        bool Flickering();
        double Interpolate();
        void SetFlick();
    }
}
