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
using Microsoft.Xna.Framework;

namespace HandballWars.Screens
{
	public partial class GameScreen
	{
	    private TileShapeCollection sCollision;
	    private TileShapeCollection cCollision;

        void CustomInitialize()
		{
            Camera.Main.X += Camera.Main.OrthogonalWidth / 2.0f;
            Camera.Main.Y -= Camera.Main.OrthogonalHeight / 2.0f;
            FlatRedBallServices.GraphicsOptions.TextureFilter = Microsoft.Xna.Framework.Graphics.TextureFilter.Point;

		    sCollision = new TileShapeCollection();
		    cCollision = new TileShapeCollection();

            
            var tilesWithCollision = BasicArena.TileProperties
		        .Where(item => item.Value.Any(property => property.Name == "HasCollision" && Convert.ToBoolean(property.Value) == true))
		        .Where(item => item.Value.Any(property => property.Name == "Is Cloud" && Convert.ToBoolean(property.Value) == false) ||
                    item.Value.All(property => property.Name != "Is Cloud"))
                .Select(item => item.Key).ToList();

		    var tilesWithCloudCollision = BasicArena.TileProperties
		        .Where(item => item.Value.Any(property => property.Name == "HasCollision" && Convert.ToBoolean(property.Value) == true))
		        .Where(item => item.Value.Any(property => property.Name == "Is Cloud" && Convert.ToBoolean(property.Value) == true))
                .Select(item => item.Key).ToList();

            sCollision.AddCollisionFrom(BasicArena, tilesWithCollision);
            cCollision.AddCollisionFrom(BasicArena, tilesWithCloudCollision);

		    sCollision.Visible = false;
		    cCollision.Visible = false;
         
		    Player1.X = 200;
		    Player1.Y = -100;
            
		}

		void CustomActivity(bool firstTimeCalled)
		{
            Player1.CollideAgainst(() => sCollision.CollideAgainstSolid(Player1), false);
            Player1.CollideAgainst(() => cCollision.CollideAgainstSolid(Player1), true);

            ParticleGeneratorInstance.CreateRingAt(BallInstance.Position, Color.Blue, Entities.ParticleEffectSize.Small);
        }

		void CustomDestroy()
		{


		}

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

	}
}
