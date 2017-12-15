using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipServer.Models
{
    public class Match
    {
		public int matchId { get; set; }
		public bool isUser1 { get; set; }
		public bool isFull { get; set; }
		public int xSize { get; set; }
		public int ySize { get; set; }
		public DateTime timeStamp { get; set; }
		public string shipConfig { get; set; }
		public string gameState { get; set; }

		private GameState gameStateObj;

		public static Match MakeNewMatch(GameBoard player1Board, GameConfig config)
		{
			GameState game = GameState.MakeNewGame(player1Board);
			Match m = new Match()
			{
				matchId = -1,
				isUser1 = true,
				isFull = game.isFull,
				xSize = config.xSize,
				ySize = config.ySize,
				timeStamp = DateTime.Now,
				shipConfig = JsonConvert.SerializeObject(new ShipConfig(config)),
				gameState = JsonConvert.SerializeObject(game),

				gameStateObj = game
			};
			return m;
		}

		public void ReConstructState()
		{
			gameStateObj = JsonConvert.DeserializeObject<GameState>(gameState);
		}

		public void JoinMatch(GameBoard player2Board)
		{
			gameStateObj.isFull = true;
			gameStateObj.gameBoards[1] = player2Board;
			isFull = true;
			timeStamp = DateTime.Now;
			gameState = JsonConvert.SerializeObject(gameStateObj);
		}

		public bool hit(bool isUser1, Point p, out bool goAgain)
		{
			bool hasHit = gameStateObj.hit(isUser1, p, out goAgain);
			if (!goAgain)
			{
				isUser1 = !isUser1;
			}
			return hasHit;
		}

		public bool hasWon(bool isUser1)
		{
			if (isUser1)
				return gameStateObj.gameBoards[0].numFilled == 0;
			else
				return gameStateObj.gameBoards[1].numFilled == 0;
		}
    }

	class GameState
	{
		public GameBoard[] gameBoards { get; set; }
		public bool isFull;

		private GameState()
		{
			gameBoards = new GameBoard[2];
			isFull = false;
		}

		[JsonConstructor]
		private GameState(GameBoard[] gameBoards, bool isFull)
		{
			this.gameBoards = gameBoards;
			this.isFull = isFull;
		}

		public static GameState MakeNewGame(GameBoard player1Board)
		{
			GameState state = new GameState();
			state.gameBoards[0] = player1Board;
			return state;
		}

		public void AddPlayer(GameBoard player2Board)
		{
			gameBoards[1] = player2Board;
			isFull = true;
		}

		public bool hit(bool isUser1, Point p, out bool goAgain)
		{
			int val;
			GameBoard gameBoard;
			if (isUser1)
			{
				gameBoard = gameBoards[1];
				val = gameBoard.board[p.x, p.y];
			}
			else
			{
				gameBoard = gameBoards[0];
				val = gameBoard.board[p.x, p.y];
			}
			switch (val)
			{
				case 0:
					gameBoard.board[p.x, p.y] = 2;
					gameBoard.totalNum--;
					goAgain = false;
					return false;
				case 1:
					gameBoard.board[p.x, p.y] = 3;
					gameBoard.totalNum--;
					gameBoard.numFilled--;
					goAgain = true;
					return true;
				case 2:
					gameBoard.board[p.x, p.y] = 2;
					goAgain = true;
					return false;
				case 3:
					gameBoard.board[p.x, p.y] = 3;
					goAgain = true;
					return true;
				default: //never happens
					goAgain = false;
					return false;
			}
		}
	}
}
