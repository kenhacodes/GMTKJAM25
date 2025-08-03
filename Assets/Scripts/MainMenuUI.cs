using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject levelSelectorPanel;
    public GameObject creditsPanel;

    public void ShowCredits()
    {
        mainPanel.SetActive(false);
        levelSelectorPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void BackFromCredits()
    {
        creditsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    void Start()
    {
        ShowMainPanel();
    }

    public void ShowLevelSelector()
    {
        mainPanel.SetActive(false);
        levelSelectorPanel.SetActive(true);
    }

    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        levelSelectorPanel.SetActive(false);
    }

    public void LoadEditor()
    {
        SceneManager.LoadScene("CharacterEditor");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("LevelCatbot");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Nivel2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Nivel3");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}