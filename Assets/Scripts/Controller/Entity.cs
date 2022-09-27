using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ
{
    public abstract class Entity : MonoBehaviour
    {
        protected virtual void Awake()
        {
            GamePauseManager.Instance.OnGamePauseChanged += OnGamePauseChanged;
        }

        protected virtual void OnDestroy() 
        {
            GamePauseManager.Instance.OnGamePauseChanged -= OnGamePauseChanged;
        }

        public virtual void OnGamePauseChanged(GamePause newGamePause)
        {
            enabled = newGamePause == GamePause.GamePlay;
        }
    }
}
