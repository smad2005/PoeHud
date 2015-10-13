using System;
using PoeHUD.Models.Enums;

namespace PoeHUD.Models.Legacy
{
	[Obsolete]
    public class LegacyLink
	{
		public static readonly LegacyLink EmptyLegacyLink = new LegacyLink(new LegacySocket[0]);
		private readonly LegacySocket[] link;
		public int Length
		{
			get
			{
				return this.link.Length;
			}
		}
		public int NumberOfRed
		{
			get;
			private set;
		}
		public int NumberOfGreen
		{
			get;
			private set;
		}
		public int NumberOfBlue
		{
			get;
			private set;
		}
		public LegacyLink(LegacySocket[] legacySockets)
		{
			this.link = legacySockets;
			this.CountColors();
		}
		public LegacyLink(string sockets)
		{
			this.link = new LegacySocket[sockets.Length];
			for (int i = 0; i < sockets.Length; i++)
			{
				this.link[i] = this.CharToSocket(sockets.ToCharArray()[i]);
			}
			this.CountColors();
		}
		private void CountColors()
		{
			LegacySocket[] array = this.link;
			for (int i = 0; i < array.Length; i++)
			{
				switch (array[i])
				{
				case LegacySocket.Red:
					this.NumberOfRed++;
					break;
				case LegacySocket.Green:
					this.NumberOfGreen++;
					break;
				case LegacySocket.Blue:
					this.NumberOfBlue++;
					break;
				}
			}
		}
		private LegacySocket CharToSocket(char s)
		{
			char c = char.ToUpper(s);
			if (c == 'B')
			{
				return LegacySocket.Blue;
			}
			if (c == 'G')
			{
				return LegacySocket.Green;
			}
			if (c == 'R')
			{
				return LegacySocket.Red;
			}
			throw new Exception("Invalid socket char: " + s);
		}
		public bool Contains(LegacyLink other)
		{
			return other.NumberOfRed <= this.NumberOfRed && other.NumberOfGreen <= this.NumberOfGreen && other.NumberOfBlue <= this.NumberOfBlue;
		}
	}
}
