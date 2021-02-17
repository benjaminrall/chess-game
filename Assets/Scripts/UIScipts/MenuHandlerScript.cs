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
    public GameObject MenuPersistentUI;
    public GameObject JoinGameUI;
    public GameObject CreateGameUI;
    public GameObject WaitingRoom;
    public GameObject Presets;
    public Text startupIPField;
    public Text startupIPFieldOutput;
    public Text CodeInput;
    public Text CodeInputOutput;
    public Text PresetName;
    public Text PresetPlayers;
    public Text WaitingRoomCode;
    public Text WaitingRoomPlayers;
    public Text WaitingRoomInfo;
    public GameObject WaitingRoomStartButton;
    public string ConnectedCode;

    //Account Details
    public string playerName;
    public IPAddress connectedIP;

    public Text NameDisplay;
    public Text ConnectedIPDisplay;

    public NetworkManager networkManager;

    private int currentPreset;

    private bool connected;
    private bool moving;

    void Start()
    {
        ServerConnectUI.SetActive(true);
        MenuUI.SetActive(false);
        MenuPersistentUI.SetActive(false);
        WaitingRoom.SetActive(false);
    }

    public void SubmitFirstIP()
    {
        string ipText = startupIPField.text;
        if (ipText == ""){
            ipText = "192.168.1.104";
        }
        if (IPAddress.TryParse(ipText, out connectedIP)){
            if (networkManager.Connect()){
                MenuUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
                ServerConnectUI.SetActive(false);
                MenuUI.SetActive(true);
                MenuPersistentUI.SetActive(true);
                NameDisplay.text = "Name: " + playerName;
                ConnectedIPDisplay.text = "Connected IP: " + connectedIP;
                connected = true;
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
        connected = false;
        ServerConnectUI.SetActive(true);
        MenuUI.SetActive(false);
        MenuPersistentUI.SetActive(false);
        connectedIP = null;
        startupIPField.text = "";
    }

    public void PlayButton()
    {
        if (MenuUI.activeSelf)
        {
            CreateGameUI.SetActive(false);
            JoinGameUI.SetActive(false);
            SelectPreset(0);
            StartCoroutine(MoveUp());
        }
    }

    public void SettingsButton()
    {
        if (MenuUI.activeSelf)
        {
            StartCoroutine(MoveDown());
        }
    }

    IEnumerator MoveUp()
    {
        while (moving)
        {
            yield return null;
        }
        moving = true;
        LeanTween.moveY(MenuUI.GetComponent<RectTransform>(), 1440, 2).setEase(LeanTweenType.easeInOutExpo);
        yield return new WaitForSeconds(2.0f);
        moving = false;
    }

    IEnumerator MoveCentre()
    {
        while (moving)
        {
            yield return null;
        }
        moving = true;
        LeanTween.moveY(MenuUI.GetComponent<RectTransform>(), 0, 2).setEase(LeanTweenType.easeInOutExpo);
        yield return new WaitForSeconds(2.0f);
        MenuUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
        moving = false;
    }

    IEnumerator MoveDown()
    {
        while (moving)
        {
            yield return null;
        }
        moving = true;
        LeanTween.moveY(MenuUI.GetComponent<RectTransform>(), -1440, 2).setEase(LeanTweenType.easeInOutExpo);
        yield return new WaitForSeconds(2.0f);
        moving = false;
    }

    public void SkipIPButton()
    {
        MenuUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
        ServerConnectUI.SetActive(false);
        MenuUI.SetActive(true);
        MenuPersistentUI.SetActive(true);
        NameDisplay.text = "Name: ";
        ConnectedIPDisplay.text = "Connected IP: ";
        connected = false;
    }

    public void EditorButton()
    {
        if (MenuUI.activeSelf)
        {
            SceneManager.LoadScene(2);
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
    
    public void BackButton()
    {
        StartCoroutine(MoveCentre());
    }

    public void CreateGame()
    {
        if (connected)
        {
            networkManager.CreateGame(Presets.transform.GetChild(currentPreset).GetComponent<PresetSettings>().players);
        }
    }

    public void JoinExistingGame()
    {
        if (connected)
        {
            ConnectedCode = CodeInput.text;
            CodeInput.text = "";
            networkManager.JoinGame();
        }
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

    public void JoinWaitingRoom()
    {
        ServerConnectUI.SetActive(false);
        MenuUI.SetActive(false);
        MenuPersistentUI.SetActive(false);
        WaitingRoom.SetActive(true);
    }

    public void LeaveWaitingRoom()
    {
        ServerConnectUI.SetActive(false);
        MenuUI.SetActive(true);
        MenuPersistentUI.SetActive(true);
        WaitingRoom.SetActive(false);   
        CreateGameUI.SetActive(false);
        JoinGameUI.SetActive(false);
    }

    public void UpdateWaitingRoom(bool host, string data)
    {
        string code = data.Split(' ')[0];
        string players = data.Split(' ')[1];
        string maxPlayers = data.Split(' ')[2];
        WaitingRoomCode.text = "Code: " + code;
        WaitingRoomPlayers.text = $"Players Connected: {players}/{maxPlayers}";
        if (host)
        {
            if (players == maxPlayers)
            {
                WaitingRoomStartButton.SetActive(true);
                WaitingRoomInfo.text = "";
            }
            else
            {
                WaitingRoomStartButton.SetActive(false);
                WaitingRoomInfo.text = "Waiting for players...";
            }
        }
        else
        {
            if (players == maxPlayers)
            {
                WaitingRoomStartButton.SetActive(false);
                WaitingRoomInfo.text = "Waiting for host...";
            }
            else
            {
                WaitingRoomStartButton.SetActive(false);
                WaitingRoomInfo.text = "Waiting for players...";
            }
        }
    }
}
