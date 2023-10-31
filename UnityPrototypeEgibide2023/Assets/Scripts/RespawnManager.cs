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

    public static RespawnManager RespawnManagerInstance;
    
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
        if (RespawnManagerInstance == null)
        {
            RespawnManagerInstance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PlayerRespawn();


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
        Instantiate(playerPrefab, transform.position = _lastCheckpoint, Quaternion.identity);
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
    
    public void PauseContinue()
    {
        Time.timeScale = 1;
        StartCoroutine(Wait());
        gameObject.SetActive(false);
    }
    
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }
    
}
