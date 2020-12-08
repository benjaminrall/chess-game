using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPickup : MonoBehaviour
{

    public bool HoldingPiece = false;
    public GameObject GO;
    public GameObject HeldPieceGO;
    private BoardHandlerScript BHS;

    private void Start() 
    {
        BHS = GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        transform.position = mousePos;

        if (RayGetPiece() != null){ // Sets GO to raycast Piece
            GO = RayGetPiece();
        }
        else{
            GO = null;
        }

        if (Input.GetMouseButtonDown(0) && GO != null && GO.GetComponent<Piece>().colour == BHS.turn){ // Pickup piece boolean
            HoldingPiece = true;
            HeldPieceGO = GO;

            BHS.ShowIndicators(true, HeldPieceGO.gameObject.GetComponent<Piece>().availableSpaces);
            HeldPieceGO.gameObject.GetComponent<PieceDrag>().OriginalPos();
            HeldPieceGO.transform.position = new Vector3(HeldPieceGO.transform.position.x, HeldPieceGO.transform.position.y + 1, HeldPieceGO.transform.position.z); 
        }
        if(Input.GetMouseButtonUp(0) && HeldPieceGO  != null){
            HeldPieceGO.gameObject.GetComponent<PieceDrag>().DropPiece();
            HeldPieceGO.transform.position = new Vector3(HeldPieceGO.transform.position.x, HeldPieceGO.transform.position.y - 1, HeldPieceGO.transform.position.z);
            HoldingPiece = false;
            HeldPieceGO = null;
        }

        if (HoldingPiece){
            HeldPieceGO.gameObject.GetComponent<PieceDrag>().DragPiece(this.gameObject);
        }
    }

    public GameObject RayGetPiece() // Returns Gameobject reference of what the mouse is over
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100)){
            return hit.transform.gameObject;
        }
        else{
            return null;
        }      
    }
}