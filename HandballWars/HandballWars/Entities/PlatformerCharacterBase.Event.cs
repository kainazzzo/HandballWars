using System;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Specialized;
using FlatRedBall.Audio;
using FlatRedBall.Screens;
using HandballWars.Entities;
using HandballWars.Screens;
namespace HandballWars.Entities
{
	public partial class PlatformerCharacterBase
	{
		void OnAfterGroundMovementSet (object sender, EventArgs e)
        {
            // Force a refresh so that the new type is immediately applied
            this.CurrentMovementType = this.CurrentMovementType;    
        }
        void OnAfterAirMovementSet (object sender, EventArgs e)
        {
            // Force a refresh so that the new type is immediately applied
            this.CurrentMovementType = this.CurrentMovementType;    

        }
        void OnAfterAfterDoubleJumpSet (object sender, EventArgs e)
        {
            // Force a refresh so that the new type is immediately applied
            this.CurrentMovementType = this.CurrentMovementType;    
        }

	}
}
