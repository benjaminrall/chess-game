using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHandlerScript : MonoBehaviour
{

    public Material[] pieceColours;
    public int players = 2;
    [HideInInspector]
    public int turn = 0;
    public GameObject indicator;

    public void UpdateAvailableSpaces(){
        foreach(Transform child in transform)
        {
            child.gameObject.GetComponent<Piece>().FindAvailableSpaces();
        }
    }

    public void ShowIndicators(bool show, List<(int x, int y)> spaces){
        if (show){
            foreach ((int x, int y) space in spaces){
                Instantiate(indicator, new Vector3(space.x, 1.5f, space.y), new Quaternion(0, 0, 0, 0), GameObject.Find("MoveIndicators").transform);
            }
        }
        else{
            foreach (Transform child in GameObject.Find("MoveIndicators").transform){
                Destroy(child.gameObject);
            }
        }
    }
}
