using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipServer.Models
{
    public class Ship
    {
		public readonly int x;
		public readonly int y;
		public readonly int length;
		public readonly bool isVertical;

		public Ship(int x, int y, int length, bool isVertical)
		{
			this.x = x;
			this.y = y;
			this.length = length;
			this.isVertical = isVertical;
		}

		public static int CompareShipSize(Ship ship1, Ship ship2)
		{
			if (ship1.length > ship2.length)
			{
				return 1;
			}
			else if (ship1.length < ship2.length)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}

		public bool fillShip(int[,] board)
		{
			int xSize = board.GetLength(1);
			int ySize = board.GetLength(0);
			if (isVertical)
			{
				if (x > 0 && x < xSize && y > 0)
				{
					for (int i = 0; i < length; i++)
					{
						if (y + i >= ySize) return false;
						board[y + i, x] = 1;
					}
				}
			}
			else
			{
				if (y > 0 && y < ySize && x > 0)
				{
					for (int i = 0; i < length; i++)
					{
						if (x + i >= xSize) return false;
						board[y, x + i] = 1;
					}
				}
			}
			return true;
		}
	}
}
