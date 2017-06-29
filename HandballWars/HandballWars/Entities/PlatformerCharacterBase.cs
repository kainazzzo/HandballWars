using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;

using FlatRedBall.Math.Geometry;
using FlatRedBall.Math.Splines;
using BitmapFont = FlatRedBall.Graphics.BitmapFont;
using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;
using HandballWars.DataTypes;
using FlatRedBall.Glue.StateInterpolation;
using StateInterpolationPlugin;
using FlatRedBall.Instructions;
#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using FlatRedBall.Screens;
#elif FRB_MDX
using Keys = Microsoft.DirectX.DirectInput.Key;


#endif



namespace HandballWars.Entities
{
    #region Enums

    public enum MovementType
    {
        Ground,
        Air,
        AfterDoubleJump
    }

    public enum HorizontalDirection
    {
        Left,
        Right
    }

    #endregion

    public partial class PlatformerCharacterBase : IInstructable
    {
        #region Fields
        

        /// <summary>
        /// Whether the character has hit its head on a solid
        /// collision this frame. This typically occurs when the
        /// character is moving up in the air. It is used to prevent
        /// upward velocity from being applied while the player is
        /// holding down the jump button.
        /// </summary>
        bool mHitHead = false;

        /// <summary>
        /// Whether the character is in the air and has double-jumped.
        /// This is used to determine which movement variables are active,
        /// effectively preventing multiple double-jumps.
        /// </summary>
        bool mHasDoubleJumped = false;

        /// <summary>
        /// The time when the jump button was last pushed. This is used to
        /// determine if upward velocity should be applied while the user
        /// holds the jump button down.
        /// </summary>
        double mTimeJumpPushed = double.NegativeInfinity;

        bool squishing = false;
        
        /// <summary>
        /// The MovementValues which were active when the user last jumped.
        /// These are used to determine the upward velocity to apply while
        /// the user holds the jump button.
        /// </summary>
        MovementValues mValuesJumpedWith;

        /// <summary>
        /// See property for information.
        /// </summary>
        MovementValues mCurrentMovement;

        /// <summary>
        /// See property for information.
        /// </summary>
        HorizontalDirection mDirectionFacing;

        /// <summary>
        /// See property for information.
        /// </summary>
        MovementType mMovementType;

        
        #endregion

        #region Properties

        /// <summary>
        /// Returns the current time, considering whether a Screen is active. 
        /// This is used to control how long a user can hold the jump button during
        /// a jump to apply upward velocity.
        /// </summary>
        double CurrentTime
        {
            get
            {
                if (ScreenManager.CurrentScreen != null)
                {
                    return ScreenManager.CurrentScreen.PauseAdjustedCurrentTime;
                }
                else
                {
                    return TimeManager.CurrentTime;
                }
            }
        }


        /// <summary>
        /// The current movement variables used for horizontal movement and jumping.
        /// These automatically get set according to the default platformer logic and should
        /// not be manually adjusted.
        /// </summary>
        protected MovementValues CurrentMovement
        {
            get
            {
                return mCurrentMovement;
            }
        }

        /// <summary>
        /// Which direciton the character is facing.
        /// </summary>
        public HorizontalDirection DirectionFacing
        {
            get
            {
                return mDirectionFacing;
            }
        }

        /// <summary>
        /// The input object which controls whether the jump was pressed.
        /// Common examples include a button or keyboard key.
        /// </summary>
		public FlatRedBall.Input.IPressableInput JumpInput
		{
			get;
			set;
		}

        public IPressableInput FallThroughInput
        {
            get;
            set;
        }

        /// <summary>
        /// The input object which controls the horizontal movement of the character.
        /// Common examples include a d-pad, analog stick, or keyboard keys.
        /// </summary>
        public FlatRedBall.Input.Multiple1DInputs HorizontalInput
        {
            get;
            set;
        }
	    
        /// <summary>
        /// The ratio that the horizontal input is being held.
        /// -1 represents full left, 0 is neutral, +1 is full right.
        /// </summary>
        protected virtual float HorizontalRatio
        {
            get
            {
                if(!InputEnabled)
                {
                    return 0;
                }
                else
                {
                    return HorizontalInput.Value;
                }
            }
        }


        /// <summary>
        /// The current movement type. This is set by the default platformer logic and
        /// is used to assign the mCurrentMovement variable.
        /// </summary>
        public MovementType CurrentMovementType
        {
            get
            {
                return mMovementType;
            }
            set
            {
                mMovementType = value;

                switch (mMovementType)
                {
                    case MovementType.Ground:
                        mCurrentMovement = GroundMovement;
                        break;
                    case MovementType.Air:
                        mCurrentMovement = AirMovement;
                        break;
                    case MovementType.AfterDoubleJump:
                        mCurrentMovement = AfterDoubleJump;
                        break;
                }
				
                if(CurrentMovement != null)
                {
                    this.YAcceleration = CurrentMovement.Gravity;
                }
            }
        }

