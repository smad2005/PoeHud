using System;
using System.Media;

namespace PoeHUD.Framework.Helpers
{
    public static class SoundHelper
    {
        public static void Play(this SoundPlayer player, ushort volume)
        {
            const ushort MAX_VOLUME = 300;
            if (volume > MAX_VOLUME)
                volume = MAX_VOLUME;
            var newVolume = (ushort)((float)volume / MAX_VOLUME * ushort.MaxValue);
            var stereo = (newVolume | (uint)newVolume << 16);
            WinApi.waveOutSetVolume(IntPtr.Zero, stereo);
            player.Play();
        }
    }
}