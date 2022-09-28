using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using WJ_Controller;
using UnityEngine.SceneManagement;

namespace WJ
{
    public class GameManager : Entity
    {
        [SerializeField] private GameObject prefabCharcterObject = null;
        [SerializeField] private GameObject friesbeeObject = null;
        [SerializeField] private List<GameObject> mapObject = new List<GameObject>();
        [SerializeField] private float RoundMaxTime = 90.0f;
        [SerializeField] private Vector2 terrainSize = new Vector2(20.0f,10.0f);
        [SerializeField] private Vector3[] characterPosition = {new Vector3(0,0,0),new Vector3(0,0,0)};
        private FrisbieController frisbie = null;       
        private GameObject characterLeft = null; 
        private GameObject characterRight = null;
        private bool endGame = false;
        private float gameTime = 0.0f;
        private int lastgameTime = 0;
        private int scoreRight = 0;
        private int scoreLeft = 0;
        private int SetLeftCount = 0;
        private int SetRightCount = 0;
        private int round = 0;
        private bool pauseGame = false;

        [Header("Interface")]
        [SerializeField] private TextMeshProUGUI timeText = null;
        [SerializeField] private TextMeshProUGUI scoreLeftText = null;
        [SerializeField] private TextMeshProUGUI scoreRightText = null;
        [SerializeField] private GameObject roundSetObject = null;
        [SerializeField] private GameObject pauseObj = null;
        [SerializeField] private GameObject winObj = null;
        [SerializeField] private TextMeshProUGUI textRoundScoreLeft = null;
        [SerializeField] private TextMeshProUGUI textRoundScoreRight = null;

        private static int indiceMap = 0; 
        private static int indicePlayerLeft = 0;
        private static int indicePlayerRight = 0;
        private static CharacterMode characterModeRight = CharacterMode.RandomBot; 

        public FrisbieController Frisbie{
            get{return frisbie;}
        }

        public Vector2 TerrainSize
        {
            get{return terrainSize;}
        }

        public static int IndicePlayerLeft
        {
            get{return indicePlayerLeft;}
            set{indicePlayerLeft = value;}
        }

        public static int IndicePlayerRight
        {
            get{return indicePlayerRight;}
            set{indicePlayerRight = value;}
        }

        public static int IndiceMap
        {
            get{return indiceMap;}
            set{indiceMap = value;}
        }

        public static CharacterMode CharacterModeRight
        {
            get{return characterModeRight;}
            set{characterModeRight = value;}
        }

        public void AddScorePoint(Faction f,int value = 1)
        {
            if(Faction.Left == f)
            {
                scoreLeft+=value;
            }
            else
            {
                scoreRight+=value;
            }
            MajUIScore();
            StartCoroutine(ResetWin(f));
        }

        private void MajUIScore()
        {
            scoreLeftText.text = scoreLeft.ToString("00");
            scoreRightText.text = scoreRight.ToString("00");
        }
        public static GameManager Instance
        {
            get
            {
                return instance;
            }
        }

        private static GameManager instance = null;



        protected override void Awake()
        {
            base.Awake();
            instance = this;
            gameTime = RoundMaxTime;
            endGame = false;
        }

        public void Start()
        {
            InputManager.InputJoueur.Player.Pause.performed += PauseGame;
            InitGame();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputManager.InputJoueur.Player.Pause.performed -= PauseGame;
        }

        public void InitGame()
        {
            frisbie = Instantiate(friesbeeObject,Vector3.zero,Quaternion.identity).GetComponent<FrisbieController>();
            frisbie.Reset();
            Instantiate(mapObject[indiceMap],Vector3.zero,Quaternion.identity);
            characterLeft = Instantiate(prefabCharcterObject,characterPosition[0],Quaternion.identity);
            InitCharacter(characterLeft,RessourceManager.Instance.CharacterInfos[indicePlayerLeft],Faction.Left);
            characterRight = Instantiate(prefabCharcterObject,characterPosition[1],Quaternion.identity);
            InitCharacter(characterRight,RessourceManager.Instance.CharacterInfos[indicePlayerRight],Faction.Right);
            StartThrow(Random.Range(0,2) == 0 ? Faction.Left : Faction.Right);
            SeeCursor(false);
            CharacterCanMove(true);
        }

