using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDrag : MonoBehaviour
{
    private int attemptedX;
    private int attemptedY;

    private Piece piece;
    private BoardHandlerScript BHS;
    private AudioManager audioPlayer;

    void Start()
    {
        BHS = GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>();
        audioPlayer = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();
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
            piece.pieceX = attemptedX;
            piece.pieceY = attemptedY;
            if (piece.gameObject.GetComponent<Pawn>()){
                if (piece.gameObject.GetComponent<Pawn>().promoted){
                    // if piece is being promoted 
                    // this is currently shite because it's to bypass te piece being deactivated too quickly
                    // idea is that from ui you get index to promotionPieces list in pawns to get whatever piece you want to instantiate
                    StartCoroutine(WaitForPromotion());
                    /*
                    foreach(Transform child in piece.transform){
                        child.gameObject.SetActive(false);
                    }                    
                    GameObject newPiece = Instantiate(piece.gameObject.GetComponent<Pawn>().promotionPieces[piece.gameObject.GetComponent<Pawn>().pieceUpgrade], BHS.transform);
                    newPiece.transform.position = new Vector3(piece.pieceX, 1, piece.pieceY);
                    newPiece.GetComponent<Piece>().colour = piece.colour;
                    newPiece.GetComponent<Piece>().pieceX = piece.pieceX;
                    newPiece.GetComponent<Piece>().pieceY = piece.pieceY;   
                    StartCoroutine(CheckAll(BHS.turn, true));
                    audioPlayer.dropPiece();
                    BHS.ShowIndicators(false, new List<(int x, int y)>());
                    */
                    return;
                }
            }
            StartCoroutine(CheckAll(BHS.turn));
            audioPlayer.dropPiece();
        }
        else
        {
            transform.position = new Vector3(piece.pieceX, this.transform.position.y, piece.pieceY);
        }
        BHS.ShowIndicators(false, new List<(int x, int y)>());
    }

    IEnumerator CheckAll(int turn, bool kill = false){
        BHS.turn = -1;
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>().UpdateAvailableSpaces();
        GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>().CheckForEnd();
        BHS.turn = (turn + 1) % BHS.players;
        if (kill){
            piece.Kill(-1);
        }
    }

    public IEnumerator WaitForPromotion()
    {
        Debug.Log("Still -1");
        while (piece.gameObject.GetComponent<Pawn>().pieceUpgrade == -1)
            yield return null;
        Debug.Log("Past -1");
        foreach (Transform child in piece.transform)
        {
            child.gameObject.SetActive(false);
        }
        GameObject newPiece = Instantiate(piece.gameObject.GetComponent<Pawn>().promotionPieces[piece.gameObject.GetComponent<Pawn>().pieceUpgrade], BHS.transform);
        newPiece.transform.position = new Vector3(piece.pieceX, 1, piece.pieceY);
        newPiece.GetComponent<Piece>().colour = piece.colour;
        newPiece.GetComponent<Piece>().pieceX = piece.pieceX;
        newPiece.GetComponent<Piece>().pieceY = piece.pieceY;
        StartCoroutine(CheckAll(BHS.turn, true));
        audioPlayer.dropPiece();
        BHS.ShowIndicators(false, new List<(int x, int y)>());
        yield return true;
    }

    public void OriginalPos()
    {
        piece.pieceX = attemptedX;
        piece.pieceY = attemptedY;
    }

}
