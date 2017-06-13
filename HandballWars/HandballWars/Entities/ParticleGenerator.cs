using FlatRedBall.Graphics.Particle;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballWars.Entities
{
    public partial class ParticleGenerator
    {
        /// <summary>
        /// Dictionary provides scaling factors for explosion sizes
        /// </summary>
        private Dictionary<ParticleEffectSize, int> explosionScale = new Dictionary<ParticleEffectSize, int>()
        {
            {ParticleEffectSize.Small, 1 },
            {ParticleEffectSize.Medium, 3 },
            {ParticleEffectSize.Large, 5 }
        };

        /// <summary>
        /// Dictionary provides scaling factors for explosion sizes
        /// </summary>
        private Dictionary<ParticleEffectSize, int> ringScale = new Dictionary<ParticleEffectSize, int>()
        {
            {ParticleEffectSize.Small, 2 },
            {ParticleEffectSize.Medium, 4 },
            {ParticleEffectSize.Large, 6 }
        };

        private void CustomInitialize()
        {
            SetupExplosionEmitter();
            SetupRingEmitter();
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

        public void CreateExplosionAt(Vector3 position, Color color, ParticleEffectSize size)
        {
            Position = position;

            var scale = explosionScale[size];

            ExplosionEmitter.SetEmissionColor(color);
            ExplosionEmitter.NumberPerEmission = DefaultNumberPerEmission * scale;

            ExplosionEmitter.EmissionSettings.ScaleY = DefaultMinScale * scale;
            ExplosionEmitter.EmissionSettings.ScaleYRange = (DefaultMaxScale - DefaultMinScale) * scale;
            ExplosionEmitter.EmissionSettings.RadialVelocity = DefaultMinVelocity * scale;
            ExplosionEmitter.EmissionSettings.RadialVelocityRange = (DefaultMaxVelocity - DefaultMinVelocity) * scale;

            ExplosionEmitter.Emit();
        }

        public void CreateRingAt(Vector3 position, Color color, ParticleEffectSize size)
        {
            Position = position;

            var scale = ringScale[size];

            RingEmitter.SetEmissionColor(color);
            // NOTE: rings need more particles to look like a ring!
            RingEmitter.NumberPerEmission = DefaultNumberPerEmission * 4 * scale;

            RingEmitter.EmissionSettings.ScaleY = DefaultMinScale * scale;
            RingEmitter.EmissionSettings.ScaleYRange = (DefaultMaxScale - DefaultMinScale) * scale;
            RingEmitter.EmissionSettings.RadialVelocity = DefaultMaxVelocity * scale;
            RingEmitter.EmissionSettings.RadialVelocityRange = 0;

            RingEmitter.Emit();
        }

        private void SetupExplosionEmitter()
        {
            ExplosionEmitter.TimedEmission = false;
            ExplosionEmitter.RemovalEvent = Emitter.RemovalEventType.Alpha0;
            ExplosionEmitter.AreaEmission = Emitter.AreaEmissionType.Point;
            ExplosionEmitter.EmissionSettings = DefaultEmissionSettings;
        }

        private void SetupRingEmitter()
        {
            RingEmitter.TimedEmission = false;
            RingEmitter.RemovalEvent = Emitter.RemovalEventType.Alpha0;
            RingEmitter.AreaEmission = Emitter.AreaEmissionType.Point;
            RingEmitter.EmissionSettings = DefaultEmissionSettings;

            // override for slower particles
            RingEmitter.EmissionSettings.RadialVelocity = 300f;
            RingEmitter.EmissionSettings.RadialVelocityRange = 50f;
            RingEmitter.EmissionSettings.Drag = 5f;
            RingEmitter.EmissionSettings.Alpha = 0.5f;
            RingEmitter.EmissionSettings.AlphaRate = -0.25f;
        }

        private EmissionSettings DefaultEmissionSettings
        {
            get
            {
                var defaultRotationSpeed = (float)Math.PI * DefaultMaxRotationsPerSecond;

                return new EmissionSettings()
                {
                    Alpha = 1f,
                    AlphaRate = -DefaultSecondsToFade,
                    AnimationChain = Particles["Smoke"],
                    ColorOperation = FlatRedBall.Graphics.ColorOperation.Modulate,
                    Drag = DefaultDrag,
                    ScaleY = DefaultMinScale,
                    ScaleYRange = DefaultMaxScale,
                    MatchScaleXToY = true,
                    RadialVelocity = DefaultMinVelocity,
                    RadialVelocityRange = DefaultMaxVelocity - DefaultMinVelocity,
                    RotationZ = -3.14f,
                    RotationZRange = 6.28f,
                    RotationZVelocity = -defaultRotationSpeed,
                    RotationZVelocityRange = defaultRotationSpeed * 2f,
                    VelocityRangeType = RangeType.Radial
                };
            }
        }
    }
}
