using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;
using StardewModdingAPI.Events;

namespace StackToNearbyChests
{
    class ControllerButtonReceiver
    {
        private Buttons? ControllerButton = null;

        internal ControllerButtonReceiver()
        {
            string configString = ModEntry.Config.ControllerButton;

            if (Enum.TryParse<Buttons>(configString, out Buttons _controllerButton))
                ControllerButton = _controllerButton;
        }


        internal void ControlEvents_ControllerButtonPressed(object sender, EventArgsControllerButtonPressed args)
        {
            if (ControllerButton.HasValue && ControllerButton.Value.Equals(args.ButtonPressed))
                StackLogic.StackToNearbyChests(ModEntry.Config.Radius);
        }
    }
}
