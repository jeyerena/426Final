using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipServer
{
	public class GameBoard
	{
		private int[,] board;
		private GameConfig config;
		public int numFilled;
		public int totalNum;

		private GameBoard(int xSize, int ySize)
		{
			board = new int[ySize, xSize];
		}

		public int this[int y, int x]
		{
			get { return board[y, x]; }
		}

		public bool Equals(GameBoard other)
		{
			return this.config.Equals(other.config);
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
			temp.config = config;
			temp.totalNum = config.xSize * config.ySize;
			board = temp;
			return true;
		}
	}
}
