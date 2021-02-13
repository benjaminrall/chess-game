using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentBoardHandler : MonoBehaviour
{
    public struct ChessSquare
    {
        public bool isActive;
        public GameObject reference;
    }

    public ChessSquare[,] Cb = new ChessSquare[32, 32];

    public Material lightSquare, darkSquare;
    public GameObject squareSprite;

    void Start()
    {

    }

    public void AddSquare(Vector2 pos)
    {
        if (pos.x <= 0 || pos.x >= 31) return;
        if (pos.y <= 0 || pos.y >= 31) return;

        Cb[(int)pos.x, (int)pos.y].isActive = true;
        RedrawBoard();
    }
    public void RemoveSquare(Vector2 pos)
    {
        if (pos.x <= 0 || pos.x >= 31) return;
        if (pos.y <= 0 || pos.y >= 31) return;

        Cb[(int)pos.x, (int)pos.y].isActive = false;
        RedrawBoard();
    }

    public void RedrawBoard()
    {
        for (int file = 0; file < 32; file++){
            for(int rank = 0; rank < 32; rank++){
                if (Cb[rank,file].isActive == false && Cb[rank, file].reference == null) continue;
                if (Cb[rank, file].isActive == true && Cb[rank, file].reference != null) continue;

                if (Cb[rank, file].isActive == true && Cb[rank, file].reference == null)
                {
                    bool isLightSquare = (file + rank) % 2 != 0;
                    Material squareColour = (isLightSquare) ? lightSquare : darkSquare;
                    Vector2 position = new Vector2(file, rank);
                    Cb[rank, file].isActive = true;
                    Cb[rank, file].reference = DrawSquare(squareColour, position);
                }
                else if(Cb[rank, file].isActive == false && Cb[rank, file].reference != null)
                {
                    Destroy(Cb[rank, file].reference);
                    Cb[rank, file].reference = null;
                }
            }
       }
    }

    public GameObject DrawSquare(Material colour, Vector2 pos)
    {
        GameObject square = Instantiate(squareSprite, new Vector3(pos.x,0,pos.y), Quaternion.Euler(-90, 0, 0), this.transform);
        square.GetComponent<SpriteRenderer>().material = colour;
        return square.gameObject;
    }
}