        public void CharacterCanMove(bool state)
        {
            characterLeft.GetComponent<Character>().CanMove = state;
            characterRight.GetComponent<Character>().CanMove = state;
        }

        public void InitCharacter(GameObject obj,CharacterInfo ci,Faction f)
        {
            Character p = null;
            if(f == Faction.Left || characterModeRight == CharacterMode.Player)
            {
                p = obj.AddComponent<Player>();
            }
            else if(characterModeRight == CharacterMode.RandomBot)
            {
                p = obj.AddComponent<RandomBot>();             
            }
            else if(characterModeRight == CharacterMode.MCTSBot)
            {
                p = obj.AddComponent<MCTSBot>();             
            }
            p.InitCharacter(ci,f);
        }

        public void SeeCursor(bool state)
        {
            Cursor.visible = state;
            if(state)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public IEnumerator ResetWin(Faction f)
        {
            frisbie.Reset();
            CharacterCanMove(false);
            ResetPlayerPosition();   
            if(gameTime > 1.5f)
            {
                yield return new WaitForSeconds(1.5f);
                StartThrow(f);
            }
            CharacterCanMove(true);
        }

        public void ResetPlayerPosition()
        {
            characterLeft.GetComponent<Character>().ResetSpawnPosition();
            characterRight.GetComponent<Character>().ResetSpawnPosition();
        }


        public void StartThrow(Faction f)
        {
            Vector3 dir;
            if(f == Faction.Left)
            {
                dir = (characterLeft.transform.position-frisbie.transform.position).normalized;
            }
            else
            {
                dir = (characterRight.transform.position-frisbie.transform.position).normalized;
            }
            dir.y = 0;
            frisbie.Throw(dir,10.0f);
        }

        public void BackMenu()
        {
            SeeCursor(true);
            FadeScreenManager.FadeIn();
            FadeScreenManager.OnFadeInComplete += LoadScene;
        }

        private void LoadScene()
        {
            FadeScreenManager.OnFadeInComplete -= LoadScene;
            SceneManager.LoadScene("Menu",LoadSceneMode.Single);
        }

        public IEnumerator WinResetRound()
        {
            frisbie.Reset();
            CharacterCanMove(false);
            ResetPlayerPosition(); 
            if(scoreLeft > scoreRight)
            {
                SetLeftCount++;
            }
            else if(scoreRight > scoreLeft)
            {
                SetRightCount++;
            }
            scoreRight = 0;
            scoreLeft = 0;
            textRoundScoreLeft.text = SetLeftCount.ToString("00");
            textRoundScoreRight.text = SetRightCount.ToString("00");
            MajUIScore();
            roundSetObject.SetActive(true);
            Faction f;
            if(isWinBO3(out f))
            {
                winObj.SetActive(true);
                yield return new WaitForSeconds(4.0f);
                BackMenu();
            }
            else
            {
                yield return new WaitForSeconds(2.0f);
                roundSetObject.SetActive(false);
                gameTime = RoundMaxTime;
                endGame = false;
                StartThrow(Random.Range(0,2) == 0 ? Faction.Left : Faction.Right);
            }
            CharacterCanMove(true);
        }

        public bool isWinBO3(out Faction faction)
        {
            if(SetLeftCount == 3 || (SetLeftCount == 2 && SetRightCount == 0))
            {
                faction = Faction.Left;
                return true;
            }
            else if(SetRightCount == 3 || (SetRightCount == 2 && SetLeftCount == 0))
            {
                faction = Faction.Right;
                return true;
            }
            faction = Faction.Left;
            return false;
        }
        

        public void PauseGame(InputAction.CallbackContext ctx)
        {
            pauseGame =!pauseGame;
            pauseObj.SetActive(pauseGame);
            SeeCursor(pauseGame);
            GamePauseManager.Instance.SetPause(pauseGame ? GamePause.Pause : GamePause.GamePlay);
        }

        void Update()
        {
            gameTime -= Time.deltaTime;
            if(!endGame && gameTime <= 0.0f)
            {
                endGame = true;
                timeText.text = "00";
                StartCoroutine(WinResetRound());
            }
            if(!endGame && lastgameTime != (int)gameTime)
            {
                lastgameTime = (int)gameTime;
                timeText.text = lastgameTime.ToString("00");
            }
        }
    }
}