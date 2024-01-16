using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    
    [SerializeField] private Canvas canvaSlotPartidas;
    [SerializeField] private Canvas canvasManuPrincipal;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeScene(string escena)
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
        SaveLoadManager.SaveGame(gameData);
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
