using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipServer.Models
{
    public class GameConfig
    {
		public readonly Ship[] ships;
		public readonly int xSize;
		public readonly int ySize;

		public GameConfig(Ship[] ships, int xSize, int ySize)
		{
			this.ships = ships;
			this.xSize = xSize;
			this.ySize = ySize;
			Array.Sort<Ship>(ships, Ship.CompareShipSize);
		}

		public bool Equals(GameConfig other)
		{
			if (this.xSize != other.xSize || this.ySize != other.ySize)
			{
				return false;
			}
			if (this.ships.Length != other.ships.Length)
			{
				return false;
			}
			for (int i = 0; i < this.ships.Length; i++)
			{
				if (this.ships[i].length != other.ships[i].length) return false;
			}
			return true;
		}
	}
}