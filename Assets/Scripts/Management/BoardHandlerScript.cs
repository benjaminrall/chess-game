using System.Collections;
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
    public (int colour, bool inCheck)[] checks;

    private void Start() {
        checks = new (int, bool)[players];
        for (int i = 0; i < checks.Length; i++){
            checks[i].colour = i;
            checks[i].inCheck = false;
        }
    }

    public void UpdateAvailableSpaces(){
        foreach(Transform child in transform)
        {
            child.gameObject.GetComponent<Piece>().FindAvailableSpaces();
        }
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
}
