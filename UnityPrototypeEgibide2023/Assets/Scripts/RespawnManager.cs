using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : EntityControler
{

    private static Vector3 _lastCheckpoint;
    private string _scene = "BasicMovementPrototypeScene";

    public GameObject playerPrefab;

    public static RespawnManager respawnManagerInstance;
    [SerializeField] private Canvas canvasPausa;
    private GameObject _jugador;
    
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
        
        
        if (respawnManagerInstance == null)
        {
            respawnManagerInstance = this;
            DontDestroyOnLoad(transform.gameObject);
            DontDestroyOnLoad(canvasPausa);
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
    
    public Vector3 GetCheckpoint()
    {
        return _lastCheckpoint;
    }

    public void SetCheckpoint(Vector3 cordinates)
    {
        _lastCheckpoint = cordinates;
        _scene = SceneManager.GetActiveScene().name;

    }

    public void PlayerRespawn()
    {
        RespawnManager.respawnManagerInstance._jugador = Instantiate(playerPrefab, transform.position = _lastCheckpoint, Quaternion.identity);
        Debug.Log("Jugador respawn " + _jugador); 
    }

    public void SceneLoad()
    {
        Debug.Log(_scene.ToString());
        SceneManager.LoadScene(_scene);
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
