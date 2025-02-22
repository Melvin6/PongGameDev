using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Paddle : CollidableObject
{
    public Vector2 Position;
    private int Height;
    private int Width;
    private int speed;
    private bool Ai;

    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

    public Paddle(int x, int y, int width, int height, int speed)
    {
        Position = new Vector2(x, y);
        this.Width = width;
        this.Height = height;
        this.speed = speed;
        this.Ai = true;
    }

    public void Move(Keys upKey, Keys downKey, int minY, int maxY, Ball ball, Paddle[] paddles, Border[] borders)
    {
        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(upKey) || state.IsKeyDown(downKey))
            Ai = false;

        if (!Ai)
        {
            Vector2 nextPos = Position;

            if (state.IsKeyDown(upKey) && Bounds.Y > minY)
                nextPos.Y -= speed;
            if (state.IsKeyDown(downKey) && Bounds.Y < maxY - Bounds.Height)
                nextPos.Y += speed;

            Rectangle nextBounds = new Rectangle((int)nextPos.X, (int)nextPos.Y, Bounds.Width, Bounds.Height);

            foreach (Paddle paddle in paddles)
            {
                if (paddle != this && nextBounds.Intersects(paddle.Bounds))
                {
                    return;
                }
            }

            foreach (Border border in borders)
            {
                if (nextBounds.Intersects(border.Bounds))
                {
                    return;
                }
            }

            Position = nextPos;
        }
        else
        {
            AiMoveY(ball, minY, maxY, paddles, borders);
        }
    }

    public void MoveHorizontal(Keys leftKey, Keys rightKey, int minX, int maxX, Ball ball, Paddle[] paddles, Border[] borders)
    {
        KeyboardState state = Keyboard.GetState();
        if (state.IsKeyDown(leftKey) || state.IsKeyDown(rightKey))
            Ai = false;

        if (!Ai)
        {
            Vector2 nextPos = Position;

            if (state.IsKeyDown(leftKey) && Bounds.X > minX)
                nextPos.X -= speed;
            if (state.IsKeyDown(rightKey) && Bounds.X < maxX - Bounds.Width)
                nextPos.X += speed;

            Rectangle nextBounds = new Rectangle((int)nextPos.X, (int)nextPos.Y, Bounds.Width, Bounds.Height);
            foreach (Paddle paddle in paddles)
            {
                if (paddle != this && nextBounds.Intersects(paddle.Bounds))
                {
                    return; 
                }
            }

            foreach (Border border in borders)
            {
                if (nextBounds.Intersects(border.Bounds))
                {
                    return;
                }
            }
            Position = nextPos;
        }
        else
        {
            AiMoveX(ball, minX, maxX, paddles, borders);
        }
    }

    public void AiMoveY(Ball ball, int minY, int maxY, Paddle[] paddles, Border[] borders)
    {
        float ballCenter = ball.Position.Y + ball.size / 2;
        int paddleCenter = Bounds.Y + Bounds.Height / 2;

        float distance = Math.Abs(ballCenter - paddleCenter);
        int speed = (int)Math.Round(Math.Min(distance * 1f, this.speed));

        speed = (int)Math.Max(speed, 0.5f);

        Vector2 nextPos = Position;
        if (ballCenter < paddleCenter)
            nextPos.Y -= speed;
        else
            nextPos.Y += speed;

        Rectangle nextBounds = new Rectangle((int)nextPos.X, (int)nextPos.Y, Bounds.Width, Bounds.Height);
        foreach(Paddle paddle in paddles){
            if (paddle != this && nextBounds.Intersects(paddle.Bounds)){
                return;
            }
        }

        foreach(Border border in borders){
            if (nextBounds.Intersects(border.Bounds)){
                return;
            }
        }

        Position = nextPos;
    }


    public void AiMoveX(Ball ball, int minX, int maxX, Paddle[] paddles, Border[] borders)
    {
        float ballCenter = ball.Position.X + ball.size / 2;
        int paddleCenter = Bounds.X + Bounds.Width / 2;

        float distance = Math.Abs(ballCenter - paddleCenter);
        int speed = (int)Math.Round(Math.Min(distance * 1f, this.speed));

        speed = (int)Math.Max(speed, 0.5f);
        Vector2 nextPos = Position;
        if (ballCenter < paddleCenter)
            nextPos.X -= speed;
        else
            nextPos.X += speed;

        Rectangle nextBounds = new Rectangle((int)nextPos.X, (int)nextPos.Y, Bounds.Width, Bounds.Height);
        
        foreach(Paddle paddle in paddles){
            if (paddle != this && nextBounds.Intersects(paddle.Bounds)){
                return;
            }
        }

        foreach(Border border in borders){
            if (nextBounds.Intersects(border.Bounds)){
                return;
            }
        }

        Position = nextPos;
    }

    private void ResolveOverlapY(Rectangle otherBounds, ref Vector2 nextPos)
    {
        // Calculate overlap on the Y-axis
        float overlapTop = nextPos.Y + Bounds.Height - otherBounds.Top;
        float overlapBottom = otherBounds.Bottom - nextPos.Y;

        // Determine the smallest overlap to resolve
        if (overlapTop < overlapBottom)
        {
            // Push the paddle up
            nextPos.Y -= overlapTop;
        }
        else
        {
            // Push the paddle down
            nextPos.Y += overlapBottom;
        }
    }

    private void ResolveOverlapX(Rectangle otherBounds, ref Vector2 nextPos)
    {
        // Calculate overlap on the X-axis
        float overlapLeft = nextPos.X + Bounds.Width - otherBounds.Left;
        float overlapRight = otherBounds.Right - nextPos.X;

        // Determine the smallest overlap to resolve
        if (overlapLeft < overlapRight)
        {
            // Push the paddle to the left
            nextPos.X -= overlapLeft;
        }
        else
        {
            // Push the paddle to the right
            nextPos.X += overlapRight;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
        spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color.White);
    }
}
