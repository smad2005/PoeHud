using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using PoeHUD.Controllers;
using PoeHUD.Framework;
using PoeHUD.Framework.Helpers;
using PoeHUD.Hud.UI;
using PoeHUD.Models;
using SharpDX;
using SharpDX.Direct3D9;

namespace PoeHUD.Hud.Preload
{
    public class PreloadAlertPlugin : SizedPlugin<PreloadAlertSettings>
    {
        private readonly HashSet<PreloadAlerConfigLine> alerts;

        private readonly Dictionary<string, PreloadAlerConfigLine>  alertStrings;

        private bool areaChanged = true;

        private DateTime maxParseTime = DateTime.Now;

        private int lastCount;

        public PreloadAlertPlugin(GameController gameController, Graphics graphics, PreloadAlertSettings settings)
            : base(gameController, graphics, settings)
        {
            Contract.Requires(gameController != null);
            alerts = new HashSet<PreloadAlerConfigLine>();
            alertStrings = LoadConfig("config/preload_alerts.txt");
            GameController.Area.OnAreaChange += OnAreaChange;
        }


        public Dictionary<string, PreloadAlerConfigLine> LoadConfig(string path)
        {
            return LoadConfigBase(path, 3).ToDictionary(line => line[0], line =>
            {
                var preloadAlerConfigLine = new PreloadAlerConfigLine { Text = line[1], Color = line.ConfigColorValueExtractor(2)};
                return preloadAlerConfigLine;
            });
        }

        public override void Render()
        {
            base.Render();
            if (!Settings.Enable)
            {
                return;
            }

            if (areaChanged)
            {
                Parse();
                lastCount = GetNumberOfObjects();
            }
            else if (DateTime.Now <= maxParseTime)
            {
                int count = GetNumberOfObjects();
                if (lastCount != count)
                {
                    areaChanged = true;
                }
            }

            if (alerts.Count > 0)
            {
                Vector2 startPosition = StartDrawPointFunc();
                Vector2 position = startPosition;
                int maxWidth = 0;
                foreach (var preloadAlerConfigLine in alerts)
                {
                    Size2 size = Graphics.DrawText(preloadAlerConfigLine.Text, Settings.TextSize, position, preloadAlerConfigLine.FastColor?.Invoke()??preloadAlerConfigLine.Color ?? Settings.DefaultTextColor, FontDrawFlags.Right);
                    maxWidth = Math.Max(size.Width, maxWidth);
                    position.Y += size.Height;
                }
                if (maxWidth > 0)
                {
                    var bounds = new RectangleF(startPosition.X - maxWidth - 5, startPosition.Y - 5,
                        maxWidth + 10, position.Y - startPosition.Y + 10);
                    Graphics.DrawBox(bounds, Settings.BackgroundColor);
                    Size = bounds.Size;
                    Margin = new Vector2(0, 5);
                }
            }
        }

        private int GetNumberOfObjects()
        {
            Memory memory = GameController.Memory;
            return memory.ReadInt(memory.AddressOfProcess + memory.offsets.FileRoot, 12);
        }

        private void OnAreaChange(AreaController area)
        {
            maxParseTime = area.CurrentArea.TimeEntered.AddSeconds(10);
            areaChanged = true;
        }

        private void Parse()
        {
            areaChanged = false;
            alerts.Clear();
            Memory memory = GameController.Memory;
            int pFileRoot = memory.ReadInt(memory.AddressOfProcess + memory.offsets.FileRoot);
            int count = memory.ReadInt(pFileRoot + 12);
            int listIterator = memory.ReadInt(pFileRoot + 20);
            int areaChangeCount = GameController.Game.AreaChangeCount;
            for (int i = 0; i < count; i++)
            {
                listIterator = memory.ReadInt(listIterator);
                if (memory.ReadInt(listIterator + 8) != 0 && memory.ReadInt(listIterator + 12, 36) == areaChangeCount)
                {
                    string text = memory.ReadStringU(memory.ReadInt(listIterator + 8));
                    if (text.Contains('@'))
                    {
                        text = text.Split('@')[0];
                    }
                    //Attempt to fix preload not catching corrupted areas by making it also catch preloaded effects and the sound associated with the corrupted area (NoRain would imply there's a rain version but I couldn't find it so perhaps they overlay them one over the other).
                    if (text.Contains("human_heart") || text.Contains("Demonic_NoRain.ogg"))
                    {
                        alerts.Add(new PreloadAlerConfigLine { Text = "Area contains Corrupted Area", FastColor = () => Settings.CorruptedAreaColor });
                    }
                    else if (alertStrings.ContainsKey(text))
                    {
                        Console.WriteLine("Alert because of " + text);
                        alerts.Add(alertStrings[text]);
                    }
                    else if (text.EndsWith("BossInvasion"))
                    {
                        alerts.Add(new PreloadAlerConfigLine { Text = "Area contains Invasion Boss" });
                    }
                }
            }
        }
    }
}
