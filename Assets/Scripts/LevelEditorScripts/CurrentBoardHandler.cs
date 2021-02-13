using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentBoardHandler : MonoBehaviour
{
    public bool[,] Cb = new bool[32, 32];

    public Material lightSquare, darkSquare;
    public GameObject squareSprite;

    void Start()
    {
        DrawSquare(lightSquare, new Vector2(0, 0));
    }

    public void RedrawBoard()
    {
       
    }

    public void DrawSquare(Material colour, Vector2 pos)
    {
        GameObject square = Instantiate(squareSprite, pos, Quaternion.Euler(90, 0, 0), this.transform);
    }
}
