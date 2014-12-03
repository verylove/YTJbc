using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameCore
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameEngine : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Texture2D _roomBg2D;
        Texture2D _fistLeft2D;
        Texture2D _fistRight2D;
        Vector2 _fistLeftLoc;
        Vector2 _fistRightLoc;
        Vector2 _centerPoint;

        public GameEngine()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            _centerPoint = GetWindowCenterPoint();

            //_fistLeftLoc.X = _centerPoint().X - _fistLeft2D.Width;
            _fistLeftLoc.X = -_fistLeft2D.Width;
            _fistLeftLoc.Y = _centerPoint.Y - _fistLeft2D.Height / 2;
            //_fistRightLoc.X =_centerPoint.X;
            _fistRightLoc.X = Window.Width;
            _fistRightLoc.Y = _centerPoint.Y - _fistRight2D.Height / 2;


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("spriteFont1");

            _roomBg2D = Content.Load<Texture2D>(GameResPath.RoomBgTexture);
            _fistLeft2D = Content.Load<Texture2D>(GameResPath.LeftFistTexture);
            _fistRight2D = Content.Load<Texture2D>(GameResPath.RightFistTexture);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }



            //if (_fistLeftLoc.Y + _fistLeft2D.Height > Window.Height)
            //{
            //    _fistLeftLoc.Y = 0;
            //}
            //_fistLeftLoc.Y += 20;

            //if (_fistRightLoc.Y + _fistRight2D.Height > Window.Height)
            //{
            //    _fistRightLoc.Y = 0;
            //}
            //_fistRightLoc.Y += 5;

            if (_fistLeftLoc.X < _centerPoint.X - _fistLeft2D.Width)
            {
                if (_fistLeftLoc.X + 5 > _centerPoint.X)
                {
                    _fistLeftLoc.X = _centerPoint.X;
                }
                else
                {
                    _fistLeftLoc.X += 5;
                }
                Console.WriteLine("left:" + _fistLeftLoc.X.ToString());
            }
            if (_fistRightLoc.X > _centerPoint.X)
            {

                if (_fistRightLoc.X + 5 < _centerPoint.X)
                {
                    _fistRightLoc.X = _centerPoint.X;
                }
                else
                {
                    _fistRightLoc.X -= 5;
                }

                Console.WriteLine("right:" + _fistRightLoc.X.ToString());
            }

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //spriteBatch.DrawString(font, "Hello from MonoGame!", new Vector2(16, 16), Color.RoyalBlue);
            spriteBatch.Draw(_roomBg2D, Vector2.Zero, new Rectangle(0, 0, Window.Width, Window.Height), Color.White);

            //◊Û»≠
            //_fistLoc.X =_centerPoint.X - _fistLeft2D.Width;
            //_fistLoc.Y =_centerPoint.Y - _fistLeft2D.Height / 2;
            spriteBatch.Draw(_fistLeft2D, _fistLeftLoc, Color.White);

            //”“»≠
            //_fistLoc.X =_centerPoint.X;
            //_fistLoc.Y =_centerPoint.Y - _fistRight2D.Height / 2;
            spriteBatch.Draw(_fistRight2D, _fistRightLoc, Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private Vector2 GetWindowCenterPoint()
        {
            Vector2 fistLoc = Vector2.Zero;

            int cx = Window.Width / 2;
            int cy = Window.Height / 2;

            fistLoc.X = cx;
            fistLoc.Y = cy;

            return fistLoc;
        }
    }
}
