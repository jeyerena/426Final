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
using System.Data;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace BattleShipServer.Controllers
{
    public class ValuesController : Controller
    {
		private SemaphoreSlim mutex = new SemaphoreSlim(1);

		[HttpGet]
		[Route("/")]
		public ContentResult GetHtml()
		{
			return new ContentResult
			{
				ContentType = "text/html",
				StatusCode = (int)HttpStatusCode.OK,
				Content = System.IO.File.ReadAllText("wwwroot/index.html")
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

		// POST /Join
		[HttpPost]
		[Route("Join")]
        public async Task<IActionResult> JoinPost([FromBody]GameConfig config)
        {
			if (!GameBoard.ConstructBoard(config, out GameBoard board)) return new ObjectResult(new InvalidInputObject("you done fukd up son"));
			MatchDBContext context = HttpContext.RequestServices.GetService(typeof(MatchDBContext)) as MatchDBContext;
			await mutex.WaitAsync();
			List<Match> matches = await context.FindBestAvailMatch(config); //go to MatchDBContext and code your own getter for some query
			if (matches.Count == 0)
			{
				//need to create new match
				await context.AddNewMatch(Match.MakeNewMatch(board, config));
			}
			else
			{
				matches[0].JoinMatch(board);
				await context.UpdateMatchRecord(matches[0]);
			}
			mutex.Release();
			return new ObjectResult(board); //stub, currently just returns board object as json, board obj
        }
    }
}
