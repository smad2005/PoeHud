using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PoeHUD.Hud.Settings;
using PoeHUD.Hud.UI;

using SharpDX;
using SharpDX.Direct3D9;

using ColorGdi = System.Drawing.Color;

namespace PoeHUD.Hud.Menu
{
    public sealed class FileButton : MenuItem
    {
        private FileNode path;

        public FileButton( FileNode path)
        {
            this.path = path;
        }

        public override int DesiredWidth
        {
            get { return 210; }
        }

        public override int DesiredHeight
        {
            get { return 25; }
        }

        public override void Render(Graphics graphics)
        {
            if (!IsVisible)
            {
                return;
            }

            string text = string.IsNullOrEmpty(path) ? "   Select the file" : Path.GetFileName(path);
            graphics.DrawBox(Bounds, Color.Black);
            graphics.DrawBox(new RectangleF(Bounds.X + 1, Bounds.Y + 1, Bounds.Width - 2, Bounds.Height - 2), Color.Gray);
            float size = DesiredHeight - 2; 
            var textPosition = new Vector2(Bounds.X + Bounds.Width / 2 - size / 2, Bounds.Y + Bounds.Height / 2);
            graphics.DrawText(text, 20, textPosition, Color.White, FontDrawFlags.VerticalCenter | FontDrawFlags.Center);

            var rectangle = new RectangleF(Bounds.Left+ 5, Bounds.Top + 1, size + 5, size);
            graphics.DrawImage("openFile.png", rectangle);

        }

        protected override void HandleEvent(MouseEventID id, Vector2 pos)
        {
            if (id == MouseEventID.LeftButtonDown)
            {
                Thread thread = new Thread(() =>
                {
                    var filedialog = new OpenFileDialog();
                    filedialog.Filter = "filter files (*.filter)|*.filter|All files (*.*)|*.*";
                    if (filedialog.ShowDialog() == DialogResult.OK)
                    {
                        path.Value = filedialog.FileName;
                    }
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }
    }
}