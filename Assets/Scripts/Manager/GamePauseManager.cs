public class GamePauseManager 
{
    public static GamePauseManager _instance;

    public static GamePauseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GamePauseManager();
            }
            return _instance;
        }
    }

    public GamePause CurrentGamePause { get; private set; }

    

    public delegate void GamePauseChangeHandler(GamePause newGamePause);
    public event GamePauseChangeHandler OnGamePauseChanged;

    private GamePauseManager(){}

    public void SetPause(GamePause newGamePause)
    {
        if(newGamePause == CurrentGamePause)
        {
            return;
        }
        CurrentGamePause = newGamePause;
        OnGamePauseChanged?.Invoke(newGamePause);
    }

}
