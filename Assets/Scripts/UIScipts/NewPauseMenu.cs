using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPauseMenu : MonoBehaviour
{
    public GameObject pauseMenuMain;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuMain.activeInHierarchy)
            {
                pauseMenuMain.SetActive(false);
                SetAllChildren(this.gameObject,false);
            }
            else if (!pauseMenuMain.activeInHierarchy)
            {
                pauseMenuMain.SetActive(true);
                SetAllChildren(this.gameObject,true);
            }
        }
    }

    public void SetAllChildren(GameObject go, bool state)
    {
        foreach (Transform child in pauseMenuMain.transform)
        {
            child.gameObject.SetActive(state);
        }
    }

    public void Button(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
    }
}
