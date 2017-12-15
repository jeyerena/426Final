using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipServer.Models
{
    public class BoardChangeHistory
    {
		public List<Change> history { get; set; }

		[JsonConstructor]
		private BoardChangeHistory(List<Change> history)
		{
			this.history = history;
		}

		public BoardChangeHistory()
		{
			history = new List<Change>();
		}
    }

	public class Change
	{
		HitResult enemyResult { get; set; }
		Point hitPos { get; set; }

		public Change(HitResult enemyResult, Point hitPos)
		{
			this.enemyResult = enemyResult;
			this.hitPos = hitPos;
		}
	}
}
