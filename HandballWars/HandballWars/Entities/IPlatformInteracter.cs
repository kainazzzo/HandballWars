using System;

namespace HandballWars.Entities
{
    public interface IPlatformInteracter
    {
        /// <summary>
        /// The last time collision checks were performed. Time values uniquely
        /// identify a game frame, so this is used to store whether collisions have
        /// been tested this frame or not. This is used to determine whether collision
        /// variables should be reset or not when a collision method is called, as
        /// multiple collisions (such as vs. solid and vs. cloud) may occur in one frame.
        /// </summary>
        double LastCollisionTime { get; set; }
        /// <summary>
        /// Whether the character is on the ground. This is false
        /// if the character has jumped or walked off of the edge
        /// of a platform.
        /// </summary>
        bool IsOnGround { get; set; }

        Action LandedAction { get; set; }
        

    }
}