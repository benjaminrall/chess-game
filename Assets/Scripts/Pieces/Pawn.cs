using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public int direction;
    private (int x, int y)[] directions;
    private bool hasMoved;
    public override void Start()
    {
        hasMoved = false;
        if (direction == 1){
            directions = new (int, int)[3]{(0, 1), (-1, 1), (1, 1)};
        }
        else if (direction == 2){
            directions = new (int, int)[3]{(1, 0), (1, 1), (1, -1)};
        }
        else if (direction == 3){
            directions = new (int, int)[3]{(0, -1), (1, -1), (-1, -1)};
        }
        else if (direction == 4){
            directions = new (int, int)[3]{(-1, 0), (-1, -1), (-1, 1)};
        }
        base.Start();
    }

    public override bool checkIsValidMove(int attemptedX, int attemptedY){
        /*
        if (!(0 <= attemptedX && attemptedX <= 7 && 0 <= attemptedY && attemptedY <= 7))    // check if in bounds of board
        {
            return false;
        }
        else if (attemptedX - pieceX == directions[0].x * 2 && attemptedY - pieceY == directions[0].y * 2 && !hasMoved){
            if (PieceAt(attemptedX, attemptedY)){
                return false;
            }
            hasMoved = true;
            return true;
        }
        else if (attemptedX - pieceX == directions[0].x && attemptedY - pieceY == directions[0].y){
            if (PieceAt(attemptedX, attemptedY)){
                return false;
            }
            if (!hasMoved){
                hasMoved = true;
            }
            return true;
        }
        else if (
            (attemptedX - pieceX == directions[1].x && attemptedY - pieceY == directions[1].y) || 
            (attemptedX - pieceX == directions[2].x && attemptedY - pieceY == directions[2].y)){
            if (PieceAt(attemptedX, attemptedY, colour)){
                TakePieceAt(attemptedX, attemptedY, colour);
                if (!hasMoved){
                    hasMoved = true;
                }
                return true;
            }
            else{
                return false;
            }
        }
        else{
            return false;
        }
        */
        foreach ((int x, int y) space in availableSpaces){
            if (attemptedX == space.x && attemptedY == space.y){
                if (PieceAt(attemptedX, attemptedY)){
                    TakePieceAt(attemptedX, attemptedY, colour);
                }
                if (!hasMoved){
                    hasMoved = true;
                }
                return true;
            }
        }
        return false;
    }

    public override void FindAvailableSpaces(){
        base.FindAvailableSpaces();
        if (0 <= pieceX + directions[0].x && pieceX + directions[0].x <= 7 && 0 <= pieceY + directions[0].y && pieceY + directions[0].y <= 7){
            if (!PieceAt(pieceX + directions[0].x, pieceY + directions[0].y)){
                availableSpaces.Add((pieceX + directions[0].x, pieceY + directions[0].y));
            }
            if (!PieceAt(pieceX + directions[0].x, pieceY + directions[0].y) && !PieceAt(pieceX + (directions[0].x * 2), pieceY + (directions[0].y * 2)) && !hasMoved){
                availableSpaces.Add((pieceX + (directions[0].x * 2), pieceY + (directions[0].y * 2)));
            }
            if (PieceAt(pieceX + directions[1].x, pieceY + directions[1].y, colour)){
                availableSpaces.Add((pieceX + directions[1].x, pieceY + directions[1].y));
            }
            if (PieceAt(pieceX + directions[2].x, pieceY + directions[2].y, colour)){
                availableSpaces.Add((pieceX + directions[2].x, pieceY + directions[2].y));
            }
        }
    }
}
