using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackToNearbyChests
{
	class ModConfig
	{
		public int Radius { get; set; } = 5;
		public string ControllerButton { get; set; } = Microsoft.Xna.Framework.Input.Buttons.RightStick.ToString();
	}
}
