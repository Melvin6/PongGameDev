using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class CollidableObject
{
    public bool CheckTunnelingCollision(Vector2 prevPos, Vector2 nextPos, Rectangle border, int size)
    {
        Vector2 prevPosAdjusted = new Vector2(prevPos.X + size / 2, prevPos.Y + size / 2);
        Vector2 nextPosAdjusted = new Vector2(nextPos.X + size / 2, nextPos.Y + size / 2);

        Vector2[] borderEdges = GetRectangleEdges(border);

        for (int i = 0; i < borderEdges.Length; i += 2)
        {
            if (DoLinesIntersect(prevPosAdjusted, nextPosAdjusted, borderEdges[i], borderEdges[i + 1]))
            {
                return true;
            }
        }
        return false;
    }


    private Vector2[] GetRectangleEdges(Rectangle rect)
    {
        return new Vector2[]
        {
            new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top),
            new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom),
            new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Left, rect.Bottom),
            new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Left, rect.Top) // Left edge
        };
    }

    private bool DoLinesIntersect(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
    {
        float d = (p2.X - p1.X) * (q2.Y - q1.Y) - (p2.Y - p1.Y) * (q2.X - q1.X);
        
        if (d == 0) return false;

        float u = ((q1.X - p1.X) * (q2.Y - q1.Y) - (q1.Y - p1.Y) * (q2.X - q1.X)) / d;
        float v = ((q1.X - p1.X) * (p2.Y - p1.Y) - (q1.Y - p1.Y) * (p2.X - p1.X)) / d;

        return (u >= 0 && u <= 1 && v >= 0 && v <= 1);
    }
}