using FlatRedBall.Graphics.Particle;
using Microsoft.Xna.Framework;

namespace HandballWars.Entities
{
    public static class EmitterExtensions
    {
        public static void SetEmissionColor(this Emitter emitter, Color color)
        {
            emitter.EmissionSettings.Red = color.R / 255f;
            emitter.EmissionSettings.Green = color.G / 255f;
            emitter.EmissionSettings.Blue = color.B / 255f;
        }
    }
    
}
