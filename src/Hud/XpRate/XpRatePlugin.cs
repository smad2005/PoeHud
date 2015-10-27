using System;
using System.Linq;
using PoeHUD.Controllers;
using PoeHUD.Framework.Helpers;
using PoeHUD.Hud.UI;
using PoeHUD.Models;
using PoeHUD.Poe.Components;

using SharpDX;
using SharpDX.Direct3D9;

namespace PoeHUD.Hud.XpRate
{
    public class XpRatePlugin : SizedPlugin<XpRateSettings>
    {
        private string xpRate, timeLeft;

        private DateTime startTime, lastTime;

        private long startXp;

        private double levelXpPenalty, partyXpPenalty;


        public XpRatePlugin(GameController gameController, Graphics graphics, XpRateSettings settings)
            : base(gameController, graphics, settings)
        {
            AreaChange();
            GameController.Area.OnAreaChange += area => AreaChange();
        }

        public override void Render()
        {
            base.Render();
            if (!Settings.Enable || (GameController.Player != null && GameController.Player.GetComponent<Player>().Level >= 100))
            {
                return;
            }

            DateTime nowTime = DateTime.Now;
            TimeSpan elapsedTime = nowTime - lastTime;
            if (elapsedTime.TotalSeconds > 1)
            {
                CalculateXp(nowTime);
                if (Settings.PartyPenalty)
                {
                    partyXpPenalty = PartyXpPenalty();
                }
                lastTime = nowTime;
            }

            Vector2 position = StartDrawPointFunc();
            int fontSize = Settings.TextSize;

            Size2 xpRateSize = Graphics.DrawText(xpRate, fontSize, position, FontDrawFlags.Right);
            Vector2 secondLine = position.Translate(0, xpRateSize.Height);
            Size2 xpLeftSize = Graphics.DrawText(timeLeft, fontSize, secondLine, FontDrawFlags.Right);
            Vector2 thirdLine = secondLine.Translate(0, xpLeftSize.Height);
            var currentArea = GameController.Area.CurrentArea;
            var xpReceiving = (Settings.LevelPenalty ? levelXpPenalty : 1.0) * (Settings.PartyPenalty ? partyXpPenalty : 1.0);
            var xpReceivingText = Settings.ShowXpReceiving ? $" & {xpReceiving:p1}" : string.Empty;
            string areaName = $"{currentArea.Name} ({currentArea.RealLevel}Lvl{xpReceivingText})";
            Size2 areaNameSize = Graphics.DrawText(areaName, fontSize, thirdLine, FontDrawFlags.Right);

            string timer = AreaInstance.GetTimeString(nowTime - currentArea.TimeEntered);
            Size2 timerSize = Graphics.MeasureText(timer, fontSize);

            float boxWidth = MathHepler.Max(xpRateSize.Width, xpLeftSize.Width, areaNameSize.Width + timerSize.Width + 20) + 15;
            float boxHeight = xpRateSize.Height + xpLeftSize.Height + areaNameSize.Height;
            var bounds = new RectangleF(position.X - boxWidth + 5, position.Y - 5, boxWidth, boxHeight + 10);

            string systemTime = string.Format("{0} ({1})", nowTime.ToShortTimeString(), GameController.Game.IngameState.CurFps);
            Size2 timeFpsSize = Graphics.MeasureText(systemTime, fontSize);
            var dif =bounds.Width - (12 + timeFpsSize.Width + xpRateSize.Width);
            if (dif < 0)
            {
                bounds.X += dif;
                bounds.Width -= dif;
            }

            Graphics.DrawText(systemTime, fontSize, new Vector2(bounds.X + 5, position.Y), Color.White);
            Graphics.DrawText(timer, fontSize, new Vector2(bounds.X + 5, thirdLine.Y), Color.White);

            Graphics.DrawBox(bounds, Settings.BackgroundColor);
            Size = bounds.Size;
            Margin = new Vector2(0, 5);
        }

        private void CalculateXp(DateTime nowTime)
        {
            long currentXp = GameController.Player.GetComponent<Player>().XP;
            double rate = (currentXp - startXp) / (nowTime - startTime).TotalHours;
            xpRate = string.Format("{0} XP/h", ConvertHelper.ToShorten(rate, "0.00"));
            int level = GameController.Player.GetComponent<Player>().Level;
            if (level >= 0 && level + 1 < Constants.PlayerXpLevels.Length && rate > 1)
            {
                long xpLeft = Constants.PlayerXpLevels[level + 1] - currentXp;
                TimeSpan time = TimeSpan.FromHours(xpLeft / rate);
                timeLeft = string.Format("{0}h {1}M {2}s until level up", time.Hours, time.Minutes, time.Seconds);
            }
        }

        private double LevelXpPenalty()
        {
            // based on xp penalty formula from http://pathofexile.gamepedia.com/Experience
            int arenaLevel = GameController.Area.CurrentArea.RealLevel;
            int characterLevel = GameController.Player.GetComponent<Player>().Level;
            double safeZone = Math.Floor(Convert.ToDouble(characterLevel) / 16) + 3;
            double effectiveDifference = Math.Max(Math.Abs(characterLevel - arenaLevel) - safeZone, 0);
            double xpMultiplier = Math.Max(Math.Pow((characterLevel + 5) / (characterLevel + 5 + Math.Pow(effectiveDifference, 2.5)), 1.5), 0.01);
            return xpMultiplier;
        }
        private double PartyXpPenalty()
        {
            var levels = GameController.Entities.Where(y => y.HasComponent<Player>()).Select(y => y.GetComponent<Player>().Level).ToList();
            // based on xp penalty formula from http://pathofexile.gamepedia.com/Experience
            int characterLevel = GameController.Player.GetComponent<Player>().Level;
            double partyXpPenalty = Math.Pow(characterLevel + 10, 2.71) / levels.Sum(level => Math.Pow(level + 10, 2.71));
            return partyXpPenalty * levels.Count;
        }

        private void AreaChange()
        {
            if (GameController.InGame)
            {
                startXp = GameController.Player.GetComponent<Player>().XP;
                levelXpPenalty = LevelXpPenalty();
            }

            startTime = lastTime = DateTime.Now;
            xpRate = "0.00 XP/h";
            timeLeft = "--h --m --s until level up";
        }
    }
}