using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LeoniV0_3
{
    /// <summary>
    /// Basic camera class supporting mouse/keyboard/gamepad-based movement.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Gets or sets the position of the camera.
        /// </summary>
        public Vector3 Position { get; set; }
        
        float yaw;
        float pitch;
        /// <summary>
        /// Gets or sets the yaw rotation of the camera.
        /// </summary>
        public float Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                yaw = MathHelper.WrapAngle(value);
            }
        }
        /// <summary>
        /// Gets or sets the pitch rotation of the camera.
        /// </summary>
        public float Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = MathHelper.Clamp(value, -MathHelper.PiOver2, MathHelper.PiOver2);
            }
        }

        /// <summary>
        /// Gets or sets the speed at which the camera moves.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets the view matrix of the camera.
        /// </summary>
        public Matrix ViewMatrix { get; private set; }
        /// <summary>
        /// Gets or sets the projection matrix of the camera.
        /// </summary>
        public Matrix ProjectionMatrix { get; set; }

        /// <summary>
        /// Gets the world transformation of the camera.
        /// </summary>
        public Matrix WorldMatrix { get; private set; }

        /// <summary>
        /// Gets the game owning the camera.
        /// </summary>
        public Game1 Game { get; private set; }

        /// <summary>
        /// Constructs a new camera.
        /// </summary>
        /// <param name="game">Game that this camera belongs to.</param>
        /// <param name="position">Initial position of the camera.</param>
        /// <param name="speed">Initial movement speed of the camera.</param>
        public Camera(Game1 game, Vector3 position, float speed)
        {
            Game = game;
            Position = position;
            Speed = speed;
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4f / 3f, .1f, 10000.0f);
            Mouse.SetPosition(200, 200);
        }

        /// <summary>
        /// Moves the camera forward using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        public void MoveForward(float dt)
        {
            Position += WorldMatrix.Forward * (dt * Speed);
        }
        /// <summary>
        /// Moves the camera right using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        /// 
        public void MoveRight(float dt)
        {
            Position += WorldMatrix.Right * (dt * Speed);
        }
        /// <summary>
        /// Moves the camera up using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        /// 
        public void MoveUp(float dt)
        {
            Position += new Vector3(0, (dt * Speed), 0);
        }

        /// <summary>
        /// Updates the camera's view matrix.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        public void Update(float dt)
        {

            if (Game.level > 2)
            {
                float thumby = -GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
                float thumbx = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X;

                float MouseX = 200;
                float MouseY = 200;

#if !XBOX
                MouseX = Game.MouseState.X;
                MouseY = Game.MouseState.Y;
#endif
                //Turn based on mouse input.
                Yaw += (200 - MouseX - thumbx * 60) * dt * .12f;
                Pitch += (200 - MouseY - thumby * 60) * dt * .12f;

                Mouse.SetPosition(200, 200);

#if !XBOX
                MouseX = Game.MouseState.X;
                MouseY = Game.MouseState.Y;
#endif
                float tempx = ((200 - MouseX - thumbx * 60) * dt * .12f) / 8;
                float tempy = ((200 - MouseY - thumby * 60) * dt * .12f) / 8;

                if (tempx < 0)
                    tempx = -tempx;
                if (tempy < 0)
                    tempy = -tempy;

                //Set Motion Blur
                (Game as Game1).blur = (tempx + tempy) / 2;
                if ((Game as Game1).forceBlur)
                    (Game as Game1).blur = 0.002f;
            }
                WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Yaw);


                float distance = Speed * dt;

                //Scoot the camera around depending on what keys are pressed.
                /*if (Game.KeyboardState.IsKeyDown(Keys.W))
                    MoveForward(distance);
                if (Game.KeyboardState.IsKeyDown(Keys.S))
                    MoveForward(-distance);
                if (Game.KeyboardState.IsKeyDown(Keys.A))
                    MoveRight(-distance);
                if (Game.KeyboardState.IsKeyDown(Keys.D))
                    MoveRight(distance);
                if (Game.KeyboardState.IsKeyDown(Keys.E))
                    MoveUp(distance);
                if (Game.KeyboardState.IsKeyDown(Keys.Q))
                    MoveUp(-distance);*/
            
            WorldMatrix = WorldMatrix * Matrix.CreateTranslation(Position);
            ViewMatrix = Matrix.Invert(WorldMatrix);
        }
    }
}
