using FlatRedBall.Graphics.Particle;
using Microsoft.Xna.Framework;

namespace HandballWars.Entities
{
    public static class EmitterExtensions
    {
        public static void SetEmissionColor(this Emitter emitter, Color color)
        {
            emitter.EmissionSettings.Red = color.R;
            emitter.EmissionSettings.Green = color.G;
            emitter.EmissionSettings.Blue = color.B;
        }
    }
    
}
