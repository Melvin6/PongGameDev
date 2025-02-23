using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D paddleTexture, ballTexture;
        
        private int[] scores;

        private Paddle[] paddles;
        private int paddleWidth = 80, paddleHeight = 20, paddleSpeed = 10;

        private Ball ball;
        private int ballSize = 20;
        private float ballSpeed = 3f, speedUp = 1.1f;

        private SpriteFont font;

        private Border[] borders;
        private int borderWidth = 10;

        private Menu menu;
        private bool _isGameOver = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            paddles = new Paddle[] {
                new Paddle(350, 70, paddleWidth, paddleHeight, paddleSpeed),
                new Paddle(350, 510, paddleWidth, paddleHeight, paddleSpeed),
                new Paddle(70, 250, paddleHeight, paddleWidth, paddleSpeed),
                new Paddle(710, 250, paddleHeight, paddleWidth, paddleSpeed)
            };

            borders = new Border[] {
                new Border(new Rectangle(50, 50, 700, borderWidth), GraphicsDevice),
                new Border(new Rectangle(50, 540, 700, borderWidth), GraphicsDevice),
                new Border(new Rectangle(50, 50, borderWidth, 500), GraphicsDevice),
                new Border(new Rectangle(740, 50, borderWidth, 500), GraphicsDevice)
            };


            scores = new int[4] {
                10, 10, 10, 10
            };
            ball = new Ball(400, 300, ballSize, ballSpeed, speedUp);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            paddleTexture = new Texture2D(GraphicsDevice, 1, 1);
            paddleTexture.SetData(new Color[] { Color.White });
            ballTexture = new Texture2D(GraphicsDevice, 1, 1);
            ballTexture.SetData(new Color[] { Color.White });
            font = Content.Load<SpriteFont>("fonts/Arial");

            menu = new Menu(
                font,
                new Vector2(400, 200),
                new List<string> { "Resume", "Restart", "Quit" },
                OnRestart,
                OnQuit,
                OnResume
            );
        }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        if (scores.Any(score => score <= 0)) {
            _isGameOver = true;
            menu.CurrentState = Menu.GameState.GameOver;
            menu.IsVisible = true;
        }

        if (!_isGameOver && keyboardState.IsKeyDown(Keys.Escape)) {
            menu.CurrentState = Menu.GameState.Paused;
            menu.IsVisible = true;
        }
        menu.Update();

        if (menu.CurrentState == Menu.GameState.Playing && !_isGameOver) {
            paddles[0].MoveHorizontal(Keys.A, Keys.D, 55, 730, ball, paddles, borders);
            paddles[1].MoveHorizontal(Keys.Left, Keys.Right, 55, 730, ball, paddles, borders);
            paddles[2].Move(Keys.W, Keys.S, 55, 530, ball, paddles, borders);
            paddles[3].Move(Keys.Up, Keys.Down, 55, 530, ball, paddles, borders);

            ball.Update(borders, paddles, ref scores);
        }
        base.Update(gameTime);
    }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            if (_isGameOver) {
                menu.Draw(_spriteBatch);
            }

            if(menu.CurrentState != Menu.GameState.Playing)
                menu.Draw(_spriteBatch);

            foreach (var border in borders)
                border.Draw(_spriteBatch, Color.White); 

            foreach (var paddle in paddles)
                paddle.Draw(_spriteBatch, paddleTexture);

            ball.Draw(_spriteBatch, ballTexture);

            _spriteBatch.DrawString(font, scores[0].ToString(), new Vector2(380, 20), Color.White);
            _spriteBatch.DrawString(font, scores[1].ToString(), new Vector2(380, 570), Color.White);
            _spriteBatch.DrawString(font, scores[2].ToString(), new Vector2(20, 280), Color.White);
            _spriteBatch.DrawString(font, scores[3].ToString(), new Vector2(770, 280), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void OnRestart()
        {
            scores = new int[4] {
                10, 10, 10, 10
            };

            ball.Reset();
            
            _isGameOver = false;
            menu.CurrentState = Menu.GameState.Playing;
            menu.IsVisible = false;
        }

        private void OnQuit()
        {
            Exit();
        }

        void OnResume()
        {
            menu.CurrentState = Menu.GameState.Playing;
            menu.IsVisible = false;
        }
    
    }
}