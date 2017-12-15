using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipServer.Models
{
    public class HitResult
    {
		public bool hasHit { get; set; }
		public bool goAgain { get; set; }
		public bool winCon { get; set; }

		public HitResult(bool hasHit, bool goAgain, bool winCon)
		{
			this.hasHit = hasHit;
			this.goAgain = goAgain;
			this.winCon = winCon;
		}
    }
}
