using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipServer.Models
{
    public class GameInstance
    {
		private GameBoard[] gameBoards;
		private int numPlayers;

		public GameInstance()
		{
			gameBoards = new GameBoard[2];
			numPlayers = 0;
		}

		public bool addPlayer(GameBoard board) //adds player, returns true on success and false on failure
		{
			if (numPlayers == 0)
			{
				gameBoards[numPlayers++] = board;
				return true;
			}
			else if (numPlayers == 1)
			{
				if (gameBoards[0].Equals(board))
				{
					gameBoards[numPlayers++] = board;
					return true;
				}
			}
			return false;
		}
	}
}
