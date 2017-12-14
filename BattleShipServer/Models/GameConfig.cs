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
	}

	public class ShipConfig
	{
		public int[] lengths { get; set; }

		public ShipConfig(GameConfig config)
		{
			for (int i = 0; i < config.ships.Length; i++)
			{
				lengths[i] = config.ships[i].length;
			}
		}
	}
}