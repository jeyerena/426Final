using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipServer
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
		}

		public static GameConfig MakeConfigFromJSON(string json)
		{
			GameConfig config = JsonConvert.DeserializeObject<GameConfig>(json);
			Array.Sort<Ship>(config.ships, Ship.CompareShipSize);
			return config;
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
