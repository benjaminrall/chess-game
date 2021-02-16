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

        public bool spawnsPiece;
        public int pieceType;
        public int pieceDirection;
        public int pieceColour;
        public GameObject pieceReference;
    }
    [System.Serializable]
    public struct PieceGameobject
    {
        public GameObject piece;
        public float scale;
    }
    [System.Serializable]
    public struct PieceMaterials
    {
        public Material Primary;
        public Material Secondary;
        public Material Tertiary;
    }

    public ChessSquare[,] Cb = new ChessSquare[32, 32];

    public Material lightSquare, darkSquare;
    public GameObject squareSprite;
    public Material[] squareTypeMats;

    public PieceGameobject[] spawnedPiecePrefabs;
    public PieceMaterials[] spawnedPieceColours;

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

    public void AddPiece(Vector2 pos, int type, int dir, int colour)
    {
        if (pos.x <= 0 || pos.x >= 31) return;
        if (pos.y <= 0 || pos.y >= 31) return;
        if (!Cb[(int)pos.x, (int)pos.y].isActive) return;

        Cb[(int)pos.x, (int)pos.y].spawnsPiece = true;
        Cb[(int)pos.x, (int)pos.y].pieceType = type;
        Cb[(int)pos.x, (int)pos.y].pieceDirection = dir;
        Cb[(int)pos.x, (int)pos.y].pieceColour = colour;
        RedrawBoard();
    }
    public void RemovePiece(Vector2 pos)
    {
        if (pos.x <= 0 || pos.x >= 31) return;
        if (pos.y <= 0 || pos.y >= 31) return;

        Cb[(int)pos.x, (int)pos.y].spawnsPiece = false;
        RedrawBoard();
    }

    public void RedrawBoard()
    {
        for (int file = 0; file < 32; file++){
            for(int rank = 0; rank < 32; rank++){
                if (Cb[rank,file].isActive == false && Cb[rank, file].reference == null) continue;
                //if (Cb[rank, file].isActive == true && Cb[rank, file].reference != null) continue;

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

                if(Cb[rank, file].squareType == -1)
                {
                    if (Cb[rank, file].squareType == -1)
                    {
                        if ((file + rank) % 2 != 0) Cb[rank, file].squareType = 0;
                        else Cb[rank, file].squareType = 1;
                    }
                }
                if(Cb[rank, file].reference != null) if (Cb[rank, file].reference.GetComponent<SpriteRenderer>().material != squareTypeMats[Cb[rank, file].squareType]) Cb[rank, file].reference.GetComponent<SpriteRenderer>().material = squareTypeMats[Cb[rank, file].squareType];

                if(Cb[rank, file].spawnsPiece && Cb[rank, file].pieceReference == null)
                {
                    Vector2 position = new Vector2(file, rank);
                    Cb[rank, file].pieceReference = DrawPiece(position, Cb[rank, file].pieceType, Cb[rank, file].pieceColour);
                    //Debug.Log(Cb[rank, file]);
                }
                else if(!Cb[rank, file].spawnsPiece && Cb[rank, file].pieceReference != null)
                {
                    Destroy(Cb[rank, file].pieceReference);
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

    public GameObject DrawPiece(Vector2 pos, int type, int colour)
    {
        GameObject piece = Instantiate(spawnedPiecePrefabs[type].piece, new Vector3(pos.x, 1, pos.y), Quaternion.Euler(0, 0, 0), this.transform);
        piece.transform.GetChild(0).GetComponent<MeshRenderer>().material = spawnedPieceColours[colour].Primary;
        piece.transform.GetChild(1).GetComponent<MeshRenderer>().material = spawnedPieceColours[colour].Secondary;
        piece.transform.GetChild(2).GetComponent<MeshRenderer>().material = spawnedPieceColours[colour].Tertiary;
        piece.transform.localScale = new Vector3(spawnedPiecePrefabs[type].scale, 1, spawnedPiecePrefabs[type].scale);
        return piece;
    }
}