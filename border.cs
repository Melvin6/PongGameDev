using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Border
{
    public Rectangle Bounds { get; private set; }
    private Texture2D texture;

    public Border(Rectangle bounds, GraphicsDevice graphicsDevice)
    {
        Bounds = bounds;

        texture = new Texture2D(graphicsDevice, 1, 1);
        texture.SetData(new Color[] { Color.White });
    }

    public void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(texture, Bounds, color);
    }

    public bool Collision(Rectangle other)
    {
        if (Bounds.Intersects(other) || IsOnOtherSide(other, 800, 600))
            return true;
        else return false;
    }

    public bool IsOnOtherSide(Rectangle other, int screenWidth, int screenHeight)
    {
        Vector2 screenMidPoint = new Vector2(screenWidth / 2, screenHeight / 2);
        bool isWide = Bounds.Width > Bounds.Height;

        if (isWide)
        {
            float lineY = Bounds.Y + Bounds.Height / 2;
            return (screenMidPoint.Y < lineY && other.Y > lineY) || (screenMidPoint.Y > lineY && other.Y + other.Height < lineY);
        }
        else
        {
            float lineX = Bounds.X + Bounds.Width / 2;
            return (screenMidPoint.X < lineX && other.X > lineX) || (screenMidPoint.X > lineX && other.X + other.Width < lineX);
        }
    }
}


