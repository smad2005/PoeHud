using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using PoeHUD.ExileBot;
using PoeHUD.Framework;
using PoeHUD.Hud.Icons;
using PoeHUD.Poe.EntityComponents;
using PoeHUD.Poe.UI;
using SlimDX.Direct3D9;
using Entity = PoeHUD.Poe.Entity;

namespace PoeHUD.Hud.Loot
{
	public class ItemAlerter : HUDPlugin
	{
		public struct CraftingBase
		{
			public string Name;
			public int MinItemLevel;
			public int MinQuality;
		}
		private HashSet<long> playedSoundsCache;
		private Dictionary<ExileBot.Entity, AlertDrawStyle> currentAlerts;
		private Dictionary<string, ItemAlerter.CraftingBase> craftingBases;
		private HashSet<string> currencyNames;
		public override void OnEnable()
		{
			this.playedSoundsCache = new HashSet<long>();
			this.currentAlerts = new Dictionary<ExileBot.Entity, AlertDrawStyle>();
			this.currencyNames = this.LoadCurrency();
			this.craftingBases = this.LoadCraftingBases();
			this.poe.Area.OnAreaChange += this.CurrentArea_OnAreaChange;
			this.poe.EntityList.OnEntityAdded += this.EntityList_OnEntityAdded;
			this.poe.EntityList.OnEntityRemoved += this.EntityList_OnEntityRemoved;
		}
		public override void OnDisable()
		{
		}
		private void EntityList_OnEntityRemoved(ExileBot.Entity entity)
		{
			if (this.currentAlerts.ContainsKey(entity))
			{
				this.currentAlerts.Remove(entity);
			}
		}
		private void EntityList_OnEntityAdded(ExileBot.Entity entity)
		{
			if (!Settings.GetBool("ItemAlert") || this.currentAlerts.ContainsKey(entity))
			{
				return;
			}
			if (entity.HasComponent<WorldItem>())
			{
				ExileBot.Entity item = new ExileBot.Entity(this.poe, entity.GetComponent<WorldItem>().ItemEntity);
				ItemUsefulProperties props = this.EvaluateItem(item);

				if (props.IsWorthAlertingPlayer(currencyNames))
				{
					this.DoAlert(entity, props);
				}
			}
		}


		private ItemUsefulProperties EvaluateItem(ExileBot.Entity item)
		{
			ItemUsefulProperties ip = new ItemUsefulProperties();

			Mods mods = item.GetComponent<Mods>();
			Sockets socks = item.GetComponent<Sockets>();
			Map map = item.HasComponent<Map>() ? item.GetComponent<Map>() : null;
			SkillGem sk = item.HasComponent<SkillGem>() ? item.GetComponent<SkillGem>() : null;
			Quality q = item.HasComponent<Quality>() ? item.GetComponent<Quality>() : null;

			ip.Name = this.poe.Files.BaseItemTypes.Translate(item.Path);
			ip.ItemLevel = mods.ItemLevel;
			ip.NumLinks = socks.LargestLinkSize;
			ip.NumSockets = socks.NumberOfSockets;
			ip.Rarity = mods.ItemRarity;
			ip.MapLevel = map == null ? 0 : 1;
			ip.IsCurrency = item.Path.Contains("Currency");
			ip.IsSkillGem = sk != null;
			ip.Quality = q == null ? 0 : q.ItemQuality;
			ip.WorthChrome = socks != null && socks.IsRGB;

			CraftingBase craftingBase;
			if (craftingBases.TryGetValue(ip.Name, out craftingBase) && Settings.GetBool("ItemAlert.Crafting"))
				ip.IsCraftingBase = ip.ItemLevel >= craftingBase.MinItemLevel && ip.Quality >= craftingBase.MinQuality;

			return ip;
		}

		private void DoAlert(ExileBot.Entity entity, ItemUsefulProperties ip)
		{
			AlertDrawStyle drawStyle = ip.GetDrawStyle();
			this.currentAlerts.Add(entity, drawStyle);
			this.overlay.MinimapRenderer.AddIcon(new ItemMinimapIcon(entity, "minimap_default_icon.png", drawStyle.color, 8));
			if (Settings.GetBool("ItemAlert.PlaySound") && !this.playedSoundsCache.Contains(entity.LongId))
			{
				this.playedSoundsCache.Add(entity.LongId);
				Sounds.AlertSound.Play();
			}
		}
		private void CurrentArea_OnAreaChange(AreaController area)
		{
			this.playedSoundsCache.Clear();
		}
		public override void Render(RenderingContext rc)
		{
			if (!Settings.GetBool("ItemAlert") || !Settings.GetBool("ItemAlert.ShowText"))
			{
				return;
			}
			Rect clientRect = this.poe.Internal.game.IngameState.IngameUi.Minimap.SmallMinimap.GetClientRect();
			Vec2 vec = new Vec2(clientRect.X + clientRect.W - Settings.GetInt("ItemAlert.PositionWidth"), clientRect.Y + clientRect.H + Settings.GetInt("ItemAlert.PositionHeight"));
			
			int y = vec.Y;
			int fontSize = Settings.GetInt("ItemAlert.ShowText.FontSize");
			foreach (KeyValuePair<ExileBot.Entity, AlertDrawStyle> kv in this.currentAlerts)
			{
				if (kv.Key.IsValid)
				{
					EntityLabel labelFromEntity = this.poe.GetLabelFromEntity(kv.Key);
					string text;
					if (labelFromEntity == null)
					{
						Entity itemEntity = kv.Key.GetComponent<WorldItem>().ItemEntity;
						if (!itemEntity.IsValid)
						{
							continue;
						}
						text = kv.Value.Text;
					}
					else
					{
						text = labelFromEntity.Text;
					}

					var drawStyle = kv.Value;
					int frameWidth = drawStyle.FrameWidth;
					Vec2 vPadding = new Vec2(frameWidth + 5, frameWidth);
					int frameMargin = frameWidth + 2;

					Vec2 textPos = new Vec2(vec.X - vPadding.X, y + vPadding.Y);

					var vTextFrame = rc.AddTextWithHeightAndOutline(textPos, text, drawStyle.color, Color.Black, fontSize, DrawTextFormat.Right);
					if( frameWidth > 0)
					{
						rc.AddFrame(new Rect(vec.X - vTextFrame.X - 2 * vPadding.X, y, vTextFrame.X + 2 * vPadding.X, vTextFrame.Y + 2 * vPadding.Y), kv.Value.color, frameWidth);
					}

					if (drawStyle.IconIndex >= 0)
					{
						const float iconsInSprite = 4;
						int iconSize = vTextFrame.Y;
						Rect iconPos = new Rect(textPos.X - iconSize - vTextFrame.X, textPos.Y, iconSize, iconSize);
						RectUV uv = new RectUV(drawStyle.IconIndex / iconsInSprite, 0, (drawStyle.IconIndex + 1)/ iconsInSprite, 1);
						rc.AddSprite("item_icons.png", iconPos, uv);
					}
					y += vTextFrame.Y + 2 * vPadding.Y + frameMargin;
				}
			}
		}
		private Dictionary<string, ItemAlerter.CraftingBase> LoadCraftingBases()
		{
			if (!File.Exists("config/crafting_bases.txt"))
			{
				return new Dictionary<string, ItemAlerter.CraftingBase>();
			}
			Dictionary<string, ItemAlerter.CraftingBase> dictionary = new Dictionary<string, ItemAlerter.CraftingBase>(StringComparer.OrdinalIgnoreCase);
			string[] array = File.ReadAllLines("config/crafting_bases.txt");
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				try
				{
					string text2 = text.Trim();
					if (!string.IsNullOrWhiteSpace(text2) && !text2.StartsWith("#"))
					{
						string[] array2 = text2.Split(new char[]
						{
							','
						});
						if (array2.Length != 0 && array2.Length <= 3)
						{
							string text3 = array2[0].Trim().ToLowerInvariant();
							int minItemLevel = 0;
							if (array2.Length >= 2)
							{
								minItemLevel = int.Parse(array2[1].Trim());
							}
							int minQuality = 0;
							if (array2.Length >= 3)
							{
								minQuality = int.Parse(array2[2].Trim());
							}
							dictionary.Add(text3, new ItemAlerter.CraftingBase
							{
								Name = text3,
								MinItemLevel = minItemLevel,
								MinQuality = minQuality
							});
						}
					}
				}
				catch (Exception)
				{
					throw new Exception("Error parsing config/whites.txt at line " + text);
				}
			}
			return dictionary;
		}
		private HashSet<string> LoadCurrency()
		{
			if (!File.Exists("config/currency.txt"))
			{
				return null;
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			string[] array = File.ReadAllLines("config/currency.txt");
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				string text2 = text.Trim();
				if (!string.IsNullOrWhiteSpace(text2))
				{
					hashSet.Add(text2.ToLowerInvariant());
				}
			}
			return hashSet;
		}
	}
}