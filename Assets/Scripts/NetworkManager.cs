using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;

public class NetworkManager : MonoBehaviour
{
    private IPEndPoint endPoint;
    private Socket socket;
    private NetworkStream networkStream;
    private bool connected = false;
    [HideInInspector]
    public int playerID = -1;

    private string code;
    private bool playing = false;
    private bool waiting = false;
    private bool host = false;
    private BoardHandlerScript BHS;
    private MenuHandlerScript menuHandler;
    
	public static NetworkManager instance;

	void Awake()
	{
		if (instance != null)
		{
			if (instance != this)
			{
				Destroy(this.gameObject);
			}
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
	}

    private void Start() {
        GameObject menuHandlerGO = GameObject.Find("MenuHandler");
        if (menuHandlerGO){
            menuHandler = menuHandlerGO.GetComponent<MenuHandlerScript>();
        }
    }

    private void Update() {
        GameObject BHS_GO = GameObject.Find("BoardHandler");
        if (BHS_GO){
            BHS = BHS_GO.GetComponent<BoardHandlerScript>();
        }
        if (BHS != null){
            if (waiting && playerID == 0){
                if (StartGame()){
                    waiting = false;
                    playing = true;
                    Debug.Log("game started");
                    BHS.networkManager = this;
                    GetGame();
                }
            }
            else if (waiting){
                GetGame();
                if (BHS.active){
                    waiting = false;
                    playing = true;
                }
            }
            else if (playing){
                if (playerID != BHS.turn){
                    BHS.networkManager = this;
                    GetGame();
                }
            }
        }
    }

    public bool Connect()
    {
        if (!connected){
            try{
                endPoint = new IPEndPoint(menuHandler.connectedIP, 5555);
                socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(endPoint);

                networkStream = new NetworkStream(socket, true);

                connected = true;
                Debug.Log("Connected Successfully");
                return true;
            }
            catch (Exception e){
                Debug.Log(e.Message);
                Debug.Log("Connection Failed");
            }
        }
        else{
            Debug.Log("Already connected");
        }
        return false;
    }

    public void CloseConnection(){
        try
        {
            socket.Shutdown(SocketShutdown.Both);
        }
        finally
        {
            socket.Close();
        }
        connected = false;
        Debug.Log("Disconnected Successfully");
    }

    public void Send(string msg){
        var buffer = System.Text.Encoding.UTF8.GetBytes(msg);
        networkStream.Write(buffer, 0, buffer.Length);
    }

    public string Receive(){
        var response = new byte[1024];
        var bytesRead = networkStream.Read(response, 0, response.Length);
        var responseStr = System.Text.Encoding.UTF8.GetString(response);
        return responseStr.Replace("\x00", "");
    }

    public void CreateGame(){
        Send("create_game::4");
        code = Receive();
        Send("join_game::" + code);
        playerID = int.Parse(Receive());
        Debug.Log("Created game with code " + code);
        SceneManager.LoadScene(1);
        waiting = true;
    }

    public void JoinGame(){
        Send("get_game_code::" + menuHandler.ConnectedCode);
        string response = "null";
        response = Receive();
        if (response != "null"){
            code = response;
            Send("join_game::" + code);
            playerID = int.Parse(Receive());
            Debug.Log(playerID);
            SceneManager.LoadScene(1);
            waiting = true;
        }
        else{
            // uiManager.joinText.text = "Game not found";
        }
    }

    public void GetGame(){
        Send("get_game::" + code);
        BHS.Decode(Receive());
    }

    public void SendGame(){
        Send("send_game::" + code + "::" + BHS.Encode());
        Receive();
    }

    public bool StartGame(){
        Send("start_game::" + code);
        if (Receive() == "true"){
            return true;
        }
        else{
            return false;
        }
    }
}
