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

        private Menu _gameOverMenu;
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

            paddles = new Paddle[]
            {
                new Paddle(350, 70, paddleWidth, paddleHeight, paddleSpeed),  // Top
                new Paddle(350, 510, paddleWidth, paddleHeight, paddleSpeed), // Bottom
                new Paddle(70, 250, paddleHeight, paddleWidth, paddleSpeed),  // Left
                new Paddle(710, 250, paddleHeight, paddleWidth, paddleSpeed)  // Right
            };

            borders = new Border[]
            {
                new Border(new Rectangle(50, 50, 700, borderWidth), GraphicsDevice),   // Top border
                new Border(new Rectangle(50, 540, 700, borderWidth), GraphicsDevice),  // Bottom border
                new Border(new Rectangle(50, 50, borderWidth, 500), GraphicsDevice),   // Left border
                new Border(new Rectangle(740, 50, borderWidth, 500), GraphicsDevice)   // Right border
            };


            scores = new int[4]
            {
                10, 10, 10, 10
            };
            ball = new Ball(400, 300, ballSize, ballSpeed, speedUp);

            // ResetBall();
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

            _gameOverMenu = new Menu(
                font,
                new Vector2(300, 200),
                new List<string> { "Restart", "Quit" },
                RestartGame,
                ExitGame
            );
        }

        protected override void Update(GameTime gameTime)
        {
            if (scores.Any(score => score <= 0))
            {
                _isGameOver = true;
                _gameOverMenu.IsVisible = true;
            }

            if (_isGameOver)
            {
                _gameOverMenu.Update(); // Update the menu if it's active
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                // Move paddles
                paddles[0].MoveHorizontal(Keys.A, Keys.D, 55, 730, ball, paddles, borders);
                paddles[1].MoveHorizontal(Keys.Left, Keys.Right, 55, 730, ball, paddles, borders);
                paddles[2].Move(Keys.W, Keys.S, 55, 530, ball, paddles, borders);
                paddles[3].Move(Keys.Up, Keys.Down, 55, 530, ball, paddles, borders);

                // Update ball
                ball.Update(borders, paddles, ref scores);

                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            if (_isGameOver)
            {
                _gameOverMenu.Draw(_spriteBatch);
            }

            foreach (var border in borders)
                border.Draw(_spriteBatch, Color.White); 

            // Draw paddles
            foreach (var paddle in paddles)
                paddle.Draw(_spriteBatch, paddleTexture);

            // Draw ball
            ball.Draw(_spriteBatch, ballTexture);

            _spriteBatch.DrawString(font, scores[0].ToString(), new Vector2(380, 20), Color.White);
            _spriteBatch.DrawString(font, scores[1].ToString(), new Vector2(380, 570), Color.White);
            _spriteBatch.DrawString(font, scores[2].ToString(), new Vector2(20, 280), Color.White);
            _spriteBatch.DrawString(font, scores[3].ToString(), new Vector2(770, 280), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void RestartGame()
        {
            scores = new int[4]
            {
                10, 10, 10, 10
            };
            _isGameOver = false;
            _gameOverMenu.IsVisible = false;
        }

        private void ExitGame()
        {
            Exit();
        }
    
    }
}