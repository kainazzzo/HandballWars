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

namespace HandballWars.Screens
{
	public partial class GameScreen
	{
	    private Stage stage;
        List<PlatformerCharacterBase> PlatformerCharacterBaseList;

        void CustomInitialize()
		{
            Camera.Main.X += Camera.Main.OrthogonalWidth / 2.0f;
            Camera.Main.Y -= Camera.Main.OrthogonalHeight / 2.0f;
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
                            BallInstance.YAcceleration = 0;
                        }
                    }
                }

                foreach (var player in PlatformerCharacterBaseList)
                {
                    if (BallInstance.Held == player && !player.CollideAgainst(BallInstance.DropCircleInstance))
                    {
                        BallInstance.Held = null;
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
            if (Camera.Main.Parent == null && PlatformerCharacterBaseList?.Count > 0)
            {
                Camera.Main.AttachTo(PlatformerCharacterBaseList[0], true);
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
