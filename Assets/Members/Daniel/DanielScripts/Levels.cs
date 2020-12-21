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
                    (0, new Vector2(-4, 0)),
                    (0, new Vector2(4, 0))
                };
                break;
            case 2:
                enemies = new List<(int, Vector2)>
                {
                    (0, new Vector2(-4, 0)),
                    (0, new Vector2(4, 0)),
                    (0, new Vector2(0, 4)),
                    (0, new Vector2(0, -4))
                };
                break;
            default:
                break;
        }

        return enemies;
    }  
}
