using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipServer
{
	public class Ship
	{
		public readonly int x;
		public readonly int y;
		public readonly int length;
		public readonly bool isVertical;

		public Ship(int x, int y, int length, bool isVertical)
		{
			this.x = x;
			this.y = y;
			this.length = length;
			this.isVertical = isVertical;
		}
	}
}
