using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool pauseMenuActive;

    public GameObject settingsMenuUI;
    private bool settingsMenuActive;

    public GameObject exitConfirmUI;
    private bool exitConfirmUIActive;

    public GameObject exitDefiniteConfirmUI;
    private bool exitDefiniteConfirmUIActive;

    public float masterVolumeSetting;
    public GameObject mVSlider;

    public AudioSource AS;

    void Start()
    {
        //Debug.Log(Application.persistentDataPath);
        AS = GameObject.Find("AudioPlayer").GetComponent<AudioSource>();

        string destination = Application.persistentDataPath + "/settings.dat";
        if (!File.Exists(destination)) WriteDefaultSettings();

        ResetUI();
        UpdateIngameSettings();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuActive){
                ResetUI();
                WriteSettings();
                pauseMenuActive = false;
            }
            else if (!pauseMenuActive){
                pauseMenuUI.SetActive(true);
                pauseMenuActive = true;
            }
        }
    }

    public void UpdateVolume()
    {
        masterVolumeSetting = mVSlider.GetComponent<Slider>().value;
    }

    public void SettingsMenu()
    {
        if (settingsMenuActive)
        {
            settingsMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
            settingsMenuActive = false; 
        }
        else if (!settingsMenuActive && !exitConfirmUIActive && !exitDefiniteConfirmUIActive)
        {
            settingsMenuUI.SetActive(true);
            pauseMenuUI.SetActive(false);
            settingsMenuActive = true;
        }
    }

    public void ExitGame()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        settingsMenuActive = false;
        exitConfirmUI.SetActive(true);
    }

    public void SecondExitGame()
    {
        exitDefiniteConfirmUI.SetActive(true);
        exitDefiniteConfirmUIActive = true;
    }

    public void ThirdExitGame()
    {
        SceneManager.LoadScene("Menu");
    }


    public void ResetUI()
    {
        pauseMenuUI.SetActive(false);
        exitConfirmUI.SetActive(false);
        exitDefiniteConfirmUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        pauseMenuActive = false;
        settingsMenuActive = false;
        exitConfirmUIActive = false;
        exitDefiniteConfirmUIActive = false;
    }

    public void WriteSettings()
    {
        string destination = Application.persistentDataPath + "/settings.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        AS.volume = masterVolumeSetting;

        GameData data = new GameData(masterVolumeSetting);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void UpdateIngameSettings()
    {
        string destination = Application.persistentDataPath + "/settings.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        mVSlider.GetComponent<Slider>().value = data.mVolume;
        AS.volume = data.mVolume;
    }

    public void WriteDefaultSettings()
    {
        string destination = Application.persistentDataPath + "/settings.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        AS.volume = masterVolumeSetting;

        GameData data = new GameData(masterVolumeSetting);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }
}
