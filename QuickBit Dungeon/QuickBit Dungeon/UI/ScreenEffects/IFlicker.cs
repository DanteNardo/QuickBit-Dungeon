namespace QuickBit_Dungeon.UI.Effects
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
