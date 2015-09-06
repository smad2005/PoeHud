﻿using System;
using System.Collections.Generic;

using PoeHUD.Framework.Helpers;

using SharpDX;
using SharpDX.Direct3D9;

namespace PoeHUD.Hud.UI.Renderers
{
    public sealed class FontRenderer : IDisposable
    {
        private readonly Device device;

        private readonly Sprite sprite;

        private readonly Dictionary<Tuple<string, int>, Font> fonts;

        public FontRenderer(Device device)
        {
            this.device = device;
            sprite = new Sprite(device);
            fonts = new Dictionary<Tuple<string, int>, Font>();
        }

        public void Begin()
        {
            sprite.Begin(SpriteFlags.AlphaBlend | SpriteFlags.SortTexture);
        }

        public Size2 DrawText(string text, string fontName, int height, Vector2 position, Color color, FontDrawFlags align)
        {
            Font font;
            Rectangle fontDimension;
            try
            {
                font = GetFont(fontName, height);
                var rectangle = new Rectangle((int)position.X, (int)position.Y, 0, 0);
                fontDimension = font.MeasureText(null, text, rectangle, align);
                if (!sprite.IsDisposed)
                    font.DrawText(sprite, text, fontDimension, align, color);
                return new Size2(fontDimension.Width, fontDimension.Height);
            }
            catch (Exception)
            {
                //Console.WriteLine("Exception! X: " + position.X + ", Y: " + position.Y + ", Text: " + text);
                //Console.WriteLine(exception.StackTrace);
            }
            return new Size2();
        }

        public void End()
        {
            sprite.End();
        }

        public void Flush()
        {
            fonts.ForEach((key, font) => font.Dispose());
            fonts.Clear();
        }

        public Size2 MeasureText(string text, string fontName, int height, FontDrawFlags align)
        {
            Font font = GetFont(fontName, height);
            Rectangle fontDimension = font.MeasureText(null, text, align);
            return new Size2(fontDimension.Width, fontDimension.Height);
        }

        public void Dispose()
        {
            sprite.Dispose();
            Flush();
        }

        private Font GetFont(string name, int height)
        {
            lock (fonts)
            {
                Font font;
                Tuple<string, int> key = Tuple.Create(name, height);
                if (!fonts.TryGetValue(key, out font))
                {
                    font = new Font(device, new FontDescription
                    {
                        FaceName = name,
                        Quality = FontQuality.ClearType,
                        Height = height
                    });
                    fonts.Add(key, font);
                }
                return font;
            }
        }
    }
}