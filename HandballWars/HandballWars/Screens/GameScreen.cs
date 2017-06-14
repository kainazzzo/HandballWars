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

namespace HandballWars.Screens
{
	public partial class GameScreen
	{
	    private Stage stage;

        void CustomInitialize()
		{
            Camera.Main.X += Camera.Main.OrthogonalWidth / 2.0f;
            Camera.Main.Y -= Camera.Main.OrthogonalHeight / 2.0f;
            FlatRedBallServices.GraphicsOptions.TextureFilter = Microsoft.Xna.Framework.Graphics.TextureFilter.Point;
            
            stage = new Stage(BasicArena);

		    Player1.X = 200;
		    Player1.Y = -100;

            var ball = BallFactory.CreateNew();

            ball.X = 400;
            ball.Y = -100;
		}

		void CustomActivity(bool firstTimeCalled)
		{
            foreach (var BallInstance in BallList)
            {
                if (BallInstance.Held == null)
                {
                    if (Player1.CollideAgainst(BallInstance.SecuredCircleInstance))
                    {
                        BallInstance.Held = Player1;
                        BallInstance.YAcceleration = 0;
                    }
                }
                else if (!Player1.CollideAgainst(BallInstance.DropCircleInstance))
                {
                    BallInstance.Held = null;
                }

                if (BallInstance.Held == null)
                {
                    BallInstance.YAcceleration = -100f;
                    stage.CheckCollision(BallInstance);
                }
            }

            stage.CheckCollision(Player1);
        }

		void CustomDestroy()
		{


		}

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

	}
}
