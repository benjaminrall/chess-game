using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIHandler : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool pauseMenuActive = false;

    public GameObject endGameUI;
    private bool endGameUIActive = false;

    void Start() 
    {
        pauseMenuUI.SetActive(false);
        pauseMenuActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuActive = !pauseMenuActive;
            pauseMenuUI.SetActive(pauseMenuActive);
            FindObjectOfType<AudioManager>().Play("DropPiece");
        }
    }
}
