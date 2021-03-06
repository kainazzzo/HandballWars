using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework.Input;

namespace HandballWars.Entities
{
	public partial class Player2
	{
        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
		private void CustomInitialize()
		{


		}

        protected override void InitializeInput()
        {
            var horizontalInput = new Multiple1DInputs();
            var throwTrajectoryInput = new Multiple2DInputs();
            var throwInput = new MultiplePressableInputs();
            var jumpInput = new MultiplePressableInputs();
            var fallThroughInput = new MultiplePressableInputs();

            this.HorizontalInput = horizontalInput;
            this.ThrowTrajectoryInput = throwTrajectoryInput;
            this.ThrowInput = throwInput;
            this.JumpInput = jumpInput;
            this.FallThroughInput = fallThroughInput;

            if (InputManager.Xbox360GamePads[0].IsConnected)
            {
                jumpInput.Inputs.Add(InputManager.Xbox360GamePads[0].GetButton(FlatRedBall.Input.Xbox360GamePad.Button.A));
                horizontalInput.Inputs.Add(InputManager.Xbox360GamePads[0].LeftStick.Horizontal);
                horizontalInput.Inputs.Add(InputManager.Xbox360GamePads[0].DPadHorizontal);
                fallThroughInput.Inputs.Add(InputManager.Xbox360GamePads[0].GetButton(Xbox360GamePad.Button.DPadDown));
            }

            jumpInput.Inputs.Add(InputManager.Keyboard.GetKey(Keys.Up));
            horizontalInput.Inputs.Add(InputManager.Keyboard.Get1DInput(Keys.Left, Keys.Right));
            fallThroughInput.Inputs.Add(InputManager.Keyboard.GetKey(Keys.Down));
            throwTrajectoryInput.Inputs.Add(InputManager.Keyboard.Get2DInput(Keys.F, Keys.H, Keys.T, Keys.G));
            throwInput.Inputs.Add(InputManager.Keyboard.GetKey(Keys.RightControl));
            

            InputEnabled = true;
        }

        private void CustomActivity()
		{
           

        }

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
	}
}
