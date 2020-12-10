using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    struct PieceLists
    {
        public List<GameObject> takenPieces;
        public int playerColour;

        //public MyStruct(int pC, GameObject go)
        //{
        //    playerColour = pC;
        //    takenPieces = new List<GameObject>();
        //}
    }

    private GameObject DeadPiecesOB;
    //public List<PieceLists>



    void Start()
    {
        DeadPiecesOB = GameObject.Find("DeadPieces");
    }

    void Update()
    {
        GetDeadPieces();
    }

    public void GetDeadPieces()
    {
        for (int i = 0; i < DeadPiecesOB.transform.childCount; i++)
        {
            Destroy(DeadPiecesOB.transform.GetChild(i).gameObject);
        }
    }

}
