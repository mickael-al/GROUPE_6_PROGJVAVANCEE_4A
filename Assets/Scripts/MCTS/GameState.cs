namespace WJ_MCTS
{
    public class GameState 
    {
        private GameManagerData gameManagerData = new GameManagerData();
        private FrisbiData frisbiData = new FrisbiData();
        private CharacterData[] characterData = new CharacterData[2];

        #region Getter
        public  CharacterData[] characterDatas { get {return characterData;} }
        public GameManagerData GameManagerData { get {return gameManagerData;} }
        public FrisbiData FrisbiData { get {return frisbiData;} }
        #endregion

        public GameState copy()
        {
            GameState gs = new GameState();
            gs.characterData[0] = new CharacterData();
            gs.characterData[1] = new CharacterData();
            
            gs.gameManagerData.endGame = gameManagerData.endGame;
            gs.gameManagerData.endSet = gameManagerData.endSet;
            gs.gameManagerData.gameTime = gameManagerData.gameTime;
            gs.gameManagerData.scoreRight = gameManagerData.scoreRight;
            gs.gameManagerData.scoreLeft = gameManagerData.scoreRight;
            gs.gameManagerData.SetLeftCount = gameManagerData.SetLeftCount;
            gs.gameManagerData.SetRightCount = gameManagerData.SetRightCount;
            gs.gameManagerData.timeNextSet = gameManagerData.timeNextSet;
            gs.gameManagerData.round = gameManagerData.round;
            gs.gameManagerData.isCurrentGame = false;

            gs.frisbiData.position = frisbiData.position;
            gs.frisbiData.currentDirection = frisbiData.currentDirection;
            gs.frisbiData.throwMode = frisbiData.throwMode;
            gs.frisbiData.move = frisbiData.move;

            gs.characterData[0].position = characterData[0].position;
            gs.characterData[0].currentDirection = characterData[0].currentDirection;
            gs.characterData[0].turn = characterData[0].turn;
            gs.characterData[0].canMove = characterData[0].canMove;
            gs.characterData[0].handObject = characterData[0].handObject;

            gs.characterData[1].position = characterData[1].position;
            gs.characterData[1].currentDirection = characterData[1].currentDirection;
            gs.characterData[1].turn = characterData[1].turn;
            gs.characterData[1].canMove = characterData[1].canMove;
            gs.characterData[1].handObject = characterData[1].handObject;
            
            return gs;
        }
    }
}
