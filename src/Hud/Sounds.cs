using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Media;

namespace PoeHUD.Hud
{
    public static class Sounds
    {
        public static SoundPlayer AlertSound;
        public static SoundPlayer DangerSoundDefault;
        private static readonly Dictionary<string, SoundPlayer> soundLib = new Dictionary<string, SoundPlayer>();
        
        public static void AddSound(string name)
        {
            Contract.Requires(name != null);
            if (!soundLib.ContainsKey(name))
            {
                try
                {
                    var soundPlayer = new SoundPlayer($"sounds/{name}");
                    soundPlayer.Load();
                    soundLib[name] = soundPlayer;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error when loading {name}| {ex.Message}:", ex);
                }
            }
        }

        public static SoundPlayer GetSound(string name)
        {
            Contract.Requires(name != null);
            return soundLib[name];
        }

        public static void LoadSounds()
        {
            AddSound("alert.wav");
            AddSound("danger.wav");
            AlertSound = GetSound("alert.wav");
            DangerSoundDefault = GetSound("danger.wav");
        }
    }
}