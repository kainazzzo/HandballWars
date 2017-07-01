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

            Camera.Main.Position = center;
            //Camera.Main.DestinationRectangle = new Rectangle((int)leftMost.X, (int)topMost.Y, (int)Math.Abs(rightMost.X - leftMost.X), (int)Math.Abs(topMost.Y - bottomMost.Y));

            //cameraIndicator.Position = center;
            //cameraIndicator.Position = new Vector3(GuiManager.Cursor.WorldXAt(0f), GuiManager.Cursor.WorldYAt(0f), 0f);
            
            //FlatRedBall.Debugging.Debugger.CommandLineWrite(center);
            
            FlatRedBall.Debugging.Debugger.Write($"OrthoWidth: {Camera.Main.OrthogonalWidth}  OrthoHeight: {Camera.Main.OrthogonalHeight}");

            var width = Math.Abs((rightMost.X + 16f) - (leftMost.X - 16f));
            var height = Math.Abs((topMost.Y + 16f) - (bottomMost.Y - 16f));

            Camera.Main.OrthogonalWidth = width;
            Camera.Main.OrthogonalHeight = width / 16 * 9;
            
            if (Camera.Main.OrthogonalHeight < height)
            {
                Camera.Main.OrthogonalHeight = height;
                Camera.Main.OrthogonalWidth = height / 9 * 16;
            }

            if (InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                Camera.Main.OrthogonalHeight *= 2f;
                Camera.Main.OrthogonalWidth *= 2f;
            }
            if (InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                Camera.Main.OrthogonalHeight *= .5f;
                Camera.Main.OrthogonalWidth *= .5f;
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
