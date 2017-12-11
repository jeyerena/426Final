using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BattleShipServer.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace BattleShipServer.Controllers
{
    [Route("Game")]
    public class ValuesController : Controller
    {
        //services GET requests at url/Game
        [HttpGet]
        public ContentResult GetPage() //I'm trying to see if I can get this shitty thing to service the webpages css and js scripts too
        {
			//need to implment shit
			//basically reads GamePage.html from Content folder into a string and send it back
			//this services the first page get
			return new ContentResult
			{
				ContentType = "text/html",
				StatusCode = (int)HttpStatusCode.OK,
				Content = "<html><body>Hello World</body></html>"
			};
		}

		//services GET requests at url/Game/Content/GamePage.css
		[HttpGet]
		[Route("Content/GamePage.css")]
		public ContentResult GetCSS()
		{
			//need to implment shit
			//basically reads GamePage.html from Content folder into a string and send it back
			//this services the first page get
			return new ContentResult
			{
				ContentType = "text/html",
				StatusCode = (int)HttpStatusCode.OK,
				Content = "<html><body>Hello World</body></html>"
			};
		}

		//services GET requests at url/Game/Content/GamePage.js
		[HttpGet]
		[Route("Content/GamePage.js")]
		public ContentResult GetJS()
		{
			//need to implment shit
			//basically reads GamePage.html from Content folder into a string and send it back
			//this services the first page get
			return new ContentResult
			{
				ContentType = "text/html",
				StatusCode = (int)HttpStatusCode.OK,
				Content = "<html><body>Hello World</body></html>"
			};
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
			TextWriter writer = new StreamWriter($"Content/{id}.txt");
			writer.WriteLine(jsonout);
			writer.Close();
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
