using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBeltHandler : MonoBehaviour
{
    public GameObject[] availableTools;
    public float beltTimings;
    public bool isDropped;

    public GameObject[][] AllTools = new GameObject[5][];
    public GameObject[] ChessBoardTools;

    public void Start()
    {
        AllTools[0] = ChessBoardTools;
    }

    public void DropDownBelt(){
        StartCoroutine(ToggleBeltDropDown());
    }
    public IEnumerator ToggleBeltDropDown()
    {
        for(int i = 0; AllTools.Length > i; i++)
        {
            if (AllTools[i] == null) continue;
            if (AllTools[i][0].activeInHierarchy)
            {
                StartCoroutine(ToggleBeltHorizontal(i));
            }
        }

        if (isDropped)
        {
            for (int i = availableTools.Length; i-- > 0; )
            {
                availableTools[i].transform.GetComponent<RectTransform>().localPosition = new Vector2(-850, 405 - (90 * (i+1)));
                yield return new WaitForSeconds(beltTimings);
            }
            isDropped = false;
            yield return null;
        }
        else
        {
            int i = 1;
            foreach (GameObject t in availableTools)
            {
                t.transform.GetComponent<RectTransform>().localPosition = new Vector2(-754, 405 - (90 * i));
                yield return new WaitForSeconds(beltTimings);
                i++;
            }
            isDropped = true;
        }
    }

    public void DropBeltHorizontal(int toolType)
    {
        StartCoroutine(ToggleBeltHorizontal(toolType));
    }
    public IEnumerator ToggleBeltHorizontal(int toolType)
    {
        
        if (AllTools[toolType][0].activeInHierarchy)
        {
            for (int i = AllTools[toolType].Length; i-- > 0;)
            {
                AllTools[toolType][i].transform.GetComponent<RectTransform>().localPosition = new Vector2(-844 - (90 * (i+1)), AllTools[toolType][0].transform.GetComponent<RectTransform>().localPosition.y);
                yield return new WaitForSeconds(beltTimings);
            }
            AllTools[toolType][0].SetActive(false);
            yield return null;
        }
        else
        {
            AllTools[toolType][0].SetActive(true);
            int i = 1;
            foreach (GameObject t in AllTools[toolType])
            {
                t.transform.GetComponent<RectTransform>().localPosition = new Vector2(-754 + (90 * i), AllTools[toolType][0].transform.GetComponent<RectTransform>().localPosition.y);
                yield return new WaitForSeconds(beltTimings);
                i++;
            }
        }
    }
}
