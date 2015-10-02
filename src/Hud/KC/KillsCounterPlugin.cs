﻿using System;
using System.Collections.Generic;
using System.Linq;

using PoeHUD.Controllers;
using PoeHUD.Framework.Helpers;
using PoeHUD.Hud.UI;
using PoeHUD.Models;
using PoeHUD.Models.Enums;
using PoeHUD.Poe.Components;

using SharpDX;
using SharpDX.Direct3D9;

namespace PoeHUD.Hud.KC
{
    public class KillsCounterPlugin : SizedPlugin<KillCounterSettings>
    {
        private readonly HashSet<EntityWrapper> aliveEntities;

        private readonly HashSet<long> countedIds;

        private readonly Dictionary<MonsterRarity, int> counters;

        private int summaryCounter;

        public KillsCounterPlugin(GameController gameController, Graphics graphics, KillCounterSettings settings)
            : base(gameController, graphics, settings)
        {
            aliveEntities = new HashSet<EntityWrapper>();
            countedIds = new HashSet<long>();
            counters = new Dictionary<MonsterRarity, int>();
            Init();
            GameController.Area.OnAreaChange += area =>
            {
                if (!Settings.Enable)
                {
                    return;
                }
                aliveEntities.Clear();
                countedIds.Clear();
                counters.Clear();
                summaryCounter = 0;
                Init();
            };
        }

        public override void Render()
        {
            base.Render();
            if (!Settings.Enable)
            {
                return;
            }

            List<EntityWrapper> deadEntities = aliveEntities.Where(entity => !entity.IsAlive).ToList();
            foreach (EntityWrapper entity in deadEntities)
            {
                Calc(entity);
                aliveEntities.Remove(entity);
            }

            Vector2 position = StartDrawPointFunc();
            var size = new Size2();
            if (Settings.ShowDetail)
            {
                size = DrawCounters(position);
            }
            Size2 size2 = Graphics.DrawText($"Kills {summaryCounter}", Settings.FontSize, position.Translate(-size.Width / 2f, size.Height), Settings.FontColor, Settings.ShowDetail ? FontDrawFlags.Center : FontDrawFlags.Right);
            int width = Math.Max(size.Width, size2.Width);
            var bounds = new RectangleF(position.X - width - 5, position.Y - 5, width + 15, size.Height + size2.Height + 10);
            Graphics.DrawImage("preload-end.png", bounds, Settings.BackgroundColor);
            Graphics.DrawImage("preload-start.png", bounds, Settings.BackgroundColor);
            //Graphics.DrawBox(bounds, new ColorBGRA(0, 0, 0, 180));
            Size = bounds.Size;
            Margin = new Vector2(5, 0);
        }

        protected override void OnEntityAdded(EntityWrapper entityWrapper)
        {
            if (!Settings.Enable)
            {
                return;
            }
            if (entityWrapper.HasComponent<Monster>())
            {
                if (entityWrapper.IsAlive)
                {
                    aliveEntities.Add(entityWrapper);
                }
                else
                {
                    Calc(entityWrapper);
                }
            }
        }

        protected override void OnEntityRemoved(EntityWrapper entityWrapper)
        {
            if (aliveEntities.Contains(entityWrapper))
            {
                aliveEntities.Remove(entityWrapper);
            }
        }

        private void Calc(EntityWrapper entityWrapper)
        {
            if (!countedIds.Contains(entityWrapper.LongId))
            {
                countedIds.Add(entityWrapper.LongId);
                MonsterRarity rarity = entityWrapper.GetComponent<ObjectMagicProperties>().Rarity;
                if (entityWrapper.IsHostile && counters.ContainsKey(rarity))
                {
                    counters[rarity]++;
                    summaryCounter++;
                }
            }
        }

        private Size2 DrawCounter(Vector2 position, string label, string counterValue, Color color)
        {
            Size2 measuredSize1 = Graphics.MeasureText(counterValue, 20, FontDrawFlags.Right);
            Size2 measuredSize2 = Graphics.MeasureText(label, 11, FontDrawFlags.Right);
            if (measuredSize1.Width > measuredSize2.Width)
            {
                Size2 size = Graphics.DrawText(counterValue, 20, position, color, FontDrawFlags.Right);
                Size2 size2 = Graphics.DrawText(label, 11, position.Translate(-size.Width / 2f, size.Height), Color.White,
                    FontDrawFlags.Center);
                return new Size2(size.Width, size.Height + size2.Height);
            }
            else
            {
                Size2 size2 = Graphics.DrawText(label, 11, position.Translate(0, measuredSize1.Height), Settings.FontColor,
                    FontDrawFlags.Right);
                Size2 size = Graphics.DrawText(counterValue, 20, position.Translate(-size2.Width / 2f, 0), color,
                    FontDrawFlags.Center);
                return new Size2(size2.Width, size.Height + size2.Height);
            }
        }

        private Size2 DrawCounters(Vector2 position)
        {
            const int INNER_MARGIN = 7;
            Size2 size = DrawCounter(position, "", counters[MonsterRarity.White].ToString(), Settings.FontColor);
            size = new Size2( DrawCounter(position.Translate(-size.Width - INNER_MARGIN, 0), "",
                        counters[MonsterRarity.Magic].ToString(), HudSkin.MagicColor).Width + size.Width + INNER_MARGIN,
                    size.Height);
            size = new Size2( DrawCounter(position.Translate(-size.Width - INNER_MARGIN, 0), "", counters[MonsterRarity.Rare].ToString(),
                        HudSkin.RareColor).Width + size.Width + INNER_MARGIN, size.Height);
            size =new Size2( DrawCounter(position.Translate(-size.Width - INNER_MARGIN, 0), "",
                        counters[MonsterRarity.Unique].ToString(), HudSkin.UniqueColor).Width + size.Width + INNER_MARGIN,
                    size.Height);
            return size;
        }

        private void Init()
        {
            foreach (MonsterRarity rarity in Enum.GetValues(typeof(MonsterRarity)))
            {
                counters[rarity] = 0;
            }
        }
    }
}