using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : EntityControler
{

    private static Vector3 _lastCheckpoint;
    private string _scene;

    public GameObject playerPrefab;

    public static RespawnManager respawnManagerInstance;
    
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
    
}
