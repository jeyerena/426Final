using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Generic;
using BattleshipServer;
using Newtonsoft.Json;

namespace BattleshipServer
{
	class Program
	{
		static void Main(string[] args)
		{
			Dictionary<int, GameInstance> games;

			var web = new HttpListener();
			web.Prefixes.Add("http://localhost:8888/");
			Console.WriteLine("Listening..");
			web.Start();

			HttpListenerContext context = web.GetContext();
			HttpListenerRequest request = context.Request;
			HttpListenerResponse response = context.Response;

			StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8);
			Console.WriteLine($"HTTP Method:\n{request.HttpMethod}");
			Console.WriteLine($"Header:\n{request.Headers}");

			string jsonstring = @"{
									'ships': [
										{ 'x':'1', 'y':'2', 'length':'3', 'isVertical':'true' },
										{ 'x':'3', 'y':'3', 'length':'3', 'isVertical':'false' },
										{ 'x':'5', 'y':'5', 'length':'2', 'isVertical':'false' }
									],
									'xSize': '7',
									'ySize': '8'
								}";
			string jsonstring2 = @"{
									'ships': [
										{ 'x':'3', 'y':'2', 'length':'3', 'isVertical':'true' },
										{ 'x':'4', 'y':'3', 'length':'3', 'isVertical':'false' },
										{ 'x':'1', 'y':'5', 'length':'2', 'isVertical':'false' },
										{ 'x':'6', 'y':'7', 'length':'1', 'isVertical':'true' }
									],
									'xSize': '7',
									'ySize': '8'
								}";

			GameConfig config = GameConfig.MakeConfigFromJSON(jsonstring);
			GameBoard player1Board;
			bool succeeded = GameBoard.ConstructBoard(config, out player1Board);
			GameConfig config2 = GameConfig.MakeConfigFromJSON(jsonstring2);
			GameBoard player2Board;
			bool succeeded2 = GameBoard.ConstructBoard(config2, out player2Board);
			GameInstance game = new GameInstance();
			bool asdf = game.addPlayer(player1Board);
			bool aaaa = game.addPlayer(player2Board);

			bool isNewGameRequest;
			if (bool.TryParse(request.Headers.Get("New-Game"), out isNewGameRequest) && isNewGameRequest)
			{

			}
			else
			{

			}

			const string responseString = "<html><body>Hello world</body></html>";
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
			response.ContentLength64 = buffer.Length;

			System.IO.Stream output = response.OutputStream;
			output.Write(buffer, 0, buffer.Length);

			output.Close();
			web.Stop();
			Console.ReadKey();
		}
	}
}
