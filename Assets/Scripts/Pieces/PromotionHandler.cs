using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromotionHandler : MonoBehaviour
{
    [HideInInspector]
    public GameObject pawnLink;
    
    public void UpgradePawn(int pieceChoice)
    {
        Debug.Log("Upgrade too: " + pieceChoice.ToString());
        pawnLink.GetComponent<Pawn>().pieceUpgrade = pieceChoice;
        Destroy(this.gameObject);
    }
}
