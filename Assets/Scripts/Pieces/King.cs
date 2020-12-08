using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override bool checkIsValidMove(int attemptedX, int attemptedY)
    {
        if (!(0 <= attemptedX && attemptedX <= 7 && 0 <= attemptedY && attemptedY <= 7))    // check if in bounds of board
        {
            return false;
        }
        else if (Math.Abs(attemptedX - pieceX) + Math.Abs(attemptedY - pieceY) == 1 ||   // KING MOVEMENT
        Math.Abs(attemptedX - pieceX) == Math.Abs(attemptedY - pieceY) && 
        (Math.Abs(attemptedX - pieceX) + Math.Abs(attemptedY - pieceY) == 2))
        {
            if (PieceAt(attemptedX, attemptedY, colour)){
                TakePieceAt(attemptedX, attemptedY);
                return true;
            }
            if (PieceAt(attemptedX, attemptedY)){
                return false;
            }
            return true;
        }
        else 
        {
            return false;
        }
    }
}
