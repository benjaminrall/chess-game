using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public Text[] ClockText;

    public int[] Milliseconds;
    public int[] Seconds;
    public int[] Minutes;

    public bool TimerActive = true;
    private BoardHandlerScript BHS;
    private static int playerCount;
    private int activePlayerColour;

    void Start()
    {
        BHS = GameObject.Find("BoardHandler").GetComponent<BoardHandlerScript>();
        playerCount = BHS.players;
        Milliseconds = new int[playerCount];
        Seconds = new int[playerCount];
        Minutes = new int[playerCount];
        FillInt(Milliseconds, 60);
        FillInt(Seconds, 59);
        FillInt(Minutes, 9);

        for (int i = 0; i < playerCount; i++)
        {
            ClockText[i].text = Minutes[i].ToString() + ":" + Seconds[i].ToString();
        }
    }

    void Update()
    {
        activePlayerColour = BHS.turn;
    }

    void FixedUpdate()
    {
        if (TimerActive == true && activePlayerColour != -1)
        {
            Milliseconds[activePlayerColour] -= 1;
            if (Milliseconds[activePlayerColour] <= 0)
            {
                Seconds[activePlayerColour] -= 1;
                Milliseconds[activePlayerColour] = 60;
            }
            if (Seconds[activePlayerColour] <= 0)
            {
                Minutes[activePlayerColour] -= 1;
                Seconds[activePlayerColour] = 60;
            }
            ClockText[activePlayerColour].text = Minutes[activePlayerColour].ToString() + ":" + Seconds[activePlayerColour].ToString();
        }
    }

    public void FillInt(int[] array, int FillNum)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = FillNum;
        }
    }
}