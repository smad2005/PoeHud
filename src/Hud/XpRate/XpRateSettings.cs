using PoeHUD.Hud.Settings;

using SharpDX;

namespace PoeHUD.Hud.XpRate
{
    public sealed class XpRateSettings : SettingsBase
    {
        public XpRateSettings()
        {
            Enable = true;
            TextSize = new RangeNode<int>(20, 10, 50);
            BackgroundColor = new ColorBGRA(0, 0, 0, 180);
            ShowXpPenalty = false;
        }

        public RangeNode<int> TextSize { get; set; }

        public ColorNode BackgroundColor { get; set; }

        public ToggleNode ShowXpPenalty { get; set; }
    }
}