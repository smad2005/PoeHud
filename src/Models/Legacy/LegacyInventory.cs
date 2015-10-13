using System;
using System.Collections.Generic;
using PoeHUD.Controllers;

namespace PoeHUD.Models.Legacy
{
    [Obsolete]
	public class LegacyInventory
	{
		private readonly Poe.RemoteMemoryObjects.Inventory InternalInventory;
		private readonly GameController Poe;
		public int Width
		{
			get
			{
				return this.InternalInventory.Width;
			}
		}
		public int Height
		{
			get
			{
				return this.InternalInventory.Height;
			}
		}
		public List<EntityWrapper> Items
		{
			get
			{
				List<EntityWrapper> list = new List<EntityWrapper>();
				foreach (Poe.Entity current in this.InternalInventory.Items)
				{
					list.Add(new EntityWrapper(this.Poe, current));
				}
				return list;
			}
		}
		public LegacyInventory(GameController poe, Poe.RemoteMemoryObjects.Inventory internalInventory)
		{
			Poe = poe;
			InternalInventory = internalInventory;
		}
		public LegacyInventory(GameController poe, int address) : this(poe, poe.Game.GetObject<Poe.RemoteMemoryObjects.Inventory>(address))
		{
		}
	}
}
