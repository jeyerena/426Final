using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipServer.Models
{
	public class ResponseMessageObj
	{
		public string message;

		public ResponseMessageObj(string m)
		{
			this.message = m;
		}
    }
}
