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
using FlatRedBall.Gui;
using Microsoft.Xna.Framework;


namespace HandballWars.Entities
{
	public partial class Ball : IPlatformInteracter
	{
        public PlatformerCharacterBase Held { get; set; } = null;
        public double LastCollisionTime { get; set; }
        public bool IsOnGround { get; set; }
        public Action LandedAction { get; set; } = null;

        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
		{


		}

		private void CustomActivity()
		{

            ParticleGeneratorInstance.CreateRingAt(Position, new Color(0, 101, 135), ParticleEffectSize.Medium);
            
            if (this.Held != null)
            {
                var distance = this.Held.Position - this.Position;

                this.Acceleration = distance;
                var trajectoryX = this.Held.ThrowTrajectoryInput.X;
                var trajectoryY = this.Held.ThrowTrajectoryInput.Y;
                Acceleration.X += trajectoryX * 32f;
                Acceleration.Y += trajectoryY * 32f;
                

                this.Acceleration *= HeldAccelerationSpeed;
                this.Drag = HeldDrag;
            }
            else
            {
                this.Acceleration = new Vector3(0, -1000f, 0);
                this.Drag = 0f;
            }
        }

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
        
    }
}
