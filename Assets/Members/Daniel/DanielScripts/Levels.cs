using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public List<(int, Vector2)> GetLevelData(int level)
    {
        List<(int, Vector2)> enemies = null;

        switch(level)
        {
            case 1:
                enemies = new List<(int, Vector2)>
                {
                    (0, new Vector2(9, 3)),
                    (3, new Vector2(0, -9))
                };
                break;
            case 2:
                enemies = new List<(int, Vector2)>
                {
                    (0, new Vector2(9, 3)),
                    (3, new Vector2(0, -9)),
                    (0, new Vector2(-1, -7))
                };
                break;
            case 3:
                enemies = new List<(int, Vector2)>
                {
                    (0, new Vector2(9, 3)),
                    (3, new Vector2(0, -9)),
                    (0, new Vector2(-1, -7)),
                    (1, new Vector2(10, 3))
                };
                break;
            case 4:
                enemies = new List<(int, Vector2)>
                {
                    (0, new Vector2(9, 3)),
                    (3, new Vector2(0, -9)),
                    (0, new Vector2(-1, -7)),
                    (1, new Vector2(10, 3)),
                    (1, new Vector2(23, -6))
                };
                break;
            case 5:
                enemies = new List<(int, Vector2)>
                {
                    (0, new Vector2(9, 3)),
                    (3, new Vector2(0, -9)),
                    (0, new Vector2(-1, -7)),
                    (1, new Vector2(10, 3)),
                    (1, new Vector2(23, -6)),
                    (0, new Vector2(2, -9))
                };
                break;
            case 6:
                enemies = new List<(int, Vector2)>
                {
                    (0, new Vector2(9, 3)),
                    (3, new Vector2(0, -9)),
                    (0, new Vector2(-1, -7)),
                    (1, new Vector2(10, 3)),
                    (1, new Vector2(23, -6)),
                    (0, new Vector2(2, -9)),
                    (2, new Vector2(22, -6))
                };
                break;
            case 7:
                enemies = new List<(int, Vector2)>
                {
                    (0, new Vector2(9, 3)),
                    (3, new Vector2(0, -9)),
                    (0, new Vector2(-1, -7)),
                    (1, new Vector2(10, 3)),
                    (1, new Vector2(23, -6)),
                    (0, new Vector2(2, -9)),
                    (2, new Vector2(22, -6)),
                    (2, new Vector2(11, 3))
                };
                break;
            case 8:
                enemies = new List<(int, Vector2)>
                {
                    (0, new Vector2(9, 3)),
                    (3, new Vector2(0, -9)),
                    (0, new Vector2(-1, -7)),
                    (1, new Vector2(10, 3)),
                    (1, new Vector2(23, -6)),
                    (0, new Vector2(2, -9)),
                    (2, new Vector2(22, -6)),
                    (2, new Vector2(11, 3)),
                    (1, new Vector2(21, -7)),
                    //(3, new Vector2(14, 4)),
                    //(2, new Vector2(15, 4)),
                    //(0, new Vector2(4, -10)),
                    //(0, new Vector2(5, -10)),
                    //(2, new Vector2(20, -8))
                };
                break;
            default:
                break;
        }

        return enemies;
    }  
}
