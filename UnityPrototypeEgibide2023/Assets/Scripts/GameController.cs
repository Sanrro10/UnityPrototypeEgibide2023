using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : EntityControler
{

    private static SPlayerSpawnData _lastSPlayerSpawn;
    private string _scene = "BasicMovementPrototypeScene";

    public GameObject playerPrefab;

    public static GameController Instance;
    [SerializeField] private Canvas canvasPausa;
    private GameObject _jugador;
    
    //Create Structure that holds the position and the sceneName of the checkpoint
    public struct SPlayerSpawnData
    {
        public Vector3 Position;
        public string SceneName;
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
        
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.gameObject);
            DontDestroyOnLoad(canvasPausa);
            //Load data from the save file
            //LoadData();
            
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
        
            
        PlayerRespawn();


    }

    public GameObject get_jugador()
    {
        Debug.Log("Jugador " + _jugador);
        return _jugador;
    }
    
    public SPlayerSpawnData GetCheckpoint()
    {
        return _lastSPlayerSpawn;
    }

    public void SetCheckpoint(Vector3 cordinates)
    {
        _lastSPlayerSpawn.Position = cordinates;
        _lastSPlayerSpawn.SceneName = SceneManager.GetActiveScene().name;
        _scene = SceneManager.GetActiveScene().name;

    }

    public void PlayerRespawn()
    {
        GameController.Instance._jugador = Instantiate(playerPrefab, transform.position = _lastSPlayerSpawn.Position, Quaternion.identity);
        Debug.Log("Jugador respawn " + _jugador); 
    }
    
    public void SpawnPlayerInPosition(Vector3 position)
    {
        GameController.Instance._jugador = Instantiate(playerPrefab, transform.position = position, Quaternion.identity);
    }
    
    public void SceneLoad(SPlayerSpawnData spawnData)
    {
        SceneManager.LoadScene(spawnData.SceneName);
        SpawnPlayerInPosition(spawnData.Position);
    }


    public void ChangeScene(string escena)
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
        _jugador.GetComponent<PlayerMovement>().EnablePlayerControls();
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
