using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour
{
    
    [SerializeField] private Canvas canvaSlotPartidas;
    [SerializeField] private Canvas canvasManuPrincipal;
    private string mainScene = "1.0.1 (Tutorial)";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void Awake()
    {
        GameObject botones = GameObject.Find("CanvasSlotPartidas");
        if (botones != null)
        {
            Button[] allButtons = botones.GetComponentsInChildren<Button>();
            foreach (Button button in allButtons)
            {
                Text buttonText = button.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = "Nuevo texto"; // Cambia esto por el texto que quieras poner
                }
            }
        }
    }*/
    
    private void ChangeScene(string escena)
    {
        SceneManager.LoadScene(escena);
    }

    public void newLoadGame()
    {
        canvasManuPrincipal.gameObject.SetActive(false);
        canvaSlotPartidas.gameObject.SetActive(true);
    }
    
    public void returnMainMenu()
    {
        canvasManuPrincipal.gameObject.SetActive(true);
        canvaSlotPartidas.gameObject.SetActive(false);
    }

    public void deleteGame(string nameSaveFile)
    {
        //Revisar no funciona como deberia
        GameData gameData = new GameData();
        SaveLoadManager.SaveGame(gameData, nameSaveFile);
    }

    public void Exit()
    {
        // save any game data here
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void playGame(string slot)
    {
        PlayerPrefs.SetString("slot", slot);
        ChangeScene(mainScene);
    }
}
