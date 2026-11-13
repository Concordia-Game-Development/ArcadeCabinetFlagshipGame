using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Room Boundaries
public class RoomBoundaries : MonoBehaviour
{
    // The EdgeCollider2D
    public EdgeCollider2D edge = null;


    // Sets the room boundary size.
    public void SetSize(Vector2 size)
    {
        if (!edge)
        {
            return;
        }

        // Fills a List with the vertices positions.
        List<Vector2> points = new();
        points.Add(0.5f * new Vector2(size.x, size.y));
        points.Add(0.5f * new Vector2(size.x, -size.y));
        points.Add(0.5f * new Vector2(-size.x, -size.y));
        points.Add(0.5f * new Vector2(-size.x, size.y));
        points.Add(0.5f * new Vector2(size.x, size.y));

        // Sets the vertices.
        edge.SetPoints(points);
    }
}
