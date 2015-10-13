using System;
using System.Linq;
using PoeHUD.Framework.Helpers;
using PoeHUD.Hud.Settings;
using PoeHUD.Hud.UI;

using SharpDX;
using SharpDX.Direct3D9;

namespace PoeHUD.Hud.Menu
{
    public class ToggleButton : MenuItem
    {
        public readonly string Name;

        private readonly ToggleNode node;

        private readonly string key;

        private readonly MenuItem parent;

        private readonly Func<MenuItem, bool> hide;

        public ToggleButton(MenuItem parent,string name, ToggleNode node, string key, Func<MenuItem,bool> hide)
        {
            this.Name = name;
            this.node = node;
            this.key = key;
            this.parent = parent;
            this.hide = hide;
            if (hide != null)
            {
                node.OnValueChanged = Hide;
            }
        }

        private void Hide()
        {
            parent.Children.Where(hide).ForEach(y => y.SetVisible(!node.Value));
        }

        public override int DesiredHeight
        {
            get { return 25; }
        }

        public override int DesiredWidth
        {
            get { return 210; }
        }

        public override void Render(Graphics graphics)
        {
            if (!IsVisible)
            {
                return;
            }
            Color color = node.Value ? Color.Gray : Color.Crimson;
            var textPosition = new Vector2(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2);
            if (key != null)
                graphics.DrawText(string.Concat("[",key,"]"), 11, Bounds.TopLeft.Translate(2, 2), Color.White);
            // TODO textSize to Settings
            graphics.DrawText(Name, 20, textPosition, Color.White, FontDrawFlags.VerticalCenter | FontDrawFlags.Center);
            graphics.DrawBox(Bounds, Color.Black);
            graphics.DrawBox(new RectangleF(Bounds.X + 1, Bounds.Y + 1, Bounds.Width - 2, Bounds.Height - 2), color);
            if (Children.Count > 0)
            {
                float width = (Bounds.Width - 2) * 0.05f;
                float height = (Bounds.Height - 2) / 2;
                var imgRect = new RectangleF(Bounds.X + Bounds.Width - 3 - width, Bounds.Y + 1 + height - height / 2, width, height);
                graphics.DrawImage("menu_submenu.png", imgRect);
            }
            Children.ForEach(x => x.Render(graphics));
        }

        protected override void HandleEvent(MouseEventID id, Vector2 pos)
        {
            if (id == MouseEventID.LeftButtonDown)
            {
                node.Value = !node.Value;
            }
        }

        public override void SetHovered(bool hover)
        {
            Action func = null;
            Children.ForEach(x =>
            {
                x.SetVisible(hover);
                var toggleButton = (x as ToggleButton);
                if (hover && toggleButton?.hide != null)
                {
                    func = toggleButton.Hide;
                }
            });
            func?.Invoke();
        }
    }
}