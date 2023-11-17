using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : EntityControler
{

    private SPlayerSpawnData _lastCheckpoint;
    private SPlayerSpawnData _playerSpawnDataInNewScene;

    public GameObject playerPrefab;

    public static GameController Instance;
    [SerializeField] private Canvas canvasPausa;
    private GameObject _jugador;
    
    //Create Structure that holds the position and the sceneName of the checkpoint
    public struct SPlayerSpawnData
    {
        public Vector3 Position;
        public SceneObject Scene;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void Awake()
    {

        Time.timeScale = 1;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.gameObject);
            DontDestroyOnLoad(canvasPausa);
            //Load data from the save file
            //LoadData();
            _lastCheckpoint.Scene = SceneManager.GetActiveScene().name;
            _lastCheckpoint.Position = Vector3.zero;
            
        }
        else
        {
            Destroy(canvasPausa.gameObject);
            Destroy(gameObject);
            Destroy(_jugador);
            //canvasPausa.gameObject.SetActive(false);
            // canvasPausa = GameObject.Find("CanvasPausa").GetComponent<Canvas>();
            // canvasPausa.gameObject.SetActive(false);
            
        }

        if (Instance._lastCheckpoint.Scene != SceneManager.GetActiveScene().name)
        {
            PlayerSpawnInNewScene();
        }
        else
        {
            PlayerRespawn();
        }
        


    }

    public GameObject get_jugador()
    {
        Debug.Log("Jugador " + _jugador);
        return _jugador;
    }
    
    public SPlayerSpawnData GetCheckpoint()
    {
        return _lastCheckpoint;
    }

    public void SetCheckpoint(Vector3 cordinates)
    {
        Instance._lastCheckpoint.Position = cordinates;
        Instance._lastCheckpoint.Scene = SceneManager.GetActiveScene().name;
    }

    public void PlayerRespawn()
    {
        GameController.Instance._jugador = Instantiate(playerPrefab, transform.position = Instance._lastCheckpoint.Position, Quaternion.identity);
        // PlayerAudioScript.audioScript.RespawnPlayer();
    }
    
    public void PlayerSpawnInNewScene()
    {
        GameController.Instance._jugador = Instantiate(playerPrefab, transform.position = Instance._playerSpawnDataInNewScene.Position, Quaternion.identity);
    }
    
    public void SceneLoad(SPlayerSpawnData spawnData)
    {
        _playerSpawnDataInNewScene = spawnData;
        SceneManager.LoadScene(spawnData.Scene);
    }


    public void ChangeSceneMenu(string escena)
    {
        canvasPausa.gameObject.SetActive(false);
        SceneManager.LoadScene(escena);


    }public void ChangeScene(string escena)
    {
        SceneManager.LoadScene(escena);
    }

    
    // Pause Canvas
    public void Pause()
    {
        Time.timeScale = 0;
        canvasPausa.gameObject.SetActive(true);
    }
    
    public void PauseContinue()
    {
        Time.timeScale = 1;
        StartCoroutine(Wait());
        canvasPausa.gameObject.SetActive(false);
        _jugador.GetComponent<PlayerController>().EnablePlayerControls();
    }
    
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }
    
    // Pause Game Over
    
    public void GameOver()
    {
        Time.timeScale = 0;
        canvasPausa.gameObject.SetActive(true);
    }
    
}
