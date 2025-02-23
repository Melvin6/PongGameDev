using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
public class Ball : CollidableObject
{
    public Vector2 Position;
    public Vector2 Velocity;
    public int size {get; private set;}
    private float startSpeed;
    private float speed;
    private float speedUp;
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, size, size);

    public Ball(int x, int y, int size, float speed, float speedUp)
    {
        Position = new Vector2(x, y);
        Velocity = new Vector2(1, 1);
        this.size = size;
        this.startSpeed = speed;
        this.speed = speed;
        this.speedUp = speedUp;
        Velocity = Vector2.Normalize(Velocity) * speed;

        Reset();
    }

    public void Update(Border[] borders, Paddle[] paddles, ref int[] scores)
    {

        Vector2 previousPosition = Position;
        Vector2 nextPosition = Position + Velocity;

        foreach (var paddle in paddles)
        {
            if (Bounds.Intersects(paddle.Bounds) || CheckTunnelingCollision(previousPosition, nextPosition, paddle.Bounds, size)) {
                PushAway(paddle.Bounds);

                if (paddle.Bounds.Width > paddle.Bounds.Height) {
                    float hitPosition = (Position.X + size / 2) - (paddle.Bounds.X + paddle.Bounds.Width / 2);
                    float normalizedHit = hitPosition / (paddle.Bounds.Width / 2);
                    Velocity.Y *= -1; 
                    Velocity.X += normalizedHit * 2f; 
                }
                else {
                    float hitPosition = (Position.Y + size / 2) - (paddle.Bounds.Y + paddle.Bounds.Height / 2);
                    float normalizedHit = hitPosition / (paddle.Bounds.Height / 2);
                    Velocity.X *= -1; 
                    Velocity.Y += normalizedHit * 10f; 
                }

                Velocity = Vector2.Normalize(Velocity) * speed;
                speed *= speedUp;
                return;
            }
        }

        for (int i = 0; i < borders.Length; i++)
        {
            if (Bounds.Intersects(borders[i].Bounds) || CheckTunnelingCollision(previousPosition, nextPosition, borders[i].Bounds, size)) {
                scores[i]--;
                Reset();
                return;
            }
        }

        Position = nextPosition;
    }

    public void Reset()
    {
        Random rand = new Random();

        Position = new Vector2(400, 300);
        speed = startSpeed;
        float angle = (float)(rand.NextDouble() * Math.PI * 2);
        Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
        spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, size, size), Color.White);
    }

    private void PushAway(Rectangle collider)
    {   
        float overlapX = Math.Min(Bounds.Right - collider.Left, collider.Right - Bounds.Left);
        float overlapY = Math.Min(Bounds.Bottom - collider.Top, collider.Bottom - Bounds.Top);

        if (overlapX < overlapY) {
            if (Velocity.X > 0) Position.X -= overlapX;
            else Position.X += overlapX;
        }
        else {
            if (Velocity.Y > 0) Position.Y -= overlapY;
            else Position.Y += overlapY;
        }
    }
}
