using PoeHUD.Hud.Settings;

namespace PoeHUD.Hud.KC
{
    public sealed class KillCounterSettings : SettingsBase
    {
        public KillCounterSettings()
        {
            Enable = false;
            ShowDetail = true;
            PerSession = false;
        }

        public ToggleNode ShowDetail { get; set; }

        public ToggleNode PerSession { get; set; }
    }
}