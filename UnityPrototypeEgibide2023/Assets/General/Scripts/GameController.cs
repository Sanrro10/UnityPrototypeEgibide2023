using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Player.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

namespace General.Scripts
{
    public class GameController : MonoBehaviour
    {

        private SPlayerSpawnData _lastCheckpoint;
        public SPlayerSpawnData PlayerSpawnDataInNewScene;
        //This should be filled when loading the game, otherwise its not going to work;
        public SPlayerPersistentData PlayerPersistentDataBetweenScenes;
        private GameData gameData;
        private string mainSceneName  = "1.0.1 (Tutorial)";
        private string mainMenuName = "Main MenuEstados";
        public List<int> collectedItems = new List<int>();
        
        public GameObject playerPrefab;

        public static GameController Instance;
        [SerializeField] private GameObject menuPausa;
        [SerializeField] private GameObject menuGameOver;
        [SerializeField] private GameObject menuOptions;
        [SerializeField] private PlayerData playerData;
        private GameObject _jugador;
        public bool justDied = false;
        private bool _useCheckpoint;
    
        //Create Structure that holds the position and the sceneName of the checkpoint
        public struct SPlayerSpawnData
        {
            public Vector3 Position;
            public SceneObject Scene;
            public Vector3 GoToPosition;
            public bool OnTheLeft;
            public bool UseSceneChangeTrigger;
        }
        
        //Structure for data persistence between Scenes
        public struct SPlayerPersistentData
        {
            public int CurrentHealth;
            public GameObject[] PotionList;
            public GameObject SelectedPotion;
            public bool AirDashUnlocked;

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
            if (menuPausa == null)
            {
                menuPausa = GameObject.Find("menuPausa").GetComponent<GameObject>();
            }
            if (menuGameOver == null)
            {
                menuGameOver = GameObject.Find("menuGameOver").GetComponent<GameObject>();
            }
            menuPausa.gameObject.SetActive(false);
            menuGameOver.gameObject.SetActive(false);
            Time.timeScale = 1;
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(transform.gameObject);
                gameData = SaveLoadManager.LoadGame(PlayerPrefs.GetString("slot"));
                if (gameData.isValid && !Application.isEditor)
                {
                    _lastCheckpoint.Scene = gameData.spawnScene;
                    _lastCheckpoint.Position = gameData.spawnPosition;
                    PlayerSpawnDataInNewScene.Scene = _lastCheckpoint.Scene;
                    PlayerSpawnDataInNewScene.Position = _lastCheckpoint.Position;
                    PlayerSpawnDataInNewScene.GoToPosition = _lastCheckpoint.Position;
                    PlayerPersistentDataBetweenScenes.CurrentHealth = 100;
                    PlayerPersistentDataBetweenScenes.AirDashUnlocked = gameData.AirDashUnlocked;
                    PlayerPersistentDataBetweenScenes.PotionList = gameData.PotionList;
                    }
                    else
                    {
                        _lastCheckpoint.Scene = SceneManager.GetActiveScene().name;
                        _lastCheckpoint.Position = Vector3.zero;
                    }

            }
            else
            {
                Destroy(gameObject);
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
            gameData.collectedItems = collectedItems;
            gameData.CurrentHealth = PlayerPersistentDataBetweenScenes.CurrentHealth;
            gameData.PotionList = PlayerPersistentDataBetweenScenes.PotionList;
            gameData.SelectedPotion = PlayerPersistentDataBetweenScenes.SelectedPotion;
            gameData.AirDashUnlocked = PlayerPersistentDataBetweenScenes.AirDashUnlocked;
            SaveGame();
        }

        public void PlayerRespawn()
        {
            GameController.Instance._jugador = Instantiate(playerPrefab, transform.position = Instance._lastCheckpoint.Position, Quaternion.identity);
            // PlayerAudioScript.audioScript.RespawnPlayer();
        }
    
