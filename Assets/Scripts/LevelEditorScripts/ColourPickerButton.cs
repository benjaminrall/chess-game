using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPickerButton : MonoBehaviour
{
    private bool isOut;
    public int colour;

    public void Update()
    {
        if(Input.GetMouseButtonDown(0) && isOut)
        {
            GameObject.Find("Cursor").GetComponent<LevelEditorCursor>().colour = colour;
            GameObject.Find("Cursor").GetComponent<LevelEditorCursor>().notFinished = true;
            isOut = false;
            transform.localScale = new Vector3(1f, 1f, 1f);
            this.transform.parent.gameObject.SetActive(false);
        }
    }

    void OnMouseOver()
    {
        if (!isOut)
        {
            transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            isOut = true;
        }
    }
    void OnMouseExit()
    {
        if (isOut)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            isOut = false;
        }
    }
}
