using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDrag : MonoBehaviour
{
    private int attemptedX;
    private int attemptedY;

    private Piece piece;
    private BoardHandlerScript BHS;

    void Start()
    {
        BHS = GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>();
        piece = gameObject.GetComponent<Piece>();
    }

    void Update()
    {
        attemptedX = Mathf.RoundToInt(transform.position.x);
        attemptedY = Mathf.RoundToInt(transform.position.z);
    }

    public void DragPiece(GameObject cursor)
    {
        transform.position = new Vector3(cursor.transform.position.x, this.transform.position.y, cursor.transform.position.z);
    }

    public void DropPiece()
    {
        if (piece.checkIsValidMove(attemptedX, attemptedY))
        {
            transform.position = new Vector3(attemptedX, this.transform.position.y, attemptedY);
            BHS.turn = (BHS.turn + 1) % BHS.players;
            OriginalPos();
            BHS.UpdateAvailableSpaces();
        }
        else
        {
            transform.position = new Vector3(piece.pieceX, this.transform.position.y, piece.pieceY);
        }
        BHS.ShowIndicators(false, new List<(int x, int y)>());
    }

    public void OriginalPos()
    {
        piece.pieceX = attemptedX;
        piece.pieceY = attemptedY;
    }

}
