using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{

    public override void Start() {
        base.Start();
    }

    public override bool checkIsValidMove(int attemptedX, int attemptedY)
    {
        /*
        if (!(0 <= attemptedX && attemptedX <= 7 && 0 <= attemptedY && attemptedY <= 7))    // check if in bounds of board
        {
            return false;
        }
        else if (!(attemptedY - pieceY == 0 || attemptedX - pieceX == 0) && Math.Abs(attemptedX - pieceX) + Math.Abs(attemptedY - pieceY) == 3){
            if (PieceAt(attemptedX, attemptedY, colour)){
                TakePieceAt(attemptedX, attemptedY, colour);
                return true;
            }
            else if (PieceAt(attemptedX, attemptedY)){
                return false;
            }
            return true;
        }
        else
        {
            return false;
        }
        */
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
        foreach ((int x, int y) newPos in new (int, int)[8]{(1, 2), (2, 1), (2, -1), (1, -2), (-1, -2), (-2, -1), (-2, 1), (-1, 2)}){
            if (pieceX + newPos.x >= 0 && pieceX + newPos.x <= 7 && pieceY + newPos.y >= 0 && pieceY + newPos.y <= 7){
                if (PieceAt(pieceX + newPos.x, pieceY + newPos.y, colour)){
                    availableSpaces.Add((pieceX + newPos.x, pieceY + newPos.y));
                }
                else if (!PieceAt(pieceX + newPos.x, pieceY + newPos.y)){
                    availableSpaces.Add((pieceX + newPos.x, pieceY + newPos.y));
                }
            }
        }
    }
}