using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pawn : Piece
{
    public int direction;
    public int promotionMoves = 6;
    public GameObject[] promotionPieces;
    public GameObject promotionUIGO;
    public GameObject WSUIGO;
    private GameObject upgradeLink;
    public int pieceUpgrade = -1;

    [HideInInspector]
    public bool enPassant = false;
    [HideInInspector]
    public (int x, int y) enPassantSquare;
    [HideInInspector]
    public List<(int x, int y)> enPassantableSquares = new List<(int x, int y)>();
    [HideInInspector]
    public List<Pawn> enPassantablePieces = new List<Pawn>();
    [HideInInspector]
    public bool promoted = false;

    public (int x, int y)[] directions;
    private bool hasMoved;
    private int turnsSinceMove = 0;
    private int movesMade = 0;
    private int startX;
    private int startY;

    public override void Setup()
    {
        hasMoved = false;
        SetDirections();
        enPassantSquare = (pieceX + directions[0].x, pieceY + directions[0].y);
        base.Setup();
        startX = pieceX;
        startY = pieceY;
        WSUIGO = GameObject.Find("WorldSpaceUI");
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

        if (enPassantableSquares.Contains((attemptedX, attemptedY)) && availableSpaces.Contains((attemptedX, attemptedY))){
            Pawn p = enPassantablePieces[enPassantableSquares.IndexOf((attemptedX, attemptedY))];
            TakePieceAt(p.pieceX, p.pieceY, colour);
        }

        if (availableSpaces.Contains((attemptedX, attemptedY))){
            if (PieceAt(attemptedX, attemptedY)){
                TakePieceAt(attemptedX, attemptedY, colour);
            }
            if (!hasMoved){
                if (attemptedX == pieceX + (2 * directions[0].x) && attemptedY == pieceY + (2 * directions[0].y)){
                    enPassant = true;
                    movesMade++;
                }
                hasMoved = true;
            }
            movesMade++;
            if (movesMade >= promotionMoves)
            {
                upgradeLink = Instantiate(promotionUIGO, new Vector3(attemptedX, 3, attemptedY), Quaternion.Euler(90,0,0));
                upgradeLink.transform.SetParent(WSUIGO.transform);
                upgradeLink.GetComponent<PromotionHandler>().pawnLink = this.gameObject;

                promoted = true;
            }
            return true;
        }
        return false;
    }

    public override void FindAvailableSpaces(){
        base.FindAvailableSpaces();
        enPassantableSquares = new List<(int x, int y)>();
        enPassantablePieces = new List<Pawn>();
        if (turnsSinceMove == 1 && enPassant){
            enPassant = false;
        }
        if(hasMoved){
            turnsSinceMove++;
        }
        SetDirections();
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
        if (hasMoved){
            if (BHS.GetPieceAt(pieceX + directions[0].x, pieceY + directions[0].y).GetComponent<Pawn>()){
                Pawn p = BHS.GetPieceAt(pieceX + directions[0].x, pieceY + directions[0].y).GetComponent<Pawn>();
                if (p.colour != colour && p.direction % 2 == (direction + 1) % 2 && p.enPassant){
                    availableSpaces.Add((p.enPassantSquare.x, p.enPassantSquare.y));
                    enPassantableSquares.Add((p.enPassantSquare.x, p.enPassantSquare.y));
                    enPassantablePieces.Add(p);
                }
            }
            if (direction % 2 == 0 && (BHS.GetPieceAt(pieceX, pieceY + directions[1].y).GetComponent<Pawn>() || BHS.GetPieceAt(pieceX, pieceY + directions[2].y).GetComponent<Pawn>())){
                Pawn p = BHS.GetPieceAt(pieceX, pieceY + directions[1].y).GetComponent<Pawn>();
                if (!p){
                    p = BHS.GetPieceAt(pieceX, pieceY + directions[2].y).GetComponent<Pawn>();
                }
                if (p.colour != colour && p.direction % 2 == direction % 2 && p.enPassant){
                    availableSpaces.Add((p.enPassantSquare.x, p.enPassantSquare.y));
                    enPassantableSquares.Add((p.enPassantSquare.x, p.enPassantSquare.y));
                    enPassantablePieces.Add(p);
                }
            }
            if (direction % 2 == 1 && (BHS.GetPieceAt(pieceX + directions[1].x, pieceY).GetComponent<Pawn>() || BHS.GetPieceAt(pieceX + directions[2].x, pieceY).GetComponent<Pawn>())){
                Pawn p = BHS.GetPieceAt(pieceX + directions[1].x, pieceY).GetComponent<Pawn>();
                if (!p){
                    p = BHS.GetPieceAt(pieceX + directions[2].x, pieceY).GetComponent<Pawn>();
                }
                if (p.colour != colour && p.direction % 2 == direction % 2 && p.enPassant){
                    availableSpaces.Add((p.enPassantSquare.x, p.enPassantSquare.y));
                    enPassantableSquares.Add((p.enPassantSquare.x, p.enPassantSquare.y));
                    enPassantablePieces.Add(p);
                }
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
