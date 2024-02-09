using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour
{
    
    [SerializeField] private Canvas canvaSlotPartidas;
    [SerializeField] private Canvas canvasManuPrincipal;
    [SerializeField] private Canvas canvasConfirmDeleteGame;
    [SerializeField] private Canvas canvasOptions;
    
    private string mainScene = "1.0.1 (Tutorial)";
    private string textSlotDefault = "Nueva Partida";
    private bool confirm = false;
    private string guardarSlot;
    private List <GameData>  gameDatas;
    private Button btnSlot1;
    private Button btnSlot2;
    private Button btnSlot3;
    private Button btnSlot1Delete;
    private Button btnSlot2Delete;
    private Button btnSlot3Delete;

    public void Start()
    {
        Destroy(GameObject.Find("GameControler"));
        Time.timeScale = 1;
        fillGameDatas();
    }

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
            bool btn_Slot1_delete = false;
            bool btn_Slot2_delete = false;
            bool btn_Slot3_delete = false;
            foreach (Button button in allButtons)
            {
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    if (button.name == "btn_Slot_1")
                    {
                        btnSlot1 = button;
                        string slotDataInfo = (gameDatas[0].isValid) ? gameDatas[0].spawnScene.GetSceneName() : textSlotDefault;
                        if (slotDataInfo.Equals(textSlotDefault))
                        {
                            btn_Slot1_delete = true;
                        }
                        buttonText.text = slotDataInfo;
                    }
                    if (button.name == "btn_Slot_2")
                    {
                        btnSlot2 = button;
                        string slotDataInfo = (gameDatas[1].isValid) ? gameDatas[1].spawnScene.GetSceneName(): textSlotDefault;
                        if (slotDataInfo.Equals(textSlotDefault))
                        {
                            btn_Slot2_delete = true;
                        }
                        buttonText.text = slotDataInfo;
                    }
                    if (button.name == "btn_Slot_3")
                    {
                        btnSlot3 = button;
                        string slotDataInfo = (gameDatas[2].isValid) ? gameDatas[2].spawnScene.GetSceneName() : textSlotDefault;
                        if (slotDataInfo.Equals(textSlotDefault))
                        {
                            btn_Slot3_delete = true;
                        }
                        buttonText.text = slotDataInfo;
                    }
                    if (button.name == "btn_Slot_1_delete")
                    {
                        btnSlot1Delete = button;
                        if (btn_Slot1_delete)
                        {
                            button.gameObject.SetActive(false);
                        }
                    }
                    if (button.name == "btn_Slot_2_delete")
                    {
                        btnSlot2Delete = button;
                        if (btn_Slot2_delete)
                        {
                            button.gameObject.SetActive(false);
                        }
                    }
                    if (button.name == "btn_Slot_3_delete")
                    {
                        btnSlot3Delete = button;
                        if (btn_Slot3_delete)
                        {
                            button.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

    }
    
    public void returnMainMenu()
    {
        canvasManuPrincipal.gameObject.SetActive(true);
        canvaSlotPartidas.gameObject.SetActive(false);
        canvasOptions.gameObject.SetActive(false);
        fillGameDatas();
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
        if (confirm)
        {
            GameData gameData = new GameData();
            SaveLoadManager.SaveGame(gameData, slot);
            if (slot.Equals("1"))
            {
                btnSlot1.GetComponentInChildren<TMP_Text>().text = textSlotDefault;
                btnSlot1Delete.gameObject.SetActive(false);
            }
            if (slot.Equals("2"))
            {
                btnSlot2.GetComponentInChildren<TMP_Text>().text = textSlotDefault;
                btnSlot2Delete.gameObject.SetActive(false);
            }
            if (slot.Equals("3"))
            {
                btnSlot3.GetComponentInChildren<TMP_Text>().text = textSlotDefault;
                btnSlot3Delete.gameObject.SetActive(false);
            }
            hideConfirmationDeleteGame();
        }
        else
        {
            guardarSlot = slot;
            showConfirmationDeleteGame();
        }

    }
    
    public void playGame(string slot)
    {
        PlayerPrefs.SetString("slot", slot);
        ChangeScene(mainScene);
    }
    
    private List<GameData> fillGameDatas()
    {
        return gameDatas = SaveLoadManager.ListDataAllGame();
    }

    public void showMenuOptions()
    {
        canvasManuPrincipal.gameObject.SetActive(false);
        canvasOptions.gameObject.SetActive(true);
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
    
}
