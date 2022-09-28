using UnityEngine;
namespace WJ
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager instance = null;
        #region  InputAction
        [SerializeField] private MCTS_Windjammers playerInput = null;

        #endregion
        #region Get
        public static MCTS_Windjammers InputJoueur
        {
            get
            {
                return instance.playerInput;
            }
        }
        #endregion

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            playerInput = new MCTS_Windjammers();
            playerInput.Enable();
        }

        private void OnEnable()
        {
            playerInput.Enable();
        }
        private void OnDisable() { }
    }
}