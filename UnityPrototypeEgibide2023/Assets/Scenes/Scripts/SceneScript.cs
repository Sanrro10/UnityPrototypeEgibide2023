using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour
{
    
    [SerializeField] private Canvas canvaSlotPartidas;
    [SerializeField] private Canvas canvasManuPrincipal;
    [SerializeField] private Canvas canvasConfirmDeleteGame;
    private string mainScene = "1.0.1 (Tutorial)";
    private bool confirm = false;
    private string guardarSlot;
  
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
        GameObject botones = GameObject.Find("CanvasSlotPartidas");
        if (botones != null)
        {
            Button[] allButtons = botones.GetComponentsInChildren<Button>();
            foreach (Button button in allButtons)
            {
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    Debug.Log(button.name);
                    if (button.name == "btn_Slot_1")
                    {
                        buttonText.text = "Nuevo texto"; // Cambia esto por el texto que quieras poner

                    }
                }
            }
        }

    }
    
    public void returnMainMenu()
    {
        canvasManuPrincipal.gameObject.SetActive(true);
        canvaSlotPartidas.gameObject.SetActive(false);
    }

    public void showConfirmationDeleteGame()
    {
        canvaSlotPartidas.gameObject.SetActive(false);
        canvasConfirmDeleteGame.gameObject.SetActive(true);
    }
    
    public void hideConfirmationDeleteGame()
    {
        canvasConfirmDeleteGame.gameObject.SetActive(false);
        canvaSlotPartidas.gameObject.SetActive(true);
    }

    public void confirmationDeleteGame(bool res)
    {
        confirm =  res;
        deleteGame(guardarSlot);
    }
    
    public void deleteGame(string slot)
    {
        
        //Revisar no funciona como deberia
        if (confirm)
        {
            GameData gameData = new GameData();
            SaveLoadManager.SaveGame(gameData, slot);
            hideConfirmationDeleteGame();
        }
        else
        {
            guardarSlot = slot;
            showConfirmationDeleteGame();
        }

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
