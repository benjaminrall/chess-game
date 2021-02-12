using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int colour;
    [HideInInspector]
    public int pieceX;
    [HideInInspector]
    public int pieceY;
    [HideInInspector]
    public List<(int x, int y)> availableSpaces = new List<(int x, int y)>();
    [HideInInspector]
    public List<(int x, int y)> tempAvailableSpaces = new List<(int x, int y)>();
    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public int killedByColour;

    private bool real = true;
    
    private Transform pieces;
    protected BoardHandlerScript BHS;
    

    public virtual void Awake() {
        pieceX = Mathf.RoundToInt(transform.position.x);
        pieceY = Mathf.RoundToInt(transform.position.z);
    }

    public virtual void Setup(){
        pieces = GameObject.Find("BoardHandler").transform;
        BHS = GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>();
        pieceX = Mathf.RoundToInt(transform.position.x);
        pieceY = Mathf.RoundToInt(transform.position.z);
        //GetComponent<Renderer>().material = BHS.pieceColours[colour];
    }

    public virtual bool checkIsValidMove(int attemptedX, int attemptedY)
    {
        return false;
    }

    public bool PieceAt(int x, int y, int c = -1)
    {
        pieces = GameObject.Find("BoardHandler").transform;
        if (c == -1){
            foreach(Transform child in pieces)
            {
                if (child.gameObject.GetComponent<Piece>().pieceX == x && child.gameObject.GetComponent<Piece>().pieceY == y)
                {
                    return true;
                }
            }
            return false;
        }
        else{
            foreach(Transform child in pieces)
            {
                if (child.gameObject.GetComponent<Piece>().pieceX == x && child.gameObject.GetComponent<Piece>().pieceY == y && c != child.gameObject.GetComponent<Piece>().colour)
                {
                    return true;
                }
            }
            return false;          
        }
    }

    public void TakePieceAt(int x, int y, int c){
        pieces = GameObject.Find("BoardHandler").transform;
        foreach(Transform child in pieces)
        {
            if (child.gameObject.GetComponent<Piece>().pieceX == x && child.gameObject.GetComponent<Piece>().pieceY == y)
            {
                child.gameObject.GetComponent<Piece>().Kill(c);
            }
        }
    }

    protected void CheckSpaces(int min, int max, int xm, int ym, bool temp = false){
        if (!temp){
            for (int i = min; i <= max; i++){
                if (PieceAt(pieceX + (xm * i), pieceY + (ym * i), colour)){
                    availableSpaces.Add((pieceX + (xm * i), pieceY + (ym * i)));
                    return;
                }
                else if(PieceAt(pieceX + (xm * i), pieceY + (ym * i))){
                    return;
                }
                else{
                    availableSpaces.Add((pieceX + (xm * i), pieceY + (ym * i)));
                }
            }    
        }
        else{
            for (int i = min; i <= max; i++){
                if (PieceAt(pieceX + (xm * i), pieceY + (ym * i), colour)){
                    tempAvailableSpaces.Add((pieceX + (xm * i), pieceY + (ym * i)));
                    return;
                }
                else if(PieceAt(pieceX + (xm * i), pieceY + (ym * i))){
                    return;
                }
                else{
                    tempAvailableSpaces.Add((pieceX + (xm * i), pieceY + (ym * i)));
                }
            }  
        }
    }

    public virtual void FindAvailableSpaces(){
        availableSpaces = new List<(int x, int y)>();
    }

    public virtual void FindTempSpaces(){
        tempAvailableSpaces = new List<(int x, int y)>();
    }

    public void Kill(int colour){
        dead = true;
        killedByColour = colour;
        transform.position = new Vector3(-2, 1, 0);
        pieceX = -2;
        pieceY = 0;
        gameObject.SetActive(false);
    }

    public void SimulateMoves(){
        // Debug.Log(gameObject.name + " simulated moves");
        if (real){
            bool inCheck = false;
            if (BHS.checks[colour]){
                inCheck = true;
            }
            List<(int x, int y)> newAvailableSpaces = new List<(int x, int y)>();
            (int x, int y) oldPos = (pieceX, pieceY);
            pieceX = -1;
            pieceY = -1;
            Pawn pawn = gameObject.GetComponent<Pawn>();
            Piece tempObject;
            Pawn tempPawn;
            foreach((int x, int y) space in availableSpaces){
                tempObject = Instantiate(gameObject, pieces).GetComponent<Piece>();
                BHS = GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>();
                tempObject.real = false;
                tempObject.pieceX = space.x;
                tempObject.pieceY = space.y;
                if (pawn && pawn.enPassantableSquares.Contains(space)){
                    tempPawn = pawn.enPassantablePieces[pawn.enPassantableSquares.IndexOf(space)];
                    BHS.UpdateAvailableSpaces(true, tempPawn.pieceX, tempPawn.pieceY, true);
                }
                else{
                    BHS.UpdateAvailableSpaces(true, tempObject.pieceX, tempObject.pieceY);
                }
                if (!BHS.checks[colour]){
                    newAvailableSpaces.Add(space);
                }
                DestroyImmediate(tempObject.gameObject);
            }
            pieceX = oldPos.x;
            pieceY  = oldPos.y;
            availableSpaces = newAvailableSpaces;
            if (gameObject.GetComponent<King>()){
                King k = gameObject.GetComponent<King>();
                if (!k.hasMoved){
                    if (!availableSpaces.Contains((k.pieceX + k.castleDirections.x, k.pieceY + k.castleDirections.y)) 
                    && availableSpaces.Contains((k.pieceX + (k.castleDirections.x * 2), k.pieceY + (k.castleDirections.y * 2)))){
                        availableSpaces.Remove((k.pieceX + (k.castleDirections.x * 2), k.pieceY + (k.castleDirections.y * 2)));
                    }
                    if (!availableSpaces.Contains((k.pieceX - k.castleDirections.x, k.pieceY - k.castleDirections.y)) 
                    && availableSpaces.Contains((k.pieceX - (k.castleDirections.x * 2), k.pieceY - (k.castleDirections.y * 2)))){
                        availableSpaces.Remove((k.pieceX - (k.castleDirections.x * 2), k.pieceY - (k.castleDirections.y * 2)));
                    }
                    if (inCheck && availableSpaces.Contains((k.pieceX - (k.castleDirections.x * 2), k.pieceY - (k.castleDirections.y * 2)))){
                        availableSpaces.Remove((k.pieceX - (k.castleDirections.x * 2), k.pieceY - (k.castleDirections.y * 2)));
                    }
                    if (inCheck && availableSpaces.Contains((k.pieceX + (k.castleDirections.x * 2), k.pieceY + (k.castleDirections.y * 2)))){
                        availableSpaces.Remove((k.pieceX + (k.castleDirections.x * 2), k.pieceY + (k.castleDirections.y * 2)));
                    }
                }
            }
        }
        else{
            Destroy(gameObject);
        }
    }
}