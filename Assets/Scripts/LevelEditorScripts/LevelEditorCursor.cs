using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelEditorCursor : MonoBehaviour
{
    [System.Serializable]
    public struct MaterialDisplayness
    {
        public Material mat;
        public string displayName;
    }

    //References
    public CurrentBoardHandler CBH;

    //Useful Shit
    public int cursorPosX;
    public int cursorPosY;

    public string currentTool;
    public int currentDrawType;
    
    public Text currentToolText;
    public Text currentMaterialText;

    public GameObject currentToolUi;
    public GameObject currentMaterialUi;
    public GameObject buttonDrop;

    public Material currentMaterial;
    public GameObject currentMaterialDisplay;

    //Board Draw
    public bool isDrawing;
    public bool isErasing;
    public MaterialDisplayness[] materialPreviews;
    public GameObject[] materialButtons;

    void Start()
    {
        currentDrawType = -1;
        currentMaterial = materialPreviews[0].mat;
        currentMaterialText.text = materialPreviews[0].displayName;
    }

    void Update()
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5f));
        cursorPosX = Mathf.RoundToInt(temp.x);
        cursorPosY = Mathf.RoundToInt(temp.z);
        transform.position = new Vector3(cursorPosX, 5f, cursorPosY);

        if (currentTool == "" && currentToolUi.activeInHierarchy) StartCoroutine(CloseCurrentToolUi());

        if (currentTool == "Board_Single")
        {
            currentMaterialUi.SetActive(true);
            //Draw
            if (Input.GetMouseButtonDown(0) && !IsMouseOverUi() && !isDrawing) isDrawing = true;
            else if (Input.GetMouseButtonUp(0) && !IsMouseOverUi() && isDrawing) isDrawing = false;
            if (isDrawing) CBH.AddSquare(new Vector2(cursorPosY, cursorPosX), currentDrawType);

            //Erase
            if (Input.GetMouseButtonDown(1) && !IsMouseOverUi() && !isErasing) isErasing = true;
            else if (Input.GetMouseButtonUp(1) && !IsMouseOverUi() && isErasing) isErasing = false;
            if (isErasing) CBH.RemoveSquare(new Vector2(cursorPosY, cursorPosX));
        }
        else currentMaterialUi.SetActive(false);
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

    public IEnumerator CloseCurrentToolUi()
    {
        if (!materialButtons[0].activeInHierarchy) yield return StartCoroutine(ToggleMaterialDropDown(true));
        currentMaterialDisplay.GetComponent<Image>().material = materialPreviews[0].mat;
        currentMaterialUi.SetActive(false);
        currentToolUi.SetActive(false);
    }

    private bool IsMouseOverUi(){
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void ChangeDrawType(int type)
    {
        currentDrawType = type;
        if (currentDrawType == -1)
        {
            currentMaterial = materialPreviews[0].mat;
            currentMaterialText.text = materialPreviews[0].displayName;
        }
        else
        {
            currentMaterial = materialPreviews[currentDrawType - 1].mat;
            currentMaterialText.text = materialPreviews[currentDrawType - 1].displayName;
        }
        currentMaterialDisplay.GetComponent<Image>().material = currentMaterial;
    }

    public void MaterialDropDown(bool state)
    {
        StartCoroutine(ToggleMaterialDropDown(state));
    }
    public IEnumerator ToggleMaterialDropDown(bool state)
    {
        if (!state){
            
            for (int i = materialButtons.Length; i-- > 0;)
            {
                materialButtons[i].transform.GetComponent<RectTransform>().localPosition = new Vector2(850, 315 - (90 * (i)));
                yield return new WaitForSeconds(0.05f);
            }
            materialButtons[0].SetActive(false);
            buttonDrop.SetActive(true);
            yield return null;
        }
        else{
            buttonDrop.SetActive(false);
            int i = 1;
            materialButtons[0].SetActive(true);
            foreach (GameObject t in materialButtons)
            {
                t.transform.GetComponent<RectTransform>().localPosition = new Vector2(755, 315 - (90 * i));
                yield return new WaitForSeconds(0.05f);
                i++;
            }
        }
    }
}
