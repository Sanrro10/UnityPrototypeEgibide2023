using System;
using System.Collections;
using Entities;
using Entities.Player.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General.Scripts
{
    public class GameController : EntityControler
    {

        private SPlayerSpawnData _lastCheckpoint;
        private SPlayerSpawnData _playerSpawnDataInNewScene;
        //Prueba - Guardado
        //Variable para SaveLoadManager
        private GameData gameData;
        //Fin: Prueba - Guardado

        public GameObject playerPrefab;

        public static GameController Instance;
        [SerializeField] private Canvas canvasPausa;
        [SerializeField] private PlayerData playerData;
        private GameObject _jugador;
        private bool _useCheckpoint;
    
        //Create Structure that holds the position and the sceneName of the checkpoint
        public struct SPlayerSpawnData
        {
            public Vector3 Position;
            public SceneObject Scene;
        }
    
        // Start is called before the first frame update
        void Start()
        {
            //reset unlocks (esto se deberia cambiar cuando metamos saves)
            playerData.airDashUnlocked = false;
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
                //Prueba Guardado
                
                gameData = SaveLoadManager.LoadGame(PlayerPrefs.GetString("slot"));
                if (gameData.isValid)
                {
                    _lastCheckpoint.Scene = gameData.spawnScene;
                    _lastCheckpoint.Position = gameData.spawnPosition;
                    _playerSpawnDataInNewScene.Scene = _lastCheckpoint.Scene;
                    _playerSpawnDataInNewScene.Position = _lastCheckpoint.Position;
                }
                else
                {
                    _lastCheckpoint.Scene = SceneManager.GetActiveScene().name;
                    _lastCheckpoint.Position = Vector3.zero;
                }
                //Fin: Prueba Guardado

            
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

            if (!_useCheckpoint)
            {
                PlayerSpawnInNewScene();
            }
            else
            {
                PlayerRespawn();
            }
        


        }

        public GameObject GetPlayerGameObject()
        {
            Debug.Log("Jugador " + _jugador);
            return _jugador;
        }

        public PlayerController GetPlayerController()
        {
            return _jugador.GetComponent<PlayerController>();
        }
    
        public SPlayerSpawnData GetCheckpoint()
        {
            return _lastCheckpoint;
        }

        public void SetCheckpoint(Vector3 cordinates)
        {
            Instance._lastCheckpoint.Position = cordinates;
            Instance._lastCheckpoint.Scene = SceneManager.GetActiveScene().name;
            //Prueba - Guardado
            gameData.spawnScene = _lastCheckpoint.Scene;
            gameData.spawnPosition = _lastCheckpoint.Position;
            gameData.isValid = true;
            /*Debug.LogWarning("Awake -> GameData.position: " + gameData.spawnPosition);
            Debug.LogWarning("Awake -> GameData.scene: " + gameData.spawnScene);*/
            //Fin: Prueba - Guardado
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

        public void SceneLoad(SPlayerSpawnData spawnData, bool useCheckpoint)
        {
            _useCheckpoint = useCheckpoint;
            _playerSpawnDataInNewScene = spawnData;
            SceneManager.LoadScene(spawnData.Scene);
        }


        public void ChangeSceneMenu()
        {
            DeletePersistentElement();
            SceneManager.LoadScene("Main Menu");
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
            _jugador.GetComponent<PlayerController>().EnablePlayerControls();
        }
        
        //Prueba Guardado
        public void SaveGame()
        {
            //Debug.Log("GameController -> Dentro del metodo SaveGame");
            SaveLoadManager.SaveGame(gameData, "");
        }
        
        public void LoadGame()
        {
            Debug.Log("GameController -> Dentro del metodo LoadGame");
            SaveLoadManager.LoadGame("");
        }

        public void DeletePersistentElement()
        {
            Destroy(canvasPausa.gameObject);
            Destroy(gameObject);
            Time.timeScale = 1;
        }

        //Fin: Prueba Guardado
    
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
}
