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
			gameStateObj.gameBoards[2] = player2Board;
			isFull = true;
			timeStamp = DateTime.Now;
			gameState = JsonConvert.SerializeObject(gameStateObj);
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
	}
}