        /// <summary>
        /// Whether input is read to control the movement of the character.
        /// This can be turned off if the player should not be able to control
        /// the character.
        /// </summary>
        public bool InputEnabled
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Action for when the character executes a jump.
        /// </summary>
        public Action JumpAction;



        private void CustomInitialize()
		{

            InitializeInput();

            InitializeActions();

            CurrentMovementType = MovementType.Ground;
		}

        private void InitializeActions()
        {
            var maxheight = MainSprite.Height;
            var relativeY = MainSprite.RelativeY;
            

            JumpAction = LandedAction = () => {

                if (!squishing)
                {
                    squishing = true;

                    var tweener = new Tweener(maxheight / 5.0f, maxheight, .35f, InterpolationType.Bounce, Easing.Out)
                    {
                        Owner = this
                    };

                    var tweener2 = new Tweener(relativeY - (maxheight / 2.0f), relativeY, .35f, InterpolationType.Bounce, Easing.Out)
                    {
                        Owner = this
                    };

                    tweener.PositionChanged += height => MainSprite.Height = height;
                    tweener2.PositionChanged += relativey => MainSprite.RelativeY = relativey;

                    TweenerManager.Self.Add(tweener);
                    TweenerManager.Self.Add(tweener2);

                    tweener.Start();
                    tweener2.Start();

                    tweener.Ended += () => squishing = false;
                }
            };
        }

        /// <summary>
        /// Sets the HorizontalInput and JumpInput instances to either the keyboard or 
        /// Xbox360GamePad index 0. This can be overridden by base classes to default
        /// to different input devices.
        /// </summary>
        protected virtual void InitializeInput()
        {
            this.HorizontalInput = new Multiple1DInputs();
            if (FlatRedBall.Input.InputManager.Xbox360GamePads[0].IsConnected)
            {
                this.JumpInput =
                    FlatRedBall.Input.InputManager.Xbox360GamePads[0].GetButton(FlatRedBall.Input.Xbox360GamePad.Button.A);

                HorizontalInput.Inputs.Add(FlatRedBall.Input.InputManager.Xbox360GamePads[0].LeftStick.Horizontal);
                HorizontalInput.Inputs.Add(FlatRedBall.Input.InputManager.Xbox360GamePads[0].DPadHorizontal);
            }
            else
            {
                this.JumpInput =
                    FlatRedBall.Input.InputManager.Keyboard.GetKey(Keys.Space);
                HorizontalInput.Inputs.Add(FlatRedBall.Input.InputManager.Keyboard.Get1DInput(Keys.Left, Keys.Right));
                FallThroughInput = InputManager.Keyboard.GetKey(Keys.Down);
            }

            InputEnabled = true;
        }

		private void CustomActivity()
		{
            ApplyInput();

            DetermineMovementValues();

            ApplyAnimation();
		}

        private void ApplyAnimation()
        {
            if (CurrentMovementType == MovementType.Ground)
            {
                if (Math.Abs(this.XVelocity) < .00001f)
                {
                    this.CurrentAnimationState = DirectionFacing == HorizontalDirection.Right ? Animation.StandRight : Animation.StandLeft;
                }
                else
                {
                    this.CurrentAnimationState = DirectionFacing == HorizontalDirection.Right ? Animation.WalkRight : Animation.WalkLeft;
                }
            }
            else
            {
                this.CurrentAnimationState = DirectionFacing == HorizontalDirection.Right ? Animation.StandRight : Animation.StandLeft;
            }
        }

        /// <summary>
        /// Reads all input and applies the read-in values to control
        /// velocity and character state.
        /// </summary>
        private void ApplyInput()
        {
            ApplyHorizontalInput();

            ApplyJumpInput();

            ApplyFallThroughInput();
        }

        private void ApplyFallThroughInput()
        {
            if (FallThroughInput.IsDown)
            {
                IsFallingThroughClouds = true;
            }

            
        }

        /// <summary>
        /// Applies the horizontal input to control horizontal movement and state.
        /// </summary>
        private void ApplyHorizontalInput()
        {
            float horizontalRatio = HorizontalRatio;


            if(horizontalRatio > 0)
            {
                mDirectionFacing = HorizontalDirection.Right;
            }
            else if(horizontalRatio < 0)
            {
                mDirectionFacing = HorizontalDirection.Left;
            }

            if (this.CurrentMovement.AccelerationTimeX <= 0)
            {
                this.XVelocity = horizontalRatio * CurrentMovement.MaxSpeedX;
            }
            else
            {
                float acceleration = CurrentMovement.MaxSpeedX / CurrentMovement.AccelerationTimeX;

                float sign = Math.Sign(horizontalRatio);
                float magnitude = Math.Abs(horizontalRatio);

                if(sign == 0)
                {
                    sign = -Math.Sign(XVelocity);
                    magnitude = 1;
                }

                if (XVelocity == 0 || sign == Math.Sign(XVelocity))
                {
                    this.XAcceleration = sign * magnitude * CurrentMovement.MaxSpeedX / CurrentMovement.AccelerationTimeX;
                }
                else
                {
                    float accelerationValue = magnitude * CurrentMovement.MaxSpeedX / CurrentMovement.DecelerationTimeX;


                    if (Math.Abs(XVelocity) < accelerationValue * TimeManager.SecondDifference)
                    {
                        this.XAcceleration = 0;
                        this.XVelocity = 0;
                    }
                    else
                    {

                        // slowing down
                        this.XAcceleration = sign * accelerationValue;
                    }

                }

                XVelocity = Math.Min(XVelocity, CurrentMovement.MaxSpeedX);
                XVelocity = Math.Max(XVelocity, -CurrentMovement.MaxSpeedX);
            }
        }

