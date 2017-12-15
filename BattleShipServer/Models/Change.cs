using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipServer.Models
{
	public class ChangeHistory
	{
		public List<Change> changes = new List<Change>();
	}

	public class Change
	{
		bool hasHit { get; set; }
		bool goAgain { get; set; }
		bool winCon { get; set; }
		int x { get; set; }
		int y { get; set; }

		public Change(HitResult enemyResult, Point hitPos)
		{
			this.hasHit = enemyResult.hasHit;
			this.goAgain = enemyResult.goAgain;
			this.winCon = enemyResult.winCon;
			this.x = hitPos.x;
			this.y = hitPos.y;
		}
	}
}
