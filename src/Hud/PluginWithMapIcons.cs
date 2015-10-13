﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PoeHUD.Controllers;
using PoeHUD.Hud.Interfaces;
using PoeHUD.Hud.Settings;
using PoeHUD.Hud.UI;
using PoeHUD.Models;

namespace PoeHUD.Hud
{
    public abstract class PluginWithMapIcons<TSettings> : Plugin<TSettings>, IPluginWithMapIcons where TSettings : SettingsBase
    {
        protected readonly Dictionary<EntityWrapper, MapIcon> CurrentIcons;

        protected PluginWithMapIcons(GameController gameController, Graphics graphics, TSettings settings): base(gameController, graphics, settings)
        {
            Contract.Requires(gameController != null);
            CurrentIcons = new Dictionary<EntityWrapper, MapIcon>();
            GameController.Area.OnAreaChange +=delegate 
            {
                CurrentIcons.Clear();
            };
        }

        protected override void OnEntityRemoved(EntityWrapper entityWrapper)
        {
            base.OnEntityRemoved(entityWrapper);
            CurrentIcons.Remove(entityWrapper);
        }

        public IEnumerable<MapIcon> GetIcons()
        {
            var toRemove = new List<EntityWrapper>();
            foreach (var kv in CurrentIcons)
            {
                if (kv.Value.IsEntityStillValid())
                    yield return kv.Value;
                else
                    toRemove.Add(kv.Key);
            }
            foreach (EntityWrapper wrapper in toRemove)
            {
                CurrentIcons.Remove(wrapper);
            }
        }
    }
}