using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    [HideInInspector]
    public bool hasMoved;

    public int direction = 0;
    public (int x, int y) castleDirections;

    private bool isCheck;
    private GameObject checkSquare;
    public GameObject highlightSquare;
    private GameObject UI;

    public override void Setup() {
        hasMoved = false;
        SetDirections();
        base.Setup();
        BHS = GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>();
        UI = GameObject.Find("GameUIHandler");
    }

    void Update()
    {
        if (BHS.checks[colour] == true && !isCheck)
        {
            checkSquare = Instantiate(highlightSquare, new Vector3(this.pieceX, 0.6f, this.pieceY), Quaternion.identity);
            checkSquare.transform.SetParent(UI.transform);
            isCheck = true;
        }
        else if (BHS.checks[colour] == false && isCheck)
        {
            Destroy(checkSquare);
            isCheck = false;
        }
    }

    private void SetDirections(){
        if (direction == 1){
            castleDirections = (1, 0);
        }
        else if (direction == 2){
            castleDirections = (0, 1);
        }
        else if (direction == 3){
            castleDirections = (1, 0);
        }
        else if (direction == 4){
            castleDirections = (0, 1);
        }
    }

    public override bool checkIsValidMove(int attemptedX, int attemptedY)
    {
        if (!(hasMoved) && availableSpaces.Contains((attemptedX, attemptedY))){
            if(attemptedX == pieceX + (castleDirections.x * 2) && attemptedY == pieceY + (castleDirections.y * 2)){
                Rook r = BHS.GetPieceAt(pieceX + (castleDirections.x * 3), pieceY + (castleDirections.y * 3)).GetComponent<Rook>();
                r.pieceX = pieceX + castleDirections.x;
                r.pieceY = pieceY + castleDirections.y;
                r.transform.position = new Vector3(r.pieceX, 1, r.pieceY);
                r.hasMoved = true;
                hasMoved = true;
                return true;
            }
            else if(attemptedX == pieceX - (castleDirections.x * 2) && attemptedY == pieceY - (castleDirections.y * 2)){
                Rook r = BHS.GetPieceAt(pieceX - (castleDirections.x * 4), pieceY - (castleDirections.y * 4)).GetComponent<Rook>();
                r.pieceX = pieceX - castleDirections.x;
                r.pieceY = pieceY - castleDirections.y;
                r.transform.position = new Vector3(r.pieceX, 1, r.pieceY);
                r.hasMoved = true;
                hasMoved = true;
                return true;
            }
        }
        if (availableSpaces.Contains((attemptedX, attemptedY))){
            if (PieceAt(attemptedX, attemptedY)){
                TakePieceAt(attemptedX, attemptedY, colour);
            }
            if (!hasMoved){
                hasMoved = true;
            }
            return true;
        }
        return false;
    }

    public override void FindAvailableSpaces(){
        base.FindAvailableSpaces();
        foreach ((int x, int y) newPos in new (int, int)[8]{(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)}){
            if (pieceX + newPos.x >= 0 && pieceX + newPos.x <= 7 && pieceY + newPos.y >= 0 && pieceY + newPos.y <= 7){
                if (PieceAt(pieceX + newPos.x, pieceY + newPos.y, colour)){
                    availableSpaces.Add((pieceX + newPos.x, pieceY + newPos.y));
                }
                else if (!PieceAt(pieceX + newPos.x, pieceY + newPos.y)){
                    availableSpaces.Add((pieceX + newPos.x, pieceY + newPos.y));
                }
            }
        }

        if (!hasMoved){
            if (!PieceAt(pieceX + castleDirections.x, pieceY + castleDirections.y) && !PieceAt(pieceX + (castleDirections.x * 2), pieceY + (castleDirections.y * 2))){
                if (BHS.GetPieceAt(pieceX + (castleDirections.x * 3), pieceY + (castleDirections.y * 3)).GetComponent<Rook>() && !BHS.GetPieceAt(pieceX + (castleDirections.x * 3), pieceY + (castleDirections.y * 3)).GetComponent<Rook>().hasMoved){
                    availableSpaces.Add((pieceX + (castleDirections.x * 2), pieceY + (castleDirections.y * 2)));
                }
            }
            if (!PieceAt(pieceX - castleDirections.x, pieceY - castleDirections.y) && !PieceAt(pieceX - (castleDirections.x * 2), pieceY - (castleDirections.y * 2)) && !PieceAt(pieceX - (castleDirections.x * 3), pieceY - (castleDirections.y * 3))){
                if (BHS.GetPieceAt(pieceX - (castleDirections.x * 4), pieceY - (castleDirections.y * 4)).GetComponent<Rook>() && !BHS.GetPieceAt(pieceX - (castleDirections.x * 4), pieceY - (castleDirections.y * 4)).GetComponent<Rook>().hasMoved){
                    availableSpaces.Add((pieceX - (castleDirections.x * 2), pieceY - (castleDirections.y * 2)));
                }
            }
        }
    }

    public override void FindTempSpaces(){
        base.FindTempSpaces();
        foreach ((int x, int y) newPos in new (int, int)[8]{(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)}){
            if (pieceX + newPos.x >= 0 && pieceX + newPos.x <= 7 && pieceY + newPos.y >= 0 && pieceY + newPos.y <= 7){
                if (PieceAt(pieceX + newPos.x, pieceY + newPos.y, colour)){
                    tempAvailableSpaces.Add((pieceX + newPos.x, pieceY + newPos.y));
                }
                else if (!PieceAt(pieceX + newPos.x, pieceY + newPos.y)){
                    tempAvailableSpaces.Add((pieceX + newPos.x, pieceY + newPos.y));
                }
            }
        }
    }
}
