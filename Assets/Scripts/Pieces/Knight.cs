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

    public override void FindTempSpaces(){
        base.FindTempSpaces();
        foreach ((int x, int y) newPos in new (int, int)[8]{(1, 2), (2, 1), (2, -1), (1, -2), (-1, -2), (-2, -1), (-2, 1), (-1, 2)}){
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