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
	public partial class Player1
	{
        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
		private void CustomInitialize()
		{
            InitializeInput();

		}

        protected override void InitializeInput()
        {
            this.HorizontalInput = new Multiple1DInputs();
            if (FlatRedBall.Input.InputManager.Xbox360GamePads[0].IsConnected)
            {
                this.JumpInput =
                    FlatRedBall.Input.InputManager.Xbox360GamePads[0].GetButton(FlatRedBall.Input.Xbox360GamePad.Button.A);

                HorizontalInput.Inputs.Add(FlatRedBall.Input.InputManager.Xbox360GamePads[0].LeftStick.Horizontal);
                HorizontalInput.Inputs.Add(FlatRedBall.Input.InputManager.Xbox360GamePads[0].DPadHorizontal);
            }
            else
            {
                this.JumpInput =
                    FlatRedBall.Input.InputManager.Keyboard.GetKey(Keys.Space);
                HorizontalInput.Inputs.Add(FlatRedBall.Input.InputManager.Keyboard.Get1DInput(Keys.Left, Keys.Right));
                FallThroughInput = InputManager.Keyboard.GetKey(Keys.Down);
            }

            InputEnabled = true;
        }
        private void CustomActivity()
		{

            FlatRedBall.Debugging.Debugger.Write(IsOnGround);
        }

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
	}
}
