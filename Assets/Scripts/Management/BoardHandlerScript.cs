﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHandlerScript : MonoBehaviour
{

    public Material[] piecePrimaryColours;
    public Material[] pieceSecondaryColours;
    public Material[] pieceTertiaryColours;
    public int players = 2;
    public GameObject indicator;

    [HideInInspector]
    public int turn = 0;
    [HideInInspector]
    public bool[] checks;
    
    private King[] kings;

    private void Start() {
        checks = new bool[players];
        for (int i = 0; i < checks.Length; i++){
            checks[i] = false;
        }
        kings = new King[players];
        foreach (Transform child in transform){
            if (child.gameObject.GetComponent<King>() != null){
                kings[child.gameObject.GetComponent<King>().colour] = child.gameObject.GetComponent<King>();
            }
        }
    }

    public void UpdateAvailableSpaces(bool temp = false, int nx = 0, int ny = 0){
        if (!temp){
            foreach(Transform child in transform)
            {
                child.gameObject.GetComponent<Piece>().FindAvailableSpaces();
                child.gameObject.GetComponent<Piece>().SimulateMoves();
            }
        }
        else{
            foreach(Transform child in transform)
            {
                if (child.gameObject.GetComponent<Piece>().pieceX == nx && child.gameObject.GetComponent<Piece>().pieceY == ny){
                    child.gameObject.GetComponent<Piece>().tempAvailableSpaces = new List<(int x, int y)>();
                }
                else{
                    child.gameObject.GetComponent<Piece>().FindTempSpaces();
                }
            }
        }
        UpdateChecks(temp);
    }

    public void ShowIndicators(bool show, List<(int x, int y)> spaces){
        if (show){
            foreach ((int x, int y) space in spaces){
                Instantiate(indicator, new Vector3(space.x, 1.5f, space.y), Quaternion.Euler(90,0,0), GameObject.Find("MoveIndicators").transform);
            }
        }
        else{
            foreach (Transform child in GameObject.Find("MoveIndicators").transform){
                Destroy(child.gameObject);
            }
        }
    }

    public void UpdateChecks(bool temp){
        List<int> checkedColours = new List<int>();
        kings = new King[players];
        foreach (Transform child in transform){
            if (child.gameObject.GetComponent<King>() != null){
                kings[child.gameObject.GetComponent<King>().colour] = child.gameObject.GetComponent<King>();
            }
        }
        if (!temp){
            foreach(Transform child in transform){
                for (int i = 0; i < kings.Length; i++){
                    if (child.gameObject.GetComponent<Piece>().colour != i){
                        foreach((int x, int y) availablePos in child.gameObject.GetComponent<Piece>().availableSpaces){
                            if (kings[i].pieceX == availablePos.x && kings[i].pieceY == availablePos.y){
                                checkedColours.Add(i);
                            }
                        }
                    }
                }
            }
        }
        else{
            foreach(Transform child in transform){
                for (int i = 0; i < kings.Length; i++){
                    if (child.gameObject.GetComponent<Piece>().colour != i){
                        foreach((int x, int y) availablePos in child.gameObject.GetComponent<Piece>().tempAvailableSpaces){
                            if (kings[i].pieceX == availablePos.x && kings[i].pieceY == availablePos.y){
                                checkedColours.Add(i);
                            }
                        }
                    }
                }
            }
        }
        bool foundCol;
        for (int i = 0; i < kings.Length; i++){
            foundCol = false;
            foreach(int c in checkedColours){
                if (i == c){
                    foundCol = true;
                    checks[i] = true;
                }
            }
            if (!foundCol){
                checks[i] = false;
            }
        }
    }

    public void CheckForEnd(){
        bool ended;
        for (int i = 0; i < players; i++){
            ended = true;
            foreach(Transform child in transform){
                if (child.gameObject.GetComponent<Piece>().colour == i){
                    if(child.gameObject.GetComponent<Piece>().availableSpaces.Count > 0 && !child.gameObject.GetComponent<Piece>().dead){
                        ended = false;
                        break;
                    }
                }
            }
            if (ended && checks[i]){
                Debug.Log(i + " checkmated");
            }
            else if (ended){
                Debug.Log(i + " stalemated");
            }
        }
    }

    public GameObject GetPieceAt(int x, int y){
        foreach(Transform child in transform){
            if (child.gameObject.GetComponent<Piece>().pieceX == x && child.gameObject.GetComponent<Piece>().pieceY == y){
                return child.gameObject;
            }
        }
        return gameObject;
    }
}
