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
    private const string VERSION = "0.0";

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

    private void Start() 
    {
        GameObject menuHandlerGO = GameObject.Find("MenuHandler");
        if (menuHandlerGO)
        {
            menuHandler = menuHandlerGO.GetComponent<MenuHandlerScript>();
        }
    }

    private void Update() 
    {
        if (connected && (waiting || playing))
        {
            if (GetHost())
            {
                host = true;
            }
            if (waiting && host)
            {
                menuHandler.UpdateWaitingRoom(true, GetGameInfo());
                /*                
                if (StartGame())
                {
                    waiting = false;
                    playing = true;
                    Debug.Log("game started");
                    BHS.networkManager = this;
                    GetGame();
                }
                */
            }
            else if (waiting)
            {
                menuHandler.UpdateWaitingRoom(false, GetGameInfo());
            }
        }
    }

    public bool Connect()
    {
        if (!connected)
        {
            try
            {
                endPoint = new IPEndPoint(menuHandler.connectedIP, 5555);
                socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(endPoint);

                networkStream = new NetworkStream(socket, true);

                Send("version_check::" + VERSION.ToString());
                string r = Receive();
                if (r == "true")
                {
                    connected = true;
                    Debug.Log("Connected Successfully");
                    return true;
                }
                else if (r == "false_c")
                {
                    menuHandler.startupIPFieldOutput.text = "Outdated Client";
                }
                else if (r == "false_s")
                {
                    menuHandler.startupIPFieldOutput.text = "Outdated Server";
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log("Connection Failed");
                menuHandler.startupIPFieldOutput.text = "Connection failed";
            }
        }
        else
        {
            Debug.Log("Already connected");
            menuHandler.startupIPFieldOutput.text = "Already connected";
        }
        return false;
    }

    public void CloseConnection()
    {
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

    public void Send(string msg)
    {
        var buffer = System.Text.Encoding.UTF8.GetBytes(msg);
        networkStream.Write(buffer, 0, buffer.Length);
    }

    public string Receive()
    {
        var response = new byte[1024];
        var bytesRead = networkStream.Read(response, 0, response.Length);
        var responseStr = System.Text.Encoding.UTF8.GetString(response);
        return responseStr.Replace("\x00", "");
    }

    public void CreateGame(int players)
    {
        Send("create_game::" + players.ToString());
        code = Receive();
        Send("join_game::" + code);
        playerID = int.Parse(Receive());
        Debug.Log("Created game with code " + code);
        menuHandler.JoinWaitingRoom();
        waiting = true;
        host = true;
    }

    public void JoinGame()
    {
        Send("get_game_code::" + menuHandler.ConnectedCode);
        string response = "null";
        response = Receive();
        if (response != "null" && response != "full")
        {
            code = response;
            Send("join_game::" + code);
            playerID = int.Parse(Receive());
            menuHandler.JoinWaitingRoom();
            waiting = true;
        }
        else if (response == "full")
        {
            menuHandler.CodeInputOutput.text = "Game full";
        }
        else
        {
            menuHandler.CodeInputOutput.text = "Game not found";
        }
    }

    public void GetGame()
    {
        Send("get_game::" + code);
        BHS.Decode(Receive());
    }

    public string GetGameInfo()
    {
        Send("get_game_info::" + code);
        return Receive();
    }

    public void SendGame()
    {
        Send("send_game::" + code + "::" + BHS.Encode());
        Receive();
    }

    public void StartGame()
    {
        Send("start_game::" + code);
        if (Receive() == "true")
        {
            Debug.Log("Game started");
        }
        else
        {
            Debug.Log("Game failed to start");
        }
    }

    public void LeaveGame()
    {
        Send("leave_game::" + code);
        if (Receive() == "true")
        {
            Debug.Log("Left game");
        }
        else
        {
            Debug.Log("Leaving game failed");
        }
        waiting = false;
        menuHandler.LeaveWaitingRoom();
    }

    public bool GetHost()
    {
        Send("get_host::" + code);
        if (Receive() == "True")
        {
            return true;
        }
        return false;
    }
}
