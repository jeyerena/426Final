using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipServer.Models
{
    public class GameBoard
    {
		public int[,] board;
		public int numFilled;
		public int totalNum;

		private GameBoard(int xSize, int ySize)
		{
			board = new int[ySize, xSize];
		}

		[JsonConstructor]
		public GameBoard(int[,] board, int numFilled, int totalNum)
		{
			this.board = board;
			this.numFilled = numFilled;
			this.totalNum = totalNum;
		}

		public static bool ConstructBoard(GameConfig config, out GameBoard board)
		{
			GameBoard temp = new GameBoard(config.xSize, config.ySize);
			temp.numFilled = 0;
			for (int i = 0; i < config.ships.GetLength(0); i++)
			{
				if (!config.ships[i].fillShip(temp.board))
				{
					board = temp;
					return false;
				}
				else
				{
					temp.numFilled += config.ships[i].length;
				}
			}
			temp.totalNum = config.xSize * config.ySize;
			board = temp;
			return true;
		}
	}
}