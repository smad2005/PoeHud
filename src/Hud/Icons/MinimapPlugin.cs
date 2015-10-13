using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using PoeHUD.Controllers;
using PoeHUD.Framework.Helpers;
using PoeHUD.Hud.UI;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.UI;

using SharpDX;

namespace PoeHUD.Hud.Icons
{
    public class MinimapPlugin : Plugin<MapIconsSettings>
    {
        private readonly Func<IEnumerable<MapIcon>> getIcons;

        public MinimapPlugin(GameController gameController, Graphics graphics, Func<IEnumerable<MapIcon>> gatherMapIcons,
            MapIconsSettings settings)
            : base(gameController, graphics, settings)
        {
            Contract.Requires(gameController != null);
            getIcons = gatherMapIcons;
        }

        public override void Render()
        {
            if (!Settings.Enable || !GameController.InGame || !Settings.IconsOnMinimap)
            {
                return;
            }

            Element smallMinimap = GameController.Game.IngameState.IngameUi.Map.SmallMinimap;
            if (!smallMinimap.IsVisible)
            {
                return;
            }

            Vector2 playerPos = GameController.Player.GetComponent<Positioned>().GridPos;
            float posZ = GameController.Player.GetComponent<Render>().Z;

            const float SCALE = 240f;
            RectangleF mapRect = smallMinimap.GetClientRect();
            var mapCenter = new Vector2(mapRect.X + mapRect.Width/2, mapRect.Y + mapRect.Height/2).Translate(0,-20);
            double diag = Math.Sqrt(mapRect.Width * mapRect.Width + mapRect.Height * mapRect.Height) / 2.0;
            foreach (MapIcon icon in getIcons().Where(x => x.IsVisible()))
            {
                float iconZ = icon.EntityWrapper.GetComponent<Render>().Z;
                Vector2 point = mapCenter
                    + MapIcon.DeltaInWorldToMinimapDelta(icon.WorldPosition - playerPos, diag, SCALE, (iconZ - posZ) / 20);

                HudTexture texture = icon.MinimapIcon;
                int size = icon.Size;
                var rect = new RectangleF(point.X - size / 2f, point.Y - size / 2f, size, size);
                bool isContain;
                mapRect.Contains(ref rect, out isContain);
                if (isContain)
                {
                    texture.Draw(Graphics, rect);
                }
            }
        }
    }
}