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

            gs.frisbiData.position.x = frisbiData.position.x;
            gs.frisbiData.position.y = frisbiData.position.y;
            gs.frisbiData.position.z = frisbiData.position.z;
            gs.frisbiData.currentDirection.x = frisbiData.currentDirection.x;
            gs.frisbiData.currentDirection.y = frisbiData.currentDirection.y;
            gs.frisbiData.currentDirection.z = frisbiData.currentDirection.z;
            gs.frisbiData.throwMode = frisbiData.throwMode;
            gs.frisbiData.move = frisbiData.move;

            gs.characterData[0].position.x = characterData[0].position.x;
            gs.characterData[0].position.y = characterData[0].position.y;
            gs.characterData[0].position.z = characterData[0].position.z;
            gs.characterData[0].currentDirection.x = characterData[0].currentDirection.x;
            gs.characterData[0].currentDirection.y = characterData[0].currentDirection.y;
            gs.characterData[0].currentDirection.z = characterData[0].currentDirection.z;
            gs.characterData[0].turn = characterData[0].turn;
            gs.characterData[0].canMove = characterData[0].canMove;
            gs.characterData[0].handObject = characterData[0].handObject;

            gs.characterData[1].position.x = characterData[1].position.x;
            gs.characterData[1].position.y = characterData[1].position.y;
            gs.characterData[1].position.z = characterData[1].position.z;
            gs.characterData[1].currentDirection.x = characterData[1].currentDirection.x;
            gs.characterData[1].currentDirection.y = characterData[1].currentDirection.y;
            gs.characterData[1].currentDirection.z = characterData[1].currentDirection.z;
            gs.characterData[1].turn = characterData[1].turn;
            gs.characterData[1].canMove = characterData[1].canMove;
            gs.characterData[1].handObject = characterData[1].handObject;
            
            return gs;
        }
    }
}
