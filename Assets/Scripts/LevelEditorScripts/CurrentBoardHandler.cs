using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentBoardHandler : MonoBehaviour
{
    public struct ChessSquare
    {
        public bool isActive;
        public int squareType;
        public GameObject reference;
    }

    public ChessSquare[,] Cb = new ChessSquare[32, 32];

    public Material lightSquare, darkSquare;
    public GameObject squareSprite;
    public Material[] squareTypeMats;

    void Start()
    {

    }

    public void AddSquare(Vector2 pos, int type)
    {
        if (pos.x <= 0 || pos.x >= 31) return;
        if (pos.y <= 0 || pos.y >= 31) return;

        Cb[(int)pos.x, (int)pos.y].isActive = true;
        Cb[(int)pos.x, (int)pos.y].squareType = type;
        RedrawBoard();
    }
    public void RemoveSquare(Vector2 pos)
    {
        if (pos.x <= 0 || pos.x >= 31) return;
        if (pos.y <= 0 || pos.y >= 31) return;

        Cb[(int)pos.x, (int)pos.y].isActive = false;
        Cb[(int)pos.x, (int)pos.y].squareType = -1;
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
                    Material squareColour;
                    if (Cb[rank, file].squareType == -1)
                    {
                        if ((file + rank) % 2 != 0) Cb[rank, file].squareType = 0;
                        else Cb[rank, file].squareType = 1;
                    }
                    squareColour = squareTypeMats[Cb[rank, file].squareType];

                    Vector2 position = new Vector2(file, rank);
                    Cb[rank, file].isActive = true;
                    Cb[rank, file].reference = DrawSquare(squareColour, position);
                }
                else if(Cb[rank, file].isActive == false && Cb[rank, file].reference != null)
                {
                    Destroy(Cb[rank, file].reference);
                    Cb[rank, file].reference = null;
                    Cb[rank, file].squareType = -1;
                }

                if (Cb[rank, file].reference.GetComponent<SpriteRenderer>().material != squareTypeMats[Cb[rank, file].squareType])
                {
                    Cb[rank, file].reference.GetComponent<SpriteRenderer>().material = squareTypeMats[Cb[rank, file].squareType];
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
