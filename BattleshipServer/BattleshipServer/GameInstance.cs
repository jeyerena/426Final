using System;
using BattleshipServer;

namespace BattleShipServer
{
	public class GameInstance
	{
		private int[][] gameBoards;
		private int[] tilesLeft;
		private int[] tiles;
		private int xSize, ySize;
		private int num_players;

		public GameInstance(int xSize, int ySize)
		{
			gameBoards = new int[2][];
			tilesLeft = new int[2];
			this.xSize = xSize;
			this.ySize = ySize;
			num_players = 0;
			int max_size = (xSize < ySize) ? ySize : xSize;
			tiles = new int[max_size];
			for (int i = 0; i < max_size; i++)
				tiles[i] = 0;
		}

		private bool constructBoard(Ship[] ships, int xSize, int ySize, out int[] board, out int numTiles)
		{
			int[] o = new int[xSize * ySize];
			int num = 0;
			for (int i = 0; i < xSize * ySize - 1; i++) o[i] = 0;
			for (int i = 0; i < ships.Length; i++)
			{
				int x = ships[i].x;
				int y = ships[i].y;
				if (ships[i].isVertical)
				{
					if (x > 0 && x < xSize && y > 0 && y + ships[i].length < ySize)
					{
						num += ships[i].length;
						for (int j = 0; j < ships[i].length; j++)
							o[(y + j) * ySize + x] = 1;
					}
					else
					{
						board = o;
						numTiles = num;
						return false;
					}
				}
				else
				{
					if (x > 0 && x + ships[i].length < xSize && y > 0 && y < ySize)
					{
						num += ships[i].length;
						for (int j = 0; j < ships[i].length; j++)
							o[y * ySize + x + j] = 1;
					}
					else
					{
						board = o;
						numTiles = num;
						return false;
					}
				}
			}
			board = o;
			numTiles = num;
			return true;
		}

		public bool addPlayer(Ship[] ships, int xSize, int ySize)
		{
			if (xSize != this.xSize || ySize != this.ySize)
				return false;
			if (num_players == 0)
			{
				for (int i = 0; i < ships.Length; i++)
				{
					if (ships[i].length > tiles.Length)
						return false;
					tiles[ships[i].length]++;
				}
				return constructBoard(ships, xSize, ySize, out gameBoards[0], out tilesLeft[0]);
			}
			else if (num_players == 1)
			{
				for (int i = 0; i < ships.Length; i++)
				{
					if (ships[i].length > tiles.Length || tiles[ships[i].length] == 0)
						return false;
					tiles[ships[i].length]--;
				}
				for (int i = 0; i < tiles.Length; i++)
				{
					if (tiles[i] != 0)
						return false;
				}
				return constructBoard(ships, xSize, ySize, out gameBoards[1], out tilesLeft[1]);
			}
			else
				return false;
		}

		public bool hit(int player, int x, int y, out bool goAgain)
		{
			if (player > 1 || x >= xSize || y >= ySize)
			{
				goAgain = true;
				return false;
			}
			if (gameBoards[player][y*ySize+x] == 0)
			{
				gameBoards[player][y*ySize+x] = 2;
				goAgain = false;
				return false;
			}
			else if (gameBoards[player][y*ySize+x] == 1)
			{
				gameBoards[player][y*ySize+x] = 2;
				goAgain = true;
				return true;
			}
			else
			{
				goAgain = true;
				return false;
			}
		}
	}
}
