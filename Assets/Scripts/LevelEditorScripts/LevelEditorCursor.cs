using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public int currentDrawType;

    public CurrentBoardHandler CBH;
    //public Tool[]

    void Start()
    {
        currentDrawType = -1;
    }

    void Update()
    {
        cursorPosX = Mathf.RoundToInt(this.transform.position.x);
        cursorPosY = Mathf.RoundToInt(this.transform.position.z);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

        if (Input.GetMouseButtonDown(0) && !IsMouseOverUi()) CBH.AddSquare(new Vector2(cursorPosY, cursorPosX), currentDrawType);
        if (Input.GetMouseButtonDown(1)) CBH.RemoveSquare(new Vector2(cursorPosY, cursorPosX));
    }

    private bool IsMouseOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void ChangeDrawType(int type)
    {
        currentDrawType = type;
    }
}
