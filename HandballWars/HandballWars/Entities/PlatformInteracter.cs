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
using Microsoft.Xna.Framework;

namespace HandballWars.Entities
{
	public partial class PlatformInteracter : IPlatformInteracter
    {
        public double LastCollisionTime { get; set; }
        public bool IsOnGround { get; set; }
        public Action LandedAction { get; set; }

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


		}

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        /// <summary>
        /// Performs a standard solid collision against a ShapeCollection.
        /// </summary>
        /// <param name="shapeCollection"></param>
        public void CollideAgainst(ShapeCollection shapeCollection, float elasticity)
        {
            CollideAgainst(shapeCollection, elasticity, false);
        }

        /// <summary>
        /// Performs a solid or cloud collision against a ShapeCollection.
        /// </summary>
        /// <param name="shapeCollection">The ShapeCollection to collide against.</param>
        /// <param name="isCloudCollision">Whether to perform solid or cloud collisions.</param>
        public void CollideAgainst(ShapeCollection shapeCollection, float elasticity, bool isCloudCollision)
        {
            CollideAgainst(() => shapeCollection.CollideAgainstBounceWithoutSnag(this.AxisAlignedRectangleInstance, elasticity), isCloudCollision);
        }

        /// <summary>
        /// Executes the collisionFunction to determine if a collision occurred, and if so, reacts
        /// to the collision by modifying the state of the object and raising appropriate events.
        /// This is useful for situations where custom collisions are needed, but then the standard
        /// behavior is desired if a collision occurs.
        /// </summary>
        /// <param name="collisionFunction">The collision function to execute.</param>
        /// <param name="isCloudCollision">Whether to perform cloud collision (only check when moving down)</param>
        public void CollideAgainst(Func<bool> collisionFunction, bool isCloudCollision)
        {
            Vector3 positionBeforeCollision = this.Position;
            Vector3 velocityBeforeCollision = this.Velocity;

            float lastY = this.Y;

            bool isFirstCollisionOfTheFrame = TimeManager.CurrentTime != LastCollisionTime;

            if (isFirstCollisionOfTheFrame)
            {
                LastCollisionTime = TimeManager.CurrentTime;
                IsOnGround = false;
            }

            if (isCloudCollision == false || velocityBeforeCollision.Y < 0)
            {

                if (collisionFunction())
                {
                    // make sure that we've been moved up, and that we're falling
                    bool shouldApplyCollision = true;
                    if (isCloudCollision)
                    {
                        if (this.Y <= positionBeforeCollision.Y)
                        {
                            shouldApplyCollision = false;
                        }
                    }

                    if (shouldApplyCollision)
                    {

                        if (this.Y > lastY)
                        {
                            if (!IsOnGround && LandedAction != null)
                            {
                                LandedAction();
                            }
                            IsOnGround = true;
                        }
                       
                    }
                    else
                    {
                        Position = positionBeforeCollision;
                        Velocity = velocityBeforeCollision;
                    }
                }
            }
        }
    }
}
