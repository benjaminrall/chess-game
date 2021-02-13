using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorCursor : MonoBehaviour
{
    public int cursorPosX;
    public int cursorPosY;



    void Start()
    {
        
    }

    void Update()
    {
        cursorPosX = Mathf.RoundToInt(Input.mousePosition.x);
        cursorPosY = Mathf.RoundToInt(Input.mousePosition.y);


    }
}
