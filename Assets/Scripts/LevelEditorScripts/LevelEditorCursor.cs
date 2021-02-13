using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorCursor : MonoBehaviour
{
    public struct Tool
    {
        public string Name;
        public Button ToolButtonMain;
        public Button[] ToolButtonChild;
    }

    public int cursorPosX;
    public int cursorPosY;

    public CurrentBoardHandler CBH;
    //public Tool[]

    void Start()
    {

    }

    void Update()
    {
        cursorPosX = Mathf.RoundToInt(this.transform.position.x);
        cursorPosY = Mathf.RoundToInt(this.transform.position.z);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));



        if (Input.GetMouseButtonDown(0)) CBH.AddSquare(new Vector2(cursorPosY, cursorPosX));
        if (Input.GetMouseButtonDown(1)) CBH.RemoveSquare(new Vector2(cursorPosY, cursorPosX));
    }
}
