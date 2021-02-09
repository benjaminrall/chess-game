using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;

public class MenuHandlerScript : MonoBehaviour
{
    public GameObject ServerConnectUI;
    public GameObject MenuUI;
    public GameObject ServerForms;
    public GameObject JoinGameUI;
    public GameObject CreateGameUI;
    public GameObject Presets;
    public Text startupIPField;
    public Text startupIPFieldOutput;
    public Text CodeInput;
    public Text CodeInputOutput;
    public Text PresetName;
    public Text PresetPlayers;
    public string ConnectedCode;

    //Account Details
    public string playerName;
    public IPAddress connectedIP;

    public Text NameDisplay;
    public Text ConnectedIPDisplay;

    public NetworkManager networkManager;

    private int currentPreset;

    void Start()
    {
        ServerConnectUI.SetActive(true);
        MenuUI.SetActive(false);
        ServerForms.SetActive(false);
    }

    public void SubmitFirstIP()
    {
        string ipText = startupIPField.text;
        if (ipText == ""){
            ipText = "192.168.1.104";
        }
        if (IPAddress.TryParse(ipText, out connectedIP)){
            if (networkManager.Connect()){
                ServerConnectUI.SetActive(false);
                MenuUI.SetActive(true);
                NameDisplay.text = "Name: " + playerName;
                ConnectedIPDisplay.text = "Connected IP: " + connectedIP;
            }
            else{
                startupIPFieldOutput.text = "Connection failed";
            }
        }
        else{
            startupIPFieldOutput.text = "Invalid IP input";
        }
        startupIPField.text = "";
    }

    public void SwitchServer()
    {
        networkManager.CloseConnection();
        ServerConnectUI.SetActive(true);
        MenuUI.SetActive(false);
        ServerForms.SetActive(false);
        connectedIP = null;
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
        SelectPreset(0);
    }
    
    public void BackButtonIP()
    {
        CreateGameUI.SetActive(false);
        JoinGameUI.SetActive(false);
        ServerForms.SetActive(false);
    }

    public void CreateGame()
    {
        networkManager.CreateGame(Presets.transform.GetChild(currentPreset).GetComponent<PresetSettings>().players);
    }

    public void JoinExistingGame()
    {
        ConnectedCode = CodeInput.text;
        CodeInput.text = "";
        networkManager.JoinGame();
    }

    public void SelectPreset(int index){
        for (int i = 0; i < Presets.transform.childCount; i++){
            Presets.transform.GetChild(i).gameObject.SetActive(i == index);
            if (i == index){
                PresetSettings s = Presets.transform.GetChild(i).GetComponent<PresetSettings>();
                PresetName.text = "Name: " + s.presetName;
                PresetPlayers.text = "Players: " + s.players.ToString();
            }
        }
    }

    public void ChangePreset(int change){
        currentPreset += change;
        if (currentPreset < 0){
            currentPreset = Presets.transform.childCount - 1;
        }
        if (currentPreset >= Presets.transform.childCount){
            currentPreset = 0;
        }
        SelectPreset(currentPreset);
    }
}
