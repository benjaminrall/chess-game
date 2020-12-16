using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool pauseMenuActive;

    public GameObject settingsMenuUI;
    public bool settingsMenuActive;

    public GameObject exitConfirmUI;
    public GameObject exitDefiniteConfirmUI;


    void Start()
    {
        pauseMenuUI.SetActive(false);
        exitConfirmUI.SetActive(false);
        exitDefiniteConfirmUI.SetActive(false);
        settingsMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuActive){
                pauseMenuUI.SetActive(false);
                pauseMenuActive = false;
            }
            else if (!pauseMenuActive){
                pauseMenuUI.SetActive(true);
                pauseMenuActive = true;
            }
        }
    }

    public void SettingsMenu()
    {
        if (settingsMenuActive)
        {
            settingsMenuUI.SetActive(false);
            settingsMenuActive = false;
        }
        else if (!settingsMenuActive)
        {
            settingsMenuUI.SetActive(true);
            settingsMenuActive = true;
        }
    }

    public void ExitGame()
    {
        exitConfirmUI.SetActive(true);
    }

    public void SecondExitGame()
    {
        exitDefiniteConfirmUI.SetActive(true);
    }

    public void ThirdExitGame()
    {
        SceneManager.LoadScene("Menu");
    }

}
