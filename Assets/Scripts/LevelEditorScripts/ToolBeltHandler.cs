using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBeltHandler : MonoBehaviour
{
    public GameObject[] availableTools;
    public float beltTimings;
    public bool isDropped;

    void Update()
    {
        
    }

    public void DropDownBelt(){
        StartCoroutine(ToggleBeltDropDown());
    }
    public IEnumerator ToggleBeltDropDown()
    {
        if (isDropped)
        {
            for (int i = availableTools.Length; i-- > 0; )
            {
                availableTools[i].transform.GetComponent<RectTransform>().localPosition = new Vector2(-850, 500);
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
}
