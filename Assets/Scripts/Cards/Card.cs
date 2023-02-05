using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Card")]
public class Card : ScriptableObject
{
    public enum Type
    {
        Straight, Corner, Omen
    }
    public Type type;
    [Range(1,6)]public int range;

    public Sprite image;

    public List<Vector3Int> GetCoordinates(Vector3Int origin, out List<Vector3Int> path,int range=0)
    {
        if (range == 0) range = this.range;
        List<Vector3Int>
            coordinates = new List<Vector3Int>();
        path = new List<Vector3Int>();

        for (int i = 1; i <= range; i++)
        {
            switch (type)
            {
                case Type.Straight:
                    path.Add(origin + new Vector3Int(0, -1, 1) * i);
                    path.Add(origin + new Vector3Int(1, -1, 0) * i);
                    path.Add(origin + new Vector3Int(1, 0, -1) * i);
                    path.Add(origin + new Vector3Int(0, 1, -1) * i);
                    path.Add(origin + new Vector3Int(-1, 1, 0) * i);
                    path.Add(origin + new Vector3Int(-1, 0, 1) * i);
                    break;
                case Type.Corner:
                    path.Add(origin + new Vector3Int(0, -1, 1) + new Vector3Int(-1, 0, 1) * (i - 1));
                    path.Add(origin + new Vector3Int(1, -1, 0) + new Vector3Int(0, -1, 1) * (i - 1));
                    path.Add(origin + new Vector3Int(1, 0, -1) + new Vector3Int(1, -1, 0) * (i - 1));
                    path.Add(origin + new Vector3Int(0, 1, -1) + new Vector3Int(1, 0, -1) * (i - 1));
                    path.Add(origin + new Vector3Int(-1, 1, 0) + new Vector3Int(0, 1, -1) * (i - 1));
                    path.Add(origin + new Vector3Int(-1, 0, 1) + new Vector3Int(-1, 1, 0) * (i - 1));
                    break;
                default: break;
            }
            if (i==range)
                switch (type)
                {
                    case Type.Straight:
                        coordinates.Add(origin + new Vector3Int(0, -1, 1) * i);
                        coordinates.Add(origin + new Vector3Int(1, -1, 0) * i);
                        coordinates.Add(origin + new Vector3Int(1, 0, -1) * i);
                        coordinates.Add(origin + new Vector3Int(0, 1, -1) * i);
                        coordinates.Add(origin + new Vector3Int(-1, 1, 0) * i);
                        coordinates.Add(origin + new Vector3Int(-1, 0, 1) * i);
                        break;
                    case Type.Corner:
                        coordinates.Add(origin + new Vector3Int(0, -1, 1) + new Vector3Int(-1, 0, 1) * (i - 1));
                        coordinates.Add(origin + new Vector3Int(1, -1, 0) + new Vector3Int(0, -1, 1) * (i - 1));
                        coordinates.Add(origin + new Vector3Int(1, 0, -1) + new Vector3Int(1, -1, 0) * (i - 1));
                        coordinates.Add(origin + new Vector3Int(0, 1, -1) + new Vector3Int(1, 0, -1) * (i - 1));
                        coordinates.Add(origin + new Vector3Int(-1, 1, 0) + new Vector3Int(0, 1, -1) * (i - 1));
                        coordinates.Add(origin + new Vector3Int(-1, 0, 1) + new Vector3Int(-1, 1, 0) * (i - 1));
                        break;
                    default: break;
                }
        }

        return coordinates;

    }
}