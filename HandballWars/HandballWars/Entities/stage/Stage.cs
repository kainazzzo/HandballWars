using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlatRedBall.TileCollisions;
using FlatRedBall.TileGraphics;

namespace HandballWars.Entities.stage
{
    class Stage
    {

        private TileShapeCollection sCollision;
        private TileShapeCollection cCollision;

        public Stage(LayeredTileMap map)
        {
            sCollision = new TileShapeCollection();
            cCollision = new TileShapeCollection();


            var tilesWithCollision = map.TileProperties
                .Where(item => item.Value.Any(property => property.Name == "HasCollision" && String.Equals((string)property.Value, "true", StringComparison.OrdinalIgnoreCase)))
                .Where(item => item.Value.Any(property => property.Name == "Is Cloud" && !String.Equals((string)property.Value, "true", StringComparison.OrdinalIgnoreCase) ||
                               item.Value.All(property2 => property2.Name != "Is Cloud")))
                .Select(item => item.Key).ToList();

            var tilesWithCloudCollision = map.TileProperties
                .Where(item => item.Value.Any(property => property.Name == "HasCollision" && String.Equals((string)property.Value, "true", StringComparison.OrdinalIgnoreCase)))
                .Where(item => item.Value.Any(property => property.Name == "Is Cloud" && String.Equals((string)property.Value, "true", StringComparison.OrdinalIgnoreCase)))
                .Select(item => item.Key).ToList();

            sCollision.AddCollisionFrom(map, tilesWithCollision);
            cCollision.AddCollisionFrom(map, tilesWithCloudCollision);

            sCollision.Visible = false;
            cCollision.Visible = false;
        }

        public void CheckCollision(PlatformInteracter character, float elasticity)
        {
            character.CollideAgainst(() => sCollision.CollideAgainstSolid(character), false);
            character.CollideAgainst(() => cCollision.CollideAgainstSolid(character), true);
        }
    }
}
