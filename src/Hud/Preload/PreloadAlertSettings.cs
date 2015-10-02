﻿using PoeHUD.Hud.Settings;

using SharpDX;

namespace PoeHUD.Hud.Preload
{
    public sealed class PreloadAlertSettings : SettingsBase
    {
        public PreloadAlertSettings()
        {
            Enable = true;
            FontSize = new RangeNode<int>(16, 10, 20);
            BackgroundColor = new ColorBGRA(255, 255, 255, 220);
            FastColor = CorruptedColor = new ColorBGRA(254, 192, 118, 255);

            MasterZana = new ColorBGRA(255, 255, 0, 255);
            MasterCatarina = new ColorBGRA(254, 192, 118, 255);
            MasterTora = new ColorBGRA(254, 192, 118, 255);
            MasterVorici = new ColorBGRA(254, 192, 118, 255);
            MasterHaku = new ColorBGRA(254, 192, 118, 255);
            MasterElreon = new ColorBGRA(254, 192, 118, 255);
            MasterVagan = new ColorBGRA(254, 192, 118, 255);
            MasterKrillson = new ColorBGRA(240, 0, 240, 255);

            ArcanistStrongbox = new ColorBGRA(255, 0, 255, 255);
            ArtisanStrongbox = new ColorBGRA(0, 255, 0, 255);
            CartographerStrongbox = new ColorBGRA(255, 0, 255, 255);
            GemcutterStrongbox = new ColorBGRA(27, 162, 155, 255);
            JewellerStrongbox = new ColorBGRA(254, 192, 118, 255);
            BlacksmithStrongbox = new ColorBGRA(254, 192, 118, 255);
            ArmourerStrongbox = new ColorBGRA(254, 192, 118, 255);
            OrnateStrongbox = new ColorBGRA(254, 192, 118, 255);
            LargeStrongbox = new ColorBGRA(254, 192, 118, 255);

            OrraGreengate = new ColorBGRA(254, 192, 118, 255);
            ThenaMoga = new ColorBGRA(254, 192, 118, 255);
            AntalieNapora = new ColorBGRA(254, 192, 118, 255);
            TorrOlgosso = new ColorBGRA(254, 192, 118, 255);
            ArmiosBell = new ColorBGRA(254, 192, 118, 255);
            ZacharieDesmarais = new ColorBGRA(254, 192, 118, 255);
            MinaraAnenima = new ColorBGRA(254, 192, 118, 255);
            IgnaPhoenix = new ColorBGRA(254, 192, 118, 255);
            JonahUnchained = new ColorBGRA(254, 192, 118, 255);
            DamoiTui = new ColorBGRA(254, 192, 118, 255);
            XandroBlooddrinker = new ColorBGRA(254, 192, 118, 255);
            VickasGiantbone = new ColorBGRA(254, 192, 118, 255);
            EoinGreyfur = new ColorBGRA(254, 192, 118, 255);
            TinevinHighdove = new ColorBGRA(254, 192, 118, 255);
            MagnusStonethorn = new ColorBGRA(254, 192, 118, 255);
            IonDarkshroud = new ColorBGRA(254, 192, 118, 255);
            AshLessard = new ColorBGRA(254, 192, 118, 255);
            WilorinDemontamer = new ColorBGRA(254, 192, 118, 255);
            AugustinaSolaria = new ColorBGRA(254, 192, 118, 255);
        }

        public RangeNode<int> FontSize { get; set; }
        public ColorNode BackgroundColor { get; set; }
        public ColorNode FastColor { get; set; }
        public ColorNode CorruptedColor { get; set; }
        public ColorNode MasterZana { get; set; }
        public ColorNode MasterCatarina { get; set; }
        public ColorNode MasterTora { get; set; }
        public ColorNode MasterVorici { get; set; }
        public ColorNode MasterHaku { get; set; }
        public ColorNode MasterElreon { get; set; }
        public ColorNode MasterVagan { get; set; }
        public ColorNode MasterKrillson { get; set; }
        public ColorNode ArcanistStrongbox { get; set; }
        public ColorNode ArtisanStrongbox { get; set; }
        public ColorNode CartographerStrongbox { get; set; }
        public ColorNode GemcutterStrongbox { get; set; }
        public ColorNode JewellerStrongbox { get; set; }
        public ColorNode BlacksmithStrongbox { get; set; }
        public ColorNode ArmourerStrongbox { get; set; }
        public ColorNode OrnateStrongbox { get; set; }
        public ColorNode LargeStrongbox { get; set; }
        //public ColorNode SimpleStrongbox { get; set; }

        public ColorNode OrraGreengate { get; set; }
        public ColorNode ThenaMoga { get; set; }
        public ColorNode AntalieNapora { get; set; }
        public ColorNode TorrOlgosso { get; set; }
        public ColorNode ArmiosBell { get; set; }
        public ColorNode ZacharieDesmarais { get; set; }
        public ColorNode MinaraAnenima { get; set; }
        public ColorNode IgnaPhoenix { get; set; }
        public ColorNode JonahUnchained { get; set; }
        public ColorNode DamoiTui { get; set; }
        public ColorNode XandroBlooddrinker { get; set; }
        public ColorNode VickasGiantbone { get; set; }
        public ColorNode EoinGreyfur { get; set; }
        public ColorNode TinevinHighdove { get; set; }
        public ColorNode MagnusStonethorn { get; set; }
        public ColorNode IonDarkshroud { get; set; }
        public ColorNode AshLessard { get; set; }
        public ColorNode WilorinDemontamer { get; set; }
        public ColorNode AugustinaSolaria { get; set; }
    }
}