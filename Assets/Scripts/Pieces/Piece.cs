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
    public bool dead = false;
    [HideInInspector]
    public int killedByColour;
    
    private Transform pieces;
    private BoardHandlerScript BHS;
    

    private void Awake() {
        pieceX = Mathf.RoundToInt(transform.position.x);
        pieceY = Mathf.RoundToInt(transform.position.z);
    }

    public virtual void Start(){
        pieces = GameObject.Find("BoardHandler").transform;
        BHS = GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>();
        //GetComponent<Renderer>().material = BHS.pieceColours[colour];
        FindAvailableSpaces();
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
                if (child != null){
                    if (child.gameObject.GetComponent<Piece>().pieceX == x && child.gameObject.GetComponent<Piece>().pieceY == y && c != child.gameObject.GetComponent<Piece>().colour)
                    {
                        return true;
                    }
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

    public virtual void FindAvailableSpaces(){
        availableSpaces = new List<(int x, int y)>();
    }

    public void Kill(int colour){
        dead = true;
        killedByColour = colour;
        transform.parent = GameObject.Find("DeadPieces").transform;
        //gameObject.SetActive(false);
    }

}
