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
        SetDirections();
        base.Start();
    }

    private void SetDirections(){
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
    }

    public override bool checkIsValidMove(int attemptedX, int attemptedY){
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

    public override void FindTempSpaces(){
        base.FindTempSpaces();
        SetDirections();
        if (0 <= pieceX + directions[0].x && pieceX + directions[0].x <= 7 && 0 <= pieceY + directions[0].y && pieceY + directions[0].y <= 7){
            if (!PieceAt(pieceX + directions[0].x, pieceY + directions[0].y)){
                tempAvailableSpaces.Add((pieceX + directions[0].x, pieceY + directions[0].y));
            }
            if (!PieceAt(pieceX + directions[0].x, pieceY + directions[0].y) && !PieceAt(pieceX + (directions[0].x * 2), pieceY + (directions[0].y * 2)) && !hasMoved){
                tempAvailableSpaces.Add((pieceX + (directions[0].x * 2), pieceY + (directions[0].y * 2)));
            }
            if (PieceAt(pieceX + directions[1].x, pieceY + directions[1].y, colour)){
                tempAvailableSpaces.Add((pieceX + directions[1].x, pieceY + directions[1].y));
            }
            if (PieceAt(pieceX + directions[2].x, pieceY + directions[2].y, colour)){
                tempAvailableSpaces.Add((pieceX + directions[2].x, pieceY + directions[2].y));
            }
        }
    }
}
