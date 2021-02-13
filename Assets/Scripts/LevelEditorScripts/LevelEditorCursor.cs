using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorCursor : MonoBehaviour
{
    public int cursorPosX;
    public int cursorPosY;

    public CurrentBoardHandler CBH;

    void Start()
    {

    }

    void Update()
    {
        cursorPosX = Mathf.RoundToInt(this.transform.position.x);
        cursorPosY = Mathf.RoundToInt(this.transform.position.z);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));



        //CBH.Cb[cursorPosY, cursorPosX].isActive = true;
        //CBH.RedrawBoard();
    }
}
