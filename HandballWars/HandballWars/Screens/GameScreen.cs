using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Localization;
using FlatRedBall.TileCollisions;
using HandballWars.Entities.stage;
using Microsoft.Xna.Framework;
using HandballWars.Factories;
using FlatRedBall.TileEntities;
using HandballWars.Entities;
using FlatRedBall.Gui;

namespace HandballWars.Screens
{
	public partial class GameScreen
	{
	    private Stage stage;
        List<PlatformerCharacterBase> PlatformerCharacterBaseList;

        void CustomInitialize()
		{

            FlatRedBallServices.GraphicsOptions.TextureFilter = Microsoft.Xna.Framework.Graphics.TextureFilter.Point;

            BasicArena.AddToManagers();
            stage = new Stage(BasicArena);

            

		   
            var ball = BallFactory.CreateNew();

            ball.X = 400;
            ball.Y = -100;

            PlatformerCharacterBaseList = new List<PlatformerCharacterBase>();

            TileEntityInstantiator.CreateEntitiesFrom(BasicArena);

            PlatformerCharacterBaseList.AddRange(PlatformInteracterList.OfType<PlatformerCharacterBase>());
		}

		void CustomActivity(bool firstTimeCalled)
		{
            HandleCamera();
            foreach (var BallInstance in BallList)
            {
                if (BallInstance.Held == null)
                {
                    foreach (var player in PlatformerCharacterBaseList) {
                        if (BallInstance.Held == null && player.CollideAgainst(BallInstance.SecuredCircleInstance))
                        {
                            BallInstance.Held = player;
                            player.BallHeld = BallInstance;
                            BallInstance.YAcceleration = 0;
                        }
                    }
                }

                foreach (var player in PlatformerCharacterBaseList)
                {
                    if (BallInstance.Held == player && !player.CollideAgainst(BallInstance.DropCircleInstance))
                    {
                        BallInstance.Held = null;
                        player.BallHeld = null;
                    }
                }

                if (BallInstance.Held == null)
                {
                    BallInstance.YAcceleration = BallGravity;
                    
                    stage.CheckCollision(BallInstance, BallElasticity);
                }
            }


            foreach (var player in PlatformerCharacterBaseList)
            {
                stage.CheckCollision(player, 0.0f);
            }
        }

        private void HandleCamera()
        {
           
            if (PlatformInteracterList.Count == 0)
            {
                return;
            }

            var leftMost = PlatformInteracterList[0];
            var rightMost = PlatformInteracterList[0];
            var topMost = PlatformInteracterList[0];
            var bottomMost = PlatformInteracterList[0];

            foreach (var actor in PlatformInteracterList)
            {
                if (actor.X < leftMost.X)
                {
                    leftMost = actor;
                }

                if (actor.X > rightMost.X)
                {
                    rightMost = actor;
                }

                if (actor.Y > topMost.Y)
                {
                    topMost = actor;
                }
                
                if (actor.Y < bottomMost.Y)
                {
                    bottomMost = actor;
                }
            }

            var center = new Vector3((rightMost.X + leftMost.X) / 2.0f, (topMost.Y + bottomMost.Y) / 2.0f, 40f);

            //var center = new Vector3((rightMost.X + leftMost.X) / 2.0f, -300, 0);

            Vector3 velocityToSet = center - Camera.Main.Position;
            velocityToSet.Z = 0;

            Camera.Main.Velocity = velocityToSet * CameraMovementCoefficient;


            var width = Math.Abs((rightMost.X + 64f) - (leftMost.X - 64f));
            var height = Math.Abs((topMost.Y + 64f) - (bottomMost.Y - 64f));

            Camera.Main.OrthogonalWidth = Math.Max(width, MinCameraWidth);
            Camera.Main.OrthogonalHeight = Camera.Main.OrthogonalWidth / 16 * 9;
            
            if (Camera.Main.OrthogonalHeight < height || Camera.Main.OrthogonalHeight < MinCameraHeight)
            {
                Camera.Main.OrthogonalHeight = Math.Max(height, MinCameraHeight);
                Camera.Main.OrthogonalWidth = Camera.Main.OrthogonalHeight / 9 * 16;
            }
        }

        void CustomDestroy()
		{


		}

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

	}
}
