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
    private CursorPickup CP;
    private MenuHandlerScript menuHandler;

    private int turnPosition;
    private int movesMade;

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
            if (waiting)
            {
                if (host)
                {
                    menuHandler.UpdateWaitingRoom(true, GetGameInfo());
                }
                else
                {
                    menuHandler.UpdateWaitingRoom(false, GetGameInfo());
                }
                if (GetGameStarted())
                {
                    waiting = false;
                    Send("get_turn_pos::" + code);
                    turnPosition = int.Parse(Receive());
                    movesMade = 0;
                    StartCoroutine(LoadBoardScene());
                }
            }
            else if (playing)
            {
                string[] received = GetGame();
                int[] data = new int[received.Length];
                for (int i = 0; i < received.Length; i++)
                {
                    data[i] = int.Parse(received[i]);
                }
                if (turnPosition + 1 % data[3] != data[0])
                {
                    if (movesMade != data[1])
                    {
                        BHS.MakeMove(FetchLastMove());
                    }
                    movesMade = data[1];
                }
                if (data[0] == turnPosition)
                {
                    CP.PickupUpdate();
                }
            }
        }
    }

    IEnumerator LoadBoardScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        BHS = GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>();
        CP = GameObject.Find("Cursor").GetComponent<CursorPickup>();
        GameObject.Find("PieceInstantiator").GetComponent<PieceInstantiatorScript>().InstantiatePieces();
        playing = true;
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

    public string[] GetGame()
    {
        Send("get_game::" + code);
        return Receive().Split('~');
    }

    public string GetGameInfo()
    {
        Send("get_game_info::" + code);
        return Receive();
    }

    public bool GetGameStarted()
    {
        Send("get_game_started::" + code);
        return bool.Parse(Receive());
    }

    public void SendMove(string info)
    {
        Send("send_move::" + code + "::" + info);
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
        if (Receive().ToLower() == "true")
        {
            return true;
        }
        return false;
    }

    public int[] FetchLastMove()
    {
        Send("fetch_move::" + code);
        string[] data = Receive().Split('~');
        int[] moveData = new int[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            moveData[i] = int.Parse(data[i]);
        }
        return moveData;
    }
}
