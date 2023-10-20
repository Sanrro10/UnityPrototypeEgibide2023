using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : EntityControler
{

    private Vector3 _lastCheckpoint;
    private Scene _scene;

    public GameObject playerPrefab;
    private GameObject _player;
        
        
    // Start is called before the first frame update
    void Start()
    {
        PlayerRespawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public Vector3 GetCheckpoint()
    {
        return _lastCheckpoint;
    }

    public void SetCheckpoint(Vector3 cordinates)
    {
        _lastCheckpoint = cordinates;
        _scene = SceneManager.GetActiveScene();

    }

    public void PlayerRespawn()
    {
        _player = Instantiate(playerPrefab, transform.position = _lastCheckpoint, Quaternion.identity);
    }

    public void SceneLoad()
    {
        SceneManager.LoadScene(0);
    }
    
}