        /// <summary>
        /// Applies the jump input to control vertical velocity and state.
        /// </summary>
        private void ApplyJumpInput()
        {
			bool jumpPushed = JumpInput.WasJustPressed && InputEnabled;
			bool jumpDown = JumpInput.IsDown && InputEnabled;


            if (jumpPushed && 
                CurrentMovement.JumpVelocity > 0 &&
                (IsOnGround || AfterDoubleJump == null || 
				(AfterDoubleJump != null && mHasDoubleJumped == false) ||
				(AfterDoubleJump != null && AfterDoubleJump.JumpVelocity > 0)

				)
                
                )
            {

                mTimeJumpPushed = CurrentTime;
                this.YVelocity = CurrentMovement.JumpVelocity;
                mValuesJumpedWith = CurrentMovement;

                if (JumpAction != null)
                {
                    JumpAction();
                }

                if (CurrentMovementType == MovementType.Air)
                {
                    mHasDoubleJumped = true ;
                }
            }

            double secondsSincePush = CurrentTime - mTimeJumpPushed;

            if (mValuesJumpedWith != null && 
                secondsSincePush < mValuesJumpedWith.JumpApplyLength &&
				(mValuesJumpedWith.JumpApplyByButtonHold == false || JumpInput.IsDown)
                )
            {
                this.YVelocity = mValuesJumpedWith.JumpVelocity;

            }

            if (mValuesJumpedWith != null && mValuesJumpedWith.JumpApplyByButtonHold &&
				(!JumpInput.IsDown || mHitHead)
                )
            {
                mValuesJumpedWith = null;
            }

            this.YVelocity = Math.Max(-CurrentMovement.MaxFallSpeed, this.YVelocity);
        }

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        

        /// <summary>
        /// Assigns the current movement values based off of whether the user is on ground and has double-jumped or not.
        /// This is called automatically, but it can be overridden in derived classes to perform custom assignment of 
        /// movement types.
        /// </summary>
        protected virtual void DetermineMovementValues()
        {
            if (IsOnGround)
            {
                mHasDoubleJumped = false;
                if (CurrentMovementType == MovementType.Air ||
                    CurrentMovementType == MovementType.AfterDoubleJump)
                {
                    CurrentMovementType = MovementType.Ground;
                }
            }
            else
            {
                if (CurrentMovementType == MovementType.Ground)
                {
                    CurrentMovementType = MovementType.Air;
                }

            }

            if (CurrentMovementType == MovementType.Air && mHasDoubleJumped)
            {
                CurrentMovementType = MovementType.AfterDoubleJump;
            }
        


        }

        /// <summary>
        /// Assigns animations based on the current movement values. This must be explicitly called or else
        /// animations won't be assigned.
        /// </summary>
        /// <param name="animations">The AnimationChainList containing all of the character's animations.</param>
        public virtual void SetAnimations(AnimationChainList animations)
        {
            if(this.MainSprite != null)
            {
                string chainToSet = GetChainToSet();

                if(!string.IsNullOrEmpty(chainToSet))
                {
                    bool differs = MainSprite.CurrentChainName == null ||
                        MainSprite.CurrentChainName != chainToSet;

                    if(differs)
                    {
                        this.MainSprite.SetAnimationChain(animations[chainToSet]);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the name of the animation chain to use. This contains standard
        /// behavior for walking, jumping, falling, and standing. This can be overridden
        /// in derived classes to support more types of animation. This is called by SetAnimation.
        /// </summary>
        /// <returns>The name of the animation to use.</returns>
        protected virtual string GetChainToSet()
        {
            string chainToSet = null;

            if (IsOnGround == false)
            {
                if (mDirectionFacing == HorizontalDirection.Right)
                {
                    if (this.YVelocity > 0)
                    {
                        chainToSet = "JumpRight";
                    }
                    else
                    {
                        chainToSet = "FallRight";
                    }
                }
                else
                {
                    if (this.YVelocity > 0)
                    {
                        chainToSet = "JumpLeft";
                    }
                    else
                    {
                        chainToSet = "FallLeft";
                    }
                }

            }
            else if (HorizontalRatio != 0)
            {
                if (mDirectionFacing == HorizontalDirection.Right)
                {
                    chainToSet = "WalkRight";
                }
                else
                {
                    chainToSet = "WalkLeft";
                }
            }
            else
            {
                if (mDirectionFacing == HorizontalDirection.Right)
                {
                    chainToSet = "StandRight";
                }
                else
                {
                    chainToSet = "StandLeft";
                }
            }
            return chainToSet;
        }

	}
}
