using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BattleShipServer.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace BattleShipServer.Controllers
{
    public class ValuesController : Controller
    {
		[HttpGet]
		[Route("/")]
		public ContentResult GetHtml()
		{
			return new ContentResult
			{
				ContentType = "text/html",
				StatusCode = (int)HttpStatusCode.OK,
				Content = System.IO.File.ReadAllText("wwwroot/game.html")
			};
		}

		[HttpGet]
		[Route("game.css")]
		public ContentResult GetCSS()
		{
			return new ContentResult
			{
				ContentType = "text/css",
				StatusCode = (int)HttpStatusCode.OK,
				Content = System.IO.File.ReadAllText("wwwroot/game.css")
			};
		}

		[HttpGet]
		[Route("game.js")]
		public ContentResult GetJS()
		{
			return new ContentResult
			{
				ContentType = "text/javascript",
				StatusCode = (int)HttpStatusCode.OK,
				Content = System.IO.File.ReadAllText("wwwroot/game.js")
			};
		}

		[HttpGet]
		[Route("logo.png")]
		public FileResult GetLogo()
		{
			var imageFileStream = System.IO.File.OpenRead("wwwroot/logo.png");
			return File(imageFileStream, "image/png");
		}

		[HttpGet]
		[Route("splash.png")]
		public FileResult GetSplash()
		{
			var imageFileStream = System.IO.File.OpenRead("wwwroot/splash.png");
			return File(imageFileStream, "image/png");
		}

		// POST Game/Join
		[HttpPost]
		[Route("Join")]
        public IActionResult JoinPost([FromBody]GameConfig config)
        {
			//needs to handle matchmaking
			int id = 1; //gets this from database
			GameBoard.ConstructBoard(config, out GameBoard board); //constructs board from config object
			string jsonout = JsonConvert.SerializeObject(board); //serializes gameboard object to json and writes it to id.txt file
			MySqlConnection conn = new MySqlConnection("Server=localhost;database=ztongdb;user=ztong;password=admin");
			conn.Open();
			MySqlCommand cmd = new MySqlCommand("SELECT shipconfig FROM Matches", conn);
			MySqlDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				var shipconfig = reader.ToString();
			}
			conn.Close();
			//no way to delete created files currently
			return new ObjectResult(board); //stub, currently just returns board object as json, board obj
        }

		// POST Game/Fire
		[HttpPost]
		[Route("Fire")]
		public ContentResult FirePost([FromBody]GameConfig config)
		{
			//reads from 1.txt and returns its content as html
			//can easily modify it to output some object that contains desired result as json
			StreamReader sr = new StreamReader("Content/1.txt");
			string line = sr.ReadToEnd();
			return new ContentResult
			{
				ContentType = "text/html",
				StatusCode = (int)HttpStatusCode.OK,
				Content = "<html><body>"+ line +"</body></html>"
			};
		}
    }
}
