using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipServer.Models
{
	public class InvalidInputObject
	{
		public string Error;

		public InvalidInputObject(string e)
		{
			this.Error = e;
		}
    }
}
