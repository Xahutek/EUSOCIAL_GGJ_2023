using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMetrics : MonoBehaviour
{
    public const float outerRadius = 0.5f;

    [HideInInspector] public const float innerRadius = outerRadius * 0.866025404f;

    public static Vector3[] corners = {
        new Vector3(0f, outerRadius,0f),
        new Vector3(innerRadius, 0.5f * outerRadius,0f),
        new Vector3(innerRadius, -0.5f * outerRadius,0f),
        new Vector3(0f, -outerRadius,0f),
        new Vector3(-innerRadius, -0.5f * outerRadius,0f),
        new Vector3(-innerRadius, 0.5f * outerRadius,0f)
    };
    public static Vector3Int[] HexCoordinateDir = {
        new Vector3Int(0, -1, 1),
        new Vector3Int(1, -1, 0),
        new Vector3Int(1, 0, -1),
        new Vector3Int(0, 1, -1),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, 0, 1)
    };
}
