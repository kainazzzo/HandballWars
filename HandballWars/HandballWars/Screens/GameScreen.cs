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


namespace HandballWars.Screens
{
	public partial class GameScreen
	{
	    private TileShapeCollection mCollision;

		void CustomInitialize()
		{
            Camera.Main.X += Camera.Main.OrthogonalWidth / 2.0f;
            Camera.Main.Y -= Camera.Main.OrthogonalHeight / 2.0f;
            FlatRedBallServices.GraphicsOptions.TextureFilter = Microsoft.Xna.Framework.Graphics.TextureFilter.Point;
            mCollision = new TileShapeCollection();
            mCollision.AddCollisionFrom(BasicArena);
		    mCollision.Visible = false;
		}

		void CustomActivity(bool firstTimeCalled)
		{
            Player1.CollideAgainst(() => mCollision.CollideAgainstSolid(Player1.RectangleInstance), false);
        }

		void CustomDestroy()
		{


		}

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

	}
}
