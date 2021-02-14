using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelEditorCursor : MonoBehaviour
{
    public struct Tool
    {
        public string toolID;
        public string toolDisplayName;
    }

    //References
    public CurrentBoardHandler CBH;

    //Useful Shit
    public int cursorPosX;
    public int cursorPosY;

    public int currentDrawType;
    public string currentTool;
    public Text currentToolText;
    public GameObject currentToolUi;

    //Board Draw
    public bool isDrawing;
    public bool isErasing;

    void Start()
    {
        currentDrawType = -1;
    }

    void Update()
    {
        cursorPosX = Mathf.RoundToInt(this.transform.position.x);
        cursorPosY = Mathf.RoundToInt(this.transform.position.z);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

        

        if (currentTool == "Board_Single")
        {
            //Draw
            if (Input.GetMouseButtonDown(0) && !IsMouseOverUi() && !isDrawing) isDrawing = true;
            else if (Input.GetMouseButtonUp(0) && !IsMouseOverUi() && isDrawing) isDrawing = false;
            if (isDrawing) CBH.AddSquare(new Vector2(cursorPosY, cursorPosX), currentDrawType); 

            //Erase
            if (Input.GetMouseButtonDown(1) && !IsMouseOverUi() && !isErasing) isErasing = true;
            else if (Input.GetMouseButtonUp(1) && !IsMouseOverUi() && isErasing) isErasing = false;
            if (isErasing) CBH.RemoveSquare(new Vector2(cursorPosY, cursorPosX));
        }
    }

    public void PickupNewTool(string tool)
    {
        string[] temp = tool.Split('-');
        currentTool = temp[0];
        currentToolText.text = temp[1];
        currentToolUi.SetActive(true);
    }

    public void ClearCurrentTool()
    {
        currentTool = "";
        currentToolText.text = "";
        currentToolUi.SetActive(false);
    }

    private bool IsMouseOverUi(){
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void ChangeDrawType(int type)
    {
        currentDrawType = type;
    }
}
