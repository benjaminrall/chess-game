using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardHandlerScript : MonoBehaviour
{

    public Material[] piecePrimaryColours;
    public Material[] pieceSecondaryColours;
    public Material[] pieceTertiaryColours;
    public int players = 2;
    public GameObject indicator;
    public NetworkManager networkManager;
    public string[] colourNames;

    public bool gameIsPlaying = true;
    public FinishedGameUI FGUI;

    [HideInInspector]
    public int turn = 0;
    [HideInInspector]
    public bool[] checks;
    [HideInInspector]
    public bool active;
    
    private King[] kings;

    public void Setup(){
        FGUI = GameObject.Find("UIHandler").GetComponent<FinishedGameUI>();

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

    public string Encode(){
        return "";
    }

    public void Decode(string msg){

    }

    public void UpdateAvailableSpaces(bool temp = false, int nx = 0, int ny = 0, bool enPassant = false){
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
                    if (enPassant){
                        (int x, int y) oldPos = (child.gameObject.GetComponent<Piece>().pieceX, child.gameObject.GetComponent<Piece>().pieceY);
                        child.gameObject.GetComponent<Piece>().pieceX = -1;
                        child.gameObject.GetComponent<Piece>().pieceY = -1;
                        UpdateAvailableSpaces(true, nx, ny);
                        UpdateChecks(temp);
                        child.gameObject.GetComponent<Piece>().pieceX = oldPos.x;
                        child.gameObject.GetComponent<Piece>().pieceY = oldPos.y;
                        return;
                    }
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
                FGUI.PlayerCheckmated(i);
            }
            else if (ended){
                Debug.Log(i + " stalemated");
                FGUI.ShowEndGameUI("Stalemate");
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