        public void PlayerSpawnInNewScene()
        {
            if (Instance.PlayerSpawnDataInNewScene.Position == Vector3.zero &&
                Instance.PlayerSpawnDataInNewScene.GoToPosition == Vector3.zero &&
                Instance.PlayerSpawnDataInNewScene.UseSceneChangeTrigger)
            {
                CalculatePlayerSpawnPosition();
                
            }
            GameController.Instance._jugador = Instantiate(playerPrefab, transform.position = Instance.PlayerSpawnDataInNewScene.Position, Quaternion.identity);
            GameController.Instance._jugador.SendMessage(nameof(PlayerController.CheckSceneChanged), SendMessageOptions.RequireReceiver);
            //GameObject.FindWithTag("Player").SendMessage((nameof(PlayerController.OnSceneChange)));
        }
        
        /*If no spawn position is given on the prefab of scene change, it calculates the spawnpoint of the player
         so that it walks from 3 units back from the spawn, towards 3 units forward*/
        private void CalculatePlayerSpawnPosition()
        {
            GameObject[] listado = GameObject.FindGameObjectsWithTag("Checkpoint");
            UnityEngine.GameObject correctChild = null;
            foreach (GameObject currentChild in listado)
            {
                //SceneChangeTrigger comprobacionScript = currentChild.GetComponent<SceneChangeTrigger>();
                if (currentChild.GetComponent<SceneChangeTrigger>() != null)
                {
                    if (correctChild is null)
                    {
                        correctChild = currentChild;
                    }

                    if (Instance.PlayerSpawnDataInNewScene.OnTheLeft) //Search for the leftmost one
                    {
                        if (currentChild.transform.position.x < correctChild.transform.position.x)
                        {
                            correctChild = currentChild;
                        }
                    }
                    else //Search for the rightmost one
                    {
                        if (currentChild.transform.position.x > correctChild.transform.position.x)
                        {
                            correctChild = currentChild;
                        }
                    }
                }
            }

            Vector3 tempVector = correctChild!.transform.position;
            
            if (Instance.PlayerSpawnDataInNewScene.OnTheLeft)
            {
                Instance.PlayerSpawnDataInNewScene.Position = new Vector3(tempVector.x - 3,
                    tempVector.y - 5.05f,
                    tempVector.z);
                Instance.PlayerSpawnDataInNewScene.GoToPosition = new Vector3(tempVector.x + 3,
                    tempVector.y - 5.05f,
                    tempVector.z);
            }
            else
            {
                Instance.PlayerSpawnDataInNewScene.Position = new Vector3(tempVector.x + 3,
                    tempVector.y - 5.05f,
                    tempVector.z);
                Instance.PlayerSpawnDataInNewScene.GoToPosition = new Vector3(tempVector.x - 3,
                    tempVector.y - 5.05f,
                    tempVector.z);
            }

        }
        
        public void SceneLoad(SPlayerSpawnData spawnData, bool useCheckpoint)
        {
            menuGameOver.SetActive(false);
            _useCheckpoint = useCheckpoint;
            PlayerSpawnDataInNewScene = spawnData;
            if (spawnData.Scene.GetSceneName() == null || spawnData.Scene.GetSceneName().Equals("") )
            {
                spawnData.Scene.SetSceneName(mainSceneName);
            }
            SceneManager.LoadScene(spawnData.Scene);
        }
        public void ChangeSceneMenu()
        {
            DeletePersistentElement();
            SceneManager.LoadScene(mainMenuName);
        }
        
        public void ChangeScene(string escena)
        {
            SceneManager.LoadScene(escena);
        }
        public void Pause()
        {
            Time.timeScale = 0;
            menuPausa.SetActive(true);
        }
    
        public void PauseContinue()
        {
            Time.timeScale = 1;
            StartCoroutine(Wait());
            menuPausa.SetActive(false);
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
            _useCheckpoint = true;
        }
        public void DeletePersistentElement()
        {
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
            _jugador.GetComponent<PlayerController>().DisablePlayerControls();
            menuGameOver.SetActive(true);
        }
        
        public void showOptions()
        {
            _jugador.GetComponent<PlayerController>().DisablePlayerControls();
            menuPausa.SetActive(false);
            menuOptions.SetActive(true);
        }
        
        public void hideOptions()
        {
            menuPausa.SetActive(true);
            menuOptions.SetActive(false);
           
        }
        public void CallSceneLoad()
        {
            GameController.Instance.SceneLoad(GameController.Instance.GetCheckpoint(),true);
            //gameControler.GetComponent<GameController>().SceneLoad(gameControler.GetComponent<GameController>().GetCheckpoint());
        }
    }
}