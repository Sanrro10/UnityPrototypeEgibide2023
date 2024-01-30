using System;
using System.Collections;
using Entities;
using Entities.Player.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Slider = UnityEngine.UIElements.Slider;

namespace General.Scripts
{
    public class GameController : MonoBehaviour
    {

        private SPlayerSpawnData _lastCheckpoint;
        private SPlayerSpawnData _playerSpawnDataInNewScene;
        private GameData gameData;
        private string mainSceneName  = "1.0.1 (Tutorial)";
        
        public GameObject playerPrefab;

        public static GameController Instance;
        [SerializeField] private Canvas canvasPausa;
        [SerializeField] private Canvas canvasGameOver;
        [SerializeField] private Canvas canvasOptions;
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
                DontDestroyOnLoad(canvasGameOver);
                DontDestroyOnLoad(canvasOptions);
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
            
            }
            else
            {
                Destroy(canvasPausa.gameObject);
                Destroy(canvasGameOver.gameObject);
                Destroy(canvasOptions.gameObject);
                Destroy(gameObject);
                Destroy(_jugador);
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
            gameData.spawnScene = _lastCheckpoint.Scene;
            gameData.spawnPosition = _lastCheckpoint.Position;
            gameData.isValid = true;
            SaveGame();
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
            canvasGameOver.gameObject.SetActive(false);
            _useCheckpoint = useCheckpoint;
            _playerSpawnDataInNewScene = spawnData;
            if (spawnData.Scene.GetSceneName() == null || spawnData.Scene.GetSceneName().Equals("") )
            {
                spawnData.Scene.SetSceneName(mainSceneName);
            }
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
        
        public void SaveGame()
        {
            SaveLoadManager.SaveGame(gameData, PlayerPrefs.GetString("slot"));
        }
        
        public void LoadGame()
        {
            Debug.Log("GameController -> Dentro del metodo LoadGame");
            SaveLoadManager.LoadGame(PlayerPrefs.GetString("slot"));
        }
        public void DeletePersistentElement()
        {
            Destroy(canvasPausa.gameObject);
            Destroy(canvasGameOver.gameObject);
            Destroy(canvasOptions.gameObject);
            Destroy(gameObject);
            Time.timeScale = 1;
        }
        
        public IEnumerator Wait()
        {
            yield return new WaitForSeconds(1);
        }
        
        public void GameOver()
        {
            Time.timeScale = 0;
            canvasGameOver.gameObject.SetActive(true);
        }
        
        public void showOptions()
        {
            Time.timeScale = 0;
            canvasPausa.gameObject.SetActive(false);
            canvasOptions.gameObject.SetActive(true);
            //canvasOptions.
        }
        
        public void hideOptions()
        {
            canvasOptions.gameObject.SetActive(false);
            canvasPausa.gameObject.SetActive(true);
        }
        
    }
}