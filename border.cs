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
}


