using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishedGameUI : MonoBehaviour
{
    public GameObject endGameUI;
    public Text endText;

    private TimerScript timer;
    private PauseMenu pauseMen;
    private BoardHandlerScript BHS;
    private PieceInstantiatorScript PISS;

    void Start()
    {
        timer = this.GetComponent<TimerScript>();
        pauseMen = this.GetComponent<PauseMenu>();
        BHS = GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>();
        PISS = GameObject.Find("PieceInstantiator").GetComponent<PieceInstantiatorScript>();

        endGameUI.SetActive(false);
    }

    public void ShowEndGameUI(string text)
    {
        endGameUI.SetActive(true);
        endText.text = text;
        timer.TimerActive = false;
        BHS.gameIsPlaying = false;
    }

    public void ResignButton()
    {
        ShowEndGameUI("Player Resigned");
    }

    public void RematchButton()
    {
        //Implement Rematch Check

        foreach (Transform child in BHS.transform)
        {
            child.GetComponent<Piece>().pieceX = -1;
            child.GetComponent<Piece>().pieceY = -1;
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        BHS.turn = 0;
        PISS.InstantiatePieces();
        endGameUI.SetActive(false);
        timer.ResetClock();
        BHS.gameIsPlaying = true;
    }
    public void ExitGame()
    {
        pauseMen.ThirdExitGame();
    }

    public void PlayerCheckmated(int colour)
    {
        string temp = BHS.colourNames[colour] + " was Checkmated";
        ShowEndGameUI(temp);
    }

    public void PlayerDisconnected()
    {
        ShowEndGameUI("Player Disconnected");
    }
}
