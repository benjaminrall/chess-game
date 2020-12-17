using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceInstantiatorScript : MonoBehaviour
{
    public PieceLayout layout;
    private BoardHandlerScript BHS;

    void Start()
    {
        InstantiatePieces();
    }

    public void InstantiatePieces()
    {    
        BHS = GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>();  
        for (int i = 0; i < layout.LayoutsPieces1.Length; i++)
        {
            GameObject piece = Instantiate(layout.LayoutsPieces1[i].Piece, new Vector3(layout.LayoutsPieces1[i].Location.x, 1, layout.LayoutsPieces1[i].Location.y), layout.LayoutsPieces1[i].Piece.transform.rotation);
            piece.transform.SetParent(BHS.transform);
            piece.GetComponent<Piece>().colour = layout.colour1;
            if (piece.GetComponent<Pawn>())
            {
                piece.GetComponent<Pawn>().direction = layout.direction1;
            }
            if (piece.GetComponent<King>())
            {
                piece.GetComponent<King>().direction = layout.direction1;
            }
            piece.GetComponent<Piece>().Setup();
        }

        for (int z = 0; z < layout.LayoutsPieces2.Length; z++)
        {
            GameObject piece = Instantiate(layout.LayoutsPieces2[z].Piece, new Vector3(layout.LayoutsPieces2[z].Location.x, 1, layout.LayoutsPieces2[z].Location.y), layout.LayoutsPieces2[z].Piece.transform.rotation);
            piece.transform.SetParent(BHS.transform);
            piece.GetComponent<Piece>().colour = layout.colour2;
            if (piece.GetComponent<Pawn>())
            {
                piece.GetComponent<Pawn>().direction = layout.direction2;
            }
            if (piece.GetComponent<King>())
            {
                piece.GetComponent<King>().direction = layout.direction2;
            }
            piece.GetComponent<Piece>().Setup();
        }
        BHS.Setup();
        BHS.UpdateAvailableSpaces();
    }
}
