using FlatRedBall.Graphics.Particle;
using FlatRedBall.Input;
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
            if (InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                RingEmitter.EmissionSettings.RadialVelocity += 10;
            }
            else if (InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                RingEmitter.EmissionSettings.RadialVelocity -= 10;
            }

            if (InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.U))
            {
                RingEmitter.EmissionSettings.Drag += 1;
            }
            else if (InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.Y))
            {
                RingEmitter.EmissionSettings.Drag -= 1;
            }

            FlatRedBall.Debugging.Debugger.Write($"RingEmitter.EmissionSettings.RadialVelocity: {RingEmitter.EmissionSettings.RadialVelocity}\nDrag: {RingEmitter.EmissionSettings.Drag}");

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
            RingEmitter.EmissionSettings.RadialVelocity = DefaultMinVelocity;
            RingEmitter.EmissionSettings.RadialVelocityRange = 0f;
            RingEmitter.EmissionSettings.YAcceleration = RadialYAcceleration;
            RingEmitter.EmissionSettings.YAccelerationRange = 0f;
            RingEmitter.EmissionSettings.Drag = DefaultDrag;
            RingEmitter.EmissionSettings.Alpha = 0.5f;
            RingEmitter.EmissionSettings.AlphaRate = -2f;
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
                    //RadialVelocity = DefaultMinVelocity,
                    //RadialVelocityRange = DefaultMaxVelocity - DefaultMinVelocity,
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
