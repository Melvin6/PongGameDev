using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Paddle
{
    public Rectangle Bounds; // Paddle's position and size
    private int speed;
    private bool Ai;

    public Paddle(int x, int y, int width, int height, int speed)
    {
        Bounds = new Rectangle(x, y, width, height);
        this.speed = speed;
        this.Ai = true;
    }

    public void Move(Keys upKey, Keys downKey, int minY, int maxY, Ball ball)
    {
        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(upKey) || state.IsKeyDown(downKey))
            Ai = false;
        if(!Ai)
        {
            if (state.IsKeyDown(upKey) && Bounds.Y > minY)
                Bounds.Y -= speed;
            if (state.IsKeyDown(downKey) && Bounds.Y < maxY - Bounds.Height)
                Bounds.Y += speed;
        }
        else{
            AiMoveY(ball, minY, maxY);
        }
    }

    public void AiMoveY(Ball ball, int minY, int maxY)
    {
        float ballCenter = ball.Position.Y + ball.size / 2;
        int paddleCenter = Bounds.Y + Bounds.Height / 2;

        float distance = Math.Abs(ballCenter - paddleCenter);
        int speed = (int)Math.Round(Math.Min(distance * 1f, this.speed));

        speed = (int)Math.Max(speed, 0.5f);
            if (ballCenter < paddleCenter)
                Bounds.Y -= speed;
            else
                Bounds.Y += speed;

        // Keep within bounds
        Bounds.Y = Math.Clamp(Bounds.Y, minY, maxY - Bounds.Height);
    }


    public void AiMoveX(Ball ball, int minX, int maxX)
    {
        float ballCenter = ball.Position.X + ball.size / 2;
        int paddleCenter = Bounds.X + Bounds.Width / 2;

        float distance = Math.Abs(ballCenter - paddleCenter);
        int speed = (int)Math.Round(Math.Min(distance * 1f, this.speed));

        speed = (int)Math.Max(speed, 0.5f);

            if (ballCenter < paddleCenter)
                Bounds.X -= speed;
            else
                Bounds.X += speed;

        Bounds.X = Math.Clamp(Bounds.X, minX, maxX - Bounds.Width);
    }


    public void MoveHorizontal(Keys leftKey, Keys rightKey, int minX, int maxX, Ball ball)
    {
        KeyboardState state = Keyboard.GetState();
        if (state.IsKeyDown(leftKey) || state.IsKeyDown(rightKey))
            Ai = false;
        if(!Ai)
        {
            if (state.IsKeyDown(leftKey) && Bounds.X > minX)
                Bounds.X -= speed;
            if (state.IsKeyDown(rightKey) && Bounds.X < maxX - Bounds.Width)
                Bounds.X += speed;
        }
        else
        {
            AiMoveX(ball, minX, maxX);
        }
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
        spriteBatch.Draw(texture, Bounds, Color.White);
    }
}
