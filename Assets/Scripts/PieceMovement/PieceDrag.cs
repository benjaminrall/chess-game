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

        if(gameObject.transform.childCount > 0)
        {
            gameObject.transform.Find("PrimaryColour").gameObject.GetComponent<Renderer>().material = BHS.piecePrimaryColours[GetComponent<Piece>().colour];
            gameObject.transform.Find("SecondaryColour").gameObject.GetComponent<Renderer>().material = BHS.pieceSecondaryColours[GetComponent<Piece>().colour];
            gameObject.transform.Find("TertiaryColour").gameObject.GetComponent<Renderer>().material = BHS.pieceTertiaryColours[GetComponent<Piece>().colour];
        }
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
            OriginalPos();
            StartCoroutine(CheckAll(BHS.turn));
        }
        else
        {
            transform.position = new Vector3(piece.pieceX, this.transform.position.y, piece.pieceY);
        }
        BHS.ShowIndicators(false, new List<(int x, int y)>());
        
    }

    IEnumerator CheckAll(int turn){
        BHS.turn = -1;
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>().UpdateAvailableSpaces();
        GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>().CheckForEnd();
        BHS.turn = (turn + 1) % BHS.players;
    }

    public void OriginalPos()
    {
        piece.pieceX = attemptedX;
        piece.pieceY = attemptedY;
    }

}
