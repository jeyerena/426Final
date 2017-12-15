﻿using System;
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
		private static SemaphoreSlim mutex = new SemaphoreSlim(1, 1);

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

		// POST /Join
		[HttpPost]
		[Route("Join")]
        public async Task<IActionResult> JoinPost([FromBody]GameConfig config)
        {
			if (!GameBoard.ConstructBoard(config, out GameBoard board)) return new ObjectResult(new ResponseMessageObj("you done fukd up son"));
			MatchDBContext context = HttpContext.RequestServices.GetService(typeof(MatchDBContext)) as MatchDBContext;
			string cookieId;
			string cookieUser;
			await mutex.WaitAsync();
			List<Match> matches = await context.FindBestAvailMatch(config); //go to MatchDBContext and code your own getter for some query
			if (matches.Count == 0)
			{
				//need to create new match
				Match newMatch = Match.MakeNewMatch(board, config);
				await context.AddNewMatch(newMatch);
				cookieId = newMatch.matchId.ToString();
				cookieUser = newMatch.isUser1.ToString();
			}
			else
			{
				matches[0].JoinMatch(board);
				await context.UpdateMatchRecord(matches[0]);
				cookieId = matches[0].matchId.ToString();
				cookieUser = matches[0].isUser1.ToString();
			}
			mutex.Release();
			CookieOptions option = new CookieOptions();
			option.Expires = DateTime.Now.AddMinutes(2);
			option.Path = "/Fire";
			Response.Cookies.Delete("matchId");
			Response.Cookies.Delete("isUser1");
			Response.Cookies.Append("matchId", cookieId, option);
			Response.Cookies.Append("isUser1", cookieUser, option);
			return new ObjectResult(board); //stub, currently just returns board object as json, board obj
        }

		[HttpPost]
		[Route("Fire")]
		public async Task<IActionResult> FirePost([FromBody]Point p)
		{
			if (!HttpContext.Request.Cookies.ContainsKey("matchId") || !HttpContext.Request.Cookies.ContainsKey("isUser1"))
				return new ObjectResult(new ResponseMessageObj("Oops, lost your cookie"));
			string cookieId = HttpContext.Request.Cookies["matchId"];
			string cookieUser = HttpContext.Request.Cookies["isUser1"];
			int matchId = Convert.ToInt32(cookieId);
			bool isUser1 = Convert.ToBoolean(cookieUser);

			MatchDBContext context = HttpContext.RequestServices.GetService(typeof(MatchDBContext)) as MatchDBContext;

			await mutex.WaitAsync();
			List<Match> matches = await context.GetMatch(matchId, isUser1);
			bool goAgain = false;
			bool hit = false;
			bool winCon = false;
			if (matches.Count != 0)
			{
				hit = matches[0].hit(isUser1, p, out goAgain);
				await context.UpdateMatchRecord(matches[0]);
				winCon = matches[0].hasWon(isUser1);
			}
			mutex.Release();

			CookieOptions option = new CookieOptions();
			option.Expires = DateTime.Now.AddMinutes(2);
			option.Path = "/Fire";
			Response.Cookies.Delete("matchId");
			Response.Cookies.Delete("isUser1");
			Response.Cookies.Append("matchId", cookieId, option);
			Response.Cookies.Append("isUser1", cookieUser, option);
			return new ObjectResult(new HitResult(hit, goAgain, winCon));
		}

		//[HttpPost]
		//[Route("Fire/Poll")]
		//public async Task<IActionResult> PollPost()
		//{
		//
		//}
    }
}
