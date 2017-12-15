using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace BattleShipServer.Models
{
    public class MatchDBContext
    {
		public string ConnectionString { get; set; }
		private SemaphoreSlim mutex;

		public MatchDBContext(string connectionString) //please check this query for correctness
		{
			this.ConnectionString = connectionString;
			mutex = new SemaphoreSlim(1);
			MySqlConnection conn = GetConnection();
			conn.Open();
			MySqlCommand cmd = new MySqlCommand("drop table if exists Matches;", conn);
			cmd.ExecuteScalar();
			cmd = new MySqlCommand("create table Matches (" +
										"matchId integer primary key auto_increment," +
										"isUser1 boolean," +
										"isFull boolean," +
										"shipConfig blob," +
										"xSize integer," +
										"ySize integer," +
										"gameState blob," +
										"timeStamp datetime)", conn);
			cmd.ExecuteScalar();
			conn.Close();
		}

		private MySqlConnection GetConnection()
		{
			return new MySqlConnection(ConnectionString);
		}

		public async Task<List<Match>> FindBestAvailMatch(GameConfig config)
		{
			List<Match> list = new List<Match>();
			int xSize = config.xSize;
			int ySize = config.ySize;
			string shipConfig = JsonConvert.SerializeObject(new ShipConfig(config));
			MySqlConnection conn = GetConnection();
			await mutex.WaitAsync();
			conn.Open();
			MySqlCommand cmd = new MySqlCommand($"delete from Matches where timeStamp <= '{DateTime.Now.AddMinutes(-2).ToString("yyyy/MM/dd HH:mm:ss")}';", conn);
			cmd.ExecuteScalar();
			cmd = new MySqlCommand($"select * from Matches where xSize = '{xSize}' and ySize = '{ySize}' and shipConfig = '{shipConfig}' and isFull = 'false' order by timeStamp desc limit 1;", conn);
			MySqlDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				Match temp = new Match()
				{
					matchId = reader.GetInt32("matchId"),
					isUser1 = reader.GetBoolean("isUser1"),
					isFull = reader.GetBoolean("isFull"),
					shipConfig = reader.GetString("shipConfig"),
					xSize = reader.GetInt32("xSize"),
					ySize = reader.GetInt32("ySize"),
					gameState = reader.GetString("gameState"),
					timeStamp = reader.GetDateTime("timeStamp")
				};
				temp.ReConstructState();
				list.Add(temp);
			}
			conn.Close();
			mutex.Release();
			return list;
		}

		public async Task AddNewMatch(Match match)
		{
			MySqlConnection conn = GetConnection();
			await mutex.WaitAsync();
			conn.Open();
			MySqlCommand cmd = new MySqlCommand($"insert into Matches values(default, '{Convert.ToInt32(match.isUser1)}', '{Convert.ToInt32(match.isFull)}', '{match.shipConfig}', '{match.xSize}', '{match.ySize}', '{match.gameState}', '{match.timeStamp.ToString("yyyy/MM/dd HH:mm:ss")}');", conn);
			cmd.ExecuteScalar();
			conn.Close();
			mutex.Release();
		}

		public async Task UpdateMatchRecord(Match match)
		{
			MySqlConnection conn = GetConnection();
			await mutex.WaitAsync();
			conn.Open();
			MySqlCommand cmd = new MySqlCommand($"update Matches set isUser1 = '{Convert.ToInt32(match.isUser1)}', " +
																	$"isFull = '{Convert.ToInt32(match.isFull)}', " +
																	$"shipConfig = '{match.shipConfig}', " +
																	$"xSize = '{match.xSize}', " +
																	$"ySize = '{match.ySize}', " +
																	$"gameState = '{match.gameState}', " +
																	$"timeStamp = '{match.timeStamp.ToString("yyyy/MM/dd HH:mm:ss")}' where matchId = '{match.matchId}';", conn);
			cmd.ExecuteScalar();
			conn.Close();
			mutex.Release();
		}

		public async Task DeleteMatchRecord(int matchId)
		{
			MySqlConnection conn = GetConnection();
			await mutex.WaitAsync();
			conn.Open();
			MySqlCommand cmd = new MySqlCommand($"delete from Matches where matchId = {matchId};", conn);
			cmd.ExecuteScalar();
			conn.Close();
			mutex.Release();
		}
    }
}
