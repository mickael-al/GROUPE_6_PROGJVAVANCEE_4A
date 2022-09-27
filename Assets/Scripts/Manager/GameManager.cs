using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace WJ
{
    public class GameManager : Entity
    {
        [SerializeField] private GameObject prefabCharcterObject = null;
        [SerializeField] private GameObject friesbeeObject = null;
        [SerializeField] private List<GameObject> mapObject = new List<GameObject>();
        [SerializeField] private float RoundMaxTime = 90.0f;
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
        [SerializeField] private TextMeshProUGUI textRoundScoreLeft = null;
        [SerializeField] private TextMeshProUGUI textRoundScoreRight = null;

        private static int indiceMap = 0; 
        private static int indicePlayerLeft = 0;
        private static int indicePlayerRight = 0;
        private static CharacterMode characterModeRight = CharacterMode.Player; 

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
            Instantiate(mapObject[indiceMap],Vector3.zero,Quaternion.identity);
            characterLeft = Instantiate(prefabCharcterObject,characterPosition[0],Quaternion.identity);
            characterRight = Instantiate(prefabCharcterObject,characterPosition[1],Quaternion.identity);
            frisbie = Instantiate(friesbeeObject,Vector3.zero,Quaternion.identity).GetComponent<FrisbieController>();
            frisbie.Reset();
            StartThrow(Random.Range(0,2) == 0 ? Faction.Left : Faction.Right);
            SeeCursor(false);
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
            ResetPlayerPosition();   
            if(gameTime > 1.5f)
            {
                yield return new WaitForSeconds(1.5f);
                StartThrow(f);
            }
        }

        public void ResetPlayerPosition()
        {
            characterLeft.transform.position = characterPosition[0];
            characterLeft.transform.position = characterPosition[1];
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

        public IEnumerator WinResetRound()
        {
            frisbie.Reset();
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
            yield return new WaitForSeconds(2.0f);
            roundSetObject.SetActive(false);
            gameTime = RoundMaxTime;
            endGame = false;
            StartThrow(Random.Range(0,2) == 0 ? Faction.Left : Faction.Right);
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