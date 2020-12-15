using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandlerScript : MonoBehaviour
{
    public GameObject ServerConnectUI;
    public GameObject MenuUI;
    public GameObject ServerForms;

    public GameObject JoinGameUI;
    public GameObject CreateGameUI;

    public Text startupIPField;

    public Text CodeInput;
    public string ConnectedCode;

    //Account Details
    public string playerName;
    public string connectedIP;

    public Text NameDisplay;
    public Text ConnectedIPDisplay;

    void Start()
    {
        ServerConnectUI.SetActive(true);
        MenuUI.SetActive(false);
        ServerForms.SetActive(false);
    }

    void Update()
    {

    }

    public void SubmitFirstIP()
    {
        connectedIP = startupIPField.text;
        startupIPField.text = "";
        Debug.Log(connectedIP);
        ServerConnectUI.SetActive(false);
        MenuUI.SetActive(true);

        NameDisplay.text = "Name: " + playerName;
        ConnectedIPDisplay.text = "Connected IP: " + connectedIP;
    }

    public void SwitchServer()
    {
        ServerConnectUI.SetActive(true);
        MenuUI.SetActive(false);
        ServerForms.SetActive(false);
        connectedIP = "";
        startupIPField.text = "";
}

    public void PlayButton()
    {
        if (MenuUI.activeSelf)
        {
            ServerForms.SetActive(true);
        }
    }
    public void QuitButton()
    {
        if (MenuUI.activeSelf)
        {
            Application.Quit();
        }
    }

    public void SwitchToJoin()
    {
        CreateGameUI.SetActive(false);
        JoinGameUI.SetActive(true);
        CodeInput.text = "";
    }

    public void SwitchToCreate()
    {
        CreateGameUI.SetActive(true);
        JoinGameUI.SetActive(false);
    }
    
    public void BackButtonIP()
    {
        CreateGameUI.SetActive(false);
        JoinGameUI.SetActive(false);
        ServerForms.SetActive(false);
    }

    public void CreateGame()
    {
        //Create Game lol
    }

    public void JoinExistingGame()
    {
        ConnectedCode = CodeInput.text;
        Debug.Log(ConnectedCode);
        CodeInput.text = "";
        if(ConnectedCode == "1234")
        {
            SceneManager.LoadScene("ChessBoard");
        }
        //Do join stuff lol
    }
}
