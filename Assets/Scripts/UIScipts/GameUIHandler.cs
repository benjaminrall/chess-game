using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIHandler : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool pauseMenuActive;

    public GameObject endGameUI;
    private bool endGameUIActive;

    public GameObject gameTimeUI;
    private bool timeUIExpanded;

    private bool moving = false;

    void Start() 
    {
        pauseMenuUI.SetActive(false);
        pauseMenuActive = false;
        endGameUI.SetActive(false);
        endGameUIActive = false;
        gameTimeUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 500.0f);
        timeUIExpanded = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuActive = !pauseMenuActive;
            pauseMenuUI.SetActive(pauseMenuActive);
            FindObjectOfType<AudioManager>().Play("DropPiece");
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!moving)
            {
                timeUIExpanded = !timeUIExpanded;
                if (timeUIExpanded) StartCoroutine(RetractTimeList());
                else StartCoroutine(ExpandTimeList());
            }
        }
    }

    IEnumerator RetractTimeList()
    {
        if (moving)
        {
            yield break;
        }
        moving = true;
        LeanTween.moveX(gameTimeUI.GetComponent<RectTransform>(), 0, 0.1f);    //.setEase(LeanTweenType.easeOutExpo);
        yield return new WaitForSeconds(0.1f);
        gameTimeUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
        moving = false;
    }

    IEnumerator ExpandTimeList()
    {
        if (moving)
        {
            yield break;
        }
        moving = true;
        LeanTween.moveX(gameTimeUI.GetComponent<RectTransform>(), -500, 0.1f);//.setEase(LeanTweenType.easeOutExpo);
        yield return new WaitForSeconds(0.1f);
        moving = false;
    }
}
