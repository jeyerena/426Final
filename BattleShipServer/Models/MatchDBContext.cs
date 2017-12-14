using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipServer.Models
{
    public class MatchDBContext
    {
		public string ConnectionString { get; set; }

		public MatchDBContext(string connectionString) //please check this query for correctness
		{
			this.ConnectionString = connectionString;
			MySqlConnection conn = GetConnection();
			conn.Open();
			MySqlCommand cmd = new MySqlCommand("drop table if exists Matches;", conn);
			cmd.ExecuteScalar();
			cmd = new MySqlCommand("create table Matches (" +
										"matchId integer primary key auto increment," +
										"isUser1 boolean," +
										"isFull boolean," +
										"shipConfig bloc," +
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

		public List<Match> GetAllMatches()
		{
			List<Match> list = new List<Match>();

			MySqlConnection conn = GetConnection();
			conn.Open();
			MySqlCommand cmd = new MySqlCommand("select * from Matches;", conn);
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
			return list;
		}

		//basically Match.shipConfig is an int array of ship lengths
		//if 2 ship configs match, the 2 serialized jsons of the ship configs should also exactly match
		//have fun!
		public List<Match> FindMatches(GameConfig config)
		{
			List<Match> list = new List<Match>();
			int xSize = config.xSize;
			int ySize = config.ySize;
			string shipConfig = JsonConvert.SerializeObject(new ShipConfig(config));

			MySqlConnection conn = GetConnection();
			conn.Open();
			MySqlCommand cmd = new MySqlCommand();
			conn.Close();
			return list;
		}
    }
}
