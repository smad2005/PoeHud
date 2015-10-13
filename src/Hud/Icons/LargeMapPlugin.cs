using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using PoeHUD.Controllers;
using PoeHUD.Framework.Helpers;
using PoeHUD.Hud.UI;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.RemoteMemoryObjects;

using SharpDX;

using Map = PoeHUD.Poe.UI.Elements.Map;

namespace PoeHUD.Hud.Icons
{
    public class LargeMapPlugin : Plugin<MapIconsSettings>
    {
        private readonly Func<IEnumerable<MapIcon>> getIcons;

        public LargeMapPlugin(GameController gameController, Graphics graphics, Func<IEnumerable<MapIcon>> gatherMapIcons,
            MapIconsSettings settings)
            : base(gameController, graphics, settings)
        {
            Contract.Requires(gameController != null);
            getIcons = gatherMapIcons;
        }

        public override void Render()
        {
            if (!Settings.Enable || !GameController.InGame || !Settings.IconsOnLargeMap
                || !GameController.Game.IngameState.IngameUi.Map.LargeMap.IsVisible)
            {
                return;
            }

            Camera camera = GameController.Game.IngameState.Camera;
            Map mapWindow = GameController.Game.IngameState.IngameUi.Map;
            RectangleF mapRect = mapWindow.GetClientRect();

            Vector2 playerPos = GameController.Player.GetComponent<Positioned>().GridPos;
            float posZ = GameController.Player.GetComponent<Render>().Z;
            Vector2 screenCenter = new Vector2(mapRect.Width / 2, mapRect.Height / 2).Translate(0,-20) + new Vector2(mapRect.X, mapRect.Y)
                + new Vector2(mapWindow.ShiftX, mapWindow.ShiftY);
            var diag = (float)Math.Sqrt(camera.Width * camera.Width + camera.Height * camera.Height);
            float k = camera.Width < 1024f ? 1120f : 1024f;
            float scale = k / camera.Height * camera.Width * 3 / 4;

            foreach (MapIcon icon in getIcons().Where(x => x.IsVisible()))
            {
                float iconZ = icon.EntityWrapper.GetComponent<Render>().Z;
                Vector2 point = screenCenter
                    + MapIcon.DeltaInWorldToMinimapDelta(icon.WorldPosition - playerPos, diag, scale, (iconZ - posZ) / 20);

                HudTexture texture = icon.LargeMapIcon ?? icon.MinimapIcon;
                int size = icon.SizeOfLargeIcon.GetValueOrDefault(icon.Size * 2);
                texture.Draw(Graphics, new RectangleF(point.X - size / 2f, point.Y - size / 2f, size, size));
            }
        }
    }
}