using System.Collections.Generic;
using UnityEngine;

public class Directions : MonoBehaviour
{
    public readonly List<(int, int)> koordinates = new()
    {
        (0,  2),
        (2,  0),
        (0, -2),
        (-2, 0)
    };
}