using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using WJ_Controller;
using UnityEngine.SceneManagement;
using WJ_MCTS;

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
        [SerializeField] private int maxScoreToSet = 24;
        private GameState gameState;
        private FrisbieController frisbie = null;       
        private Character characterLeft = null; 
        private Character characterRight = null;

        #region Getter
        public FrisbieController Frisbie { get {return frisbie;} }
        public Character CharacterLeft { get {return characterLeft;} }
        public Character CharacterRight { get {return characterRight;} }
        #endregion 

        [Header("Interface")]
        [SerializeField] private TextMeshProUGUI timeText = null;
        [SerializeField] private TextMeshProUGUI scoreLeftText = null;
        [SerializeField] private TextMeshProUGUI scoreRightText = null;
        [SerializeField] private GameObject roundSetObject = null;
        [SerializeField] private GameObject pauseObj = null;
        [SerializeField] private GameObject winObj = null;
        [SerializeField] private TextMeshProUGUI textRoundScoreLeft = null;
        [SerializeField] private TextMeshProUGUI textRoundScoreRight = null;
        private int lastgameTime = 0;
        private int characterId = 0;
        private bool pauseGame = false;

        #region dataStatic
        private static int indiceMap = 0; 
        private static int indicePlayerLeft = 0;
        private static int indicePlayerRight = 0;
        private static CharacterMode characterModeRight = CharacterMode.RandomBot; 
        
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
        #endregion

        public void AddScorePoint(GameState gs, Faction f,int value = 1)
        {
            if(Faction.Left == f)
            {
                gs.GameManagerData.scoreLeft+=value;
            }
            else
            {
                gs.GameManagerData.scoreRight+=value;
            }
            if(gs.GameManagerData.isCurrentGame)
            {
                MajUIScore(gs.GameManagerData);
            }
            ResetWin(gs,f);
        }

        private void MajUIScore(GameManagerData data)
        {
            scoreLeftText.text = data.scoreLeft.ToString("00");
            scoreRightText.text = data.scoreRight.ToString("00");
        }
        public static GameManager Instance
        {
            get{return instance;}
        }

        private static GameManager instance = null;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        public void Start()
        {
            InputManager.InputJoueur.Player.Pause.performed += PauseGame;
            InitData();
            InitGame();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputManager.InputJoueur.Player.Pause.performed -= PauseGame;
        }

        public void InitData()
        {
            gameState.GameManagerData.gameTime = RoundMaxTime;
            gameState.GameManagerData.endSet = false;
            gameState.GameManagerData.endGame = false;
            gameState.GameManagerData.isCurrentGame = true;
        }

        public void InitGame()
        {
            frisbie = Instantiate(friesbeeObject,Vector3.zero,Quaternion.identity).GetComponent<FrisbieController>();
            frisbie.InitFrisbie(terrainSize,gameState);
            Instantiate(mapObject[indiceMap],Vector3.zero,Quaternion.identity);
            characterLeft = InitCharacter(
                Instantiate(prefabCharcterObject,characterPosition[characterId],Quaternion.identity),
                RessourceManager.Instance.CharacterInfos[indicePlayerLeft],
                Faction.Left,
                characterPosition[characterId]);
            characterRight = InitCharacter(
                Instantiate(prefabCharcterObject,characterPosition[characterId],Quaternion.identity),
                RessourceManager.Instance.CharacterInfos[indicePlayerRight],
                Faction.Right,
                characterPosition[characterId]);
            StartThrow(gameState,Random.Range(0,2) == 0 ? Faction.Left : Faction.Right);
            SeeCursor(false);
            CharacterCanMove(true);
        }

        public Character InitCharacter(GameObject obj,CharacterInfo ci,Faction f,Vector3 spawnPosition)
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
            gameState.CharacterDataDictionary[characterId++] = new KeyValuePair<Character,CharacterData>(p,new CharacterData());
            p.InitCharacter(ci,gameState,f,terrainSize,spawnPosition);
            return p;
        }

        public void CharacterCanMove(bool state)
        {
            //characterLeft.CanMove = state;
            //characterRight.CanMove = state;
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

        public void ResetWin(GameState gs,Faction f)
        {
            frisbie.Reset(gs.FrisbiData);
            CharacterCanMove(false);
            ResetPlayerPosition(gs);
            /*if(gs.GameManagerData.gameTime > 1.5f)
            {
                yield return new WaitForSeconds(1.5f);
                StartThrow(f);
            }
            CharacterCanMove(true);*/
        }

        public void ResetPlayerPosition(GameState gs)
        {
            characterLeft.ResetSpawnPosition(gs);
            characterRight.ResetSpawnPosition(gs);
        }


        public void StartThrow(GameState gs,Faction f)
        {
            Vector3 dir;
            if(f == Faction.Left)
            {
                dir = (gs.CharacterDataDictionary[0].Value.position-gs.FrisbiData.position).normalized;
            }
            else
            {
                dir = (gs.CharacterDataDictionary[1].Value.position-gs.FrisbiData.position).normalized;
            }
            dir.y = 0;
            frisbie.Throw(gs.FrisbiData,dir,10.0f);
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

        public void WinResetRound(GameState gs)
        {
            frisbie.Reset(gs.FrisbiData);
            ResetPlayerPosition(gs); 
            if(gs.GameManagerData.scoreLeft > gs.GameManagerData.scoreRight)
            {
                gs.GameManagerData.SetLeftCount++;
            }
            else if(gs.GameManagerData.scoreRight > gs.GameManagerData.scoreLeft)
            {
                gs.GameManagerData.SetRightCount++;
            }
            gs.GameManagerData.scoreRight = 0;
            gs.GameManagerData.scoreLeft = 0;
            if(gs.GameManagerData.isCurrentGame)
            {
                MajUIScore(gs.GameManagerData);
                textRoundScoreLeft.text = gs.GameManagerData.SetLeftCount.ToString("00");
                textRoundScoreRight.text = gs.GameManagerData.SetRightCount.ToString("00");
                roundSetObject.SetActive(true);
            }
            Faction f;
            if(isWinBO3(gs.GameManagerData,out f))
            {
                gs.GameManagerData.timeNextSet = 4.0f;
                gs.GameManagerData.endGame = true;
                if(gs.GameManagerData.isCurrentGame)
                {
                    winObj.SetActive(true);
                }
            }
            else
            {
                gs.GameManagerData.timeNextSet = 2.0f;
                gs.GameManagerData.endSet = true;
            }
        }

        public bool isWinBO3(GameManagerData gameManagerData, out Faction faction)
        {
            if(gameManagerData.SetLeftCount == 3 || (gameManagerData.SetLeftCount == 2 && gameManagerData.SetRightCount == 0))
            {
                faction = Faction.Left;
                return true;
            }
            else if(gameManagerData.SetRightCount == 3 || (gameManagerData.SetRightCount == 2 && gameManagerData.SetLeftCount == 0))
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

        public void Simulate(ref GameState gameState,float dt)
        {
            if(gameState.GameManagerData.endGame && gameState.GameManagerData.timeNextSet < 0.0f) {return;}
            gameState.GameManagerData.gameTime -= dt;
            gameState.GameManagerData.timeNextSet -= dt;
            if(!gameState.GameManagerData.endSet && gameState.GameManagerData.gameTime <= 0.0f || gameState.GameManagerData.scoreLeft >= maxScoreToSet || gameState.GameManagerData.scoreRight >= maxScoreToSet)
            {
                gameState.GameManagerData.endSet = true;
                if(gameState.GameManagerData.isCurrentGame)
                {
                    timeText.text = "00";
                }
                WinResetRound(gameState);
            }
            if(gameState.GameManagerData.endSet && gameState.GameManagerData.timeNextSet <= 0.0f)
            {
                Faction f;
                if(isWinBO3(gameState.GameManagerData,out f))
                {
                    if(gameState.GameManagerData.isCurrentGame)
                    {
                        BackMenu();
                    }
                }
                else
                {
                    if(gameState.GameManagerData.isCurrentGame)
                    {
                        roundSetObject.SetActive(false);
                    }
                    gameState.GameManagerData.gameTime = RoundMaxTime;
                    gameState.GameManagerData.endSet = false;
                    StartThrow(gameState,f == Faction.Left ? Faction.Right : Faction.Left);
                }
                CharacterCanMove(true);
            }
        }

        void Update()
        {
            Simulate(ref gameState,Time.deltaTime);
            if(!gameState.GameManagerData.endSet && lastgameTime != (int)gameState.GameManagerData.gameTime)
            {
                lastgameTime = (int)gameState.GameManagerData.gameTime;
                timeText.text = lastgameTime.ToString("00");
            }
        }
    }
}