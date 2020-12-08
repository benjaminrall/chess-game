using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public override bool checkIsValidMove(int attemptedX, int attemptedY)
    {
        /*
        if (!(0 <= attemptedX && attemptedX <= 7 && 0 <= attemptedY && attemptedY <= 7))    // check if in bounds of board
        {
            return false;
        }
        else if (attemptedY - pieceY == 0 || attemptedX - pieceX == 0)
        {
            if (Math.Abs(attemptedY) - pieceY == 0 && Math.Abs(attemptedX) - pieceX > 0){
                return CheckPath(1, Math.Abs(attemptedX - pieceX), 1, 0);
            } 
            else if (Math.Abs(attemptedY) - pieceY == 0 && Math.Abs(attemptedX) - pieceX < 0){
                return CheckPath(1, Math.Abs(attemptedX - pieceX), -1, 0);
            }
            else if (Math.Abs(attemptedX) - pieceX == 0 && Math.Abs(attemptedY) - pieceY > 0){
                return CheckPath(1, Math.Abs(attemptedY - pieceY), 0, 1);
            }
            else if (Math.Abs(attemptedX) - pieceX == 0 && Math.Abs(attemptedY) - pieceY < 0){
                return CheckPath(1, Math.Abs(attemptedY - pieceY), 0, -1);
            }
            return false;
        }
        else
        {
            return false;
        }*/
        foreach ((int x, int y) space in availableSpaces){
            if (attemptedX == space.x && attemptedY == space.y){
                if (PieceAt(attemptedX, attemptedY)){
                    TakePieceAt(attemptedX, attemptedY, colour);
                }
                return true;
            }
        }
        return false;
    }

    public override void FindAvailableSpaces(){
        base.FindAvailableSpaces();
        CheckSpaces(1, 7 - pieceY, 0, 1);
        CheckSpaces(1, pieceY, 0, -1);
        CheckSpaces(1, 7 - pieceX, 1, 0);
        CheckSpaces(1, pieceX, -1, 0);
        /*for (int i = 0; i < availableSpaces.Count; i++){
            Debug.Log(availableSpaces[i].x.ToString() + " " + availableSpaces[i].y.ToString());
        }*/
    }
}
