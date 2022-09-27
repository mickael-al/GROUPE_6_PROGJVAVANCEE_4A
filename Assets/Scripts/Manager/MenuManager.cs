using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WJ
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        private float boutonTime = 0.0f;

        public bool NotClick()
        {
            if(Time.time-boutonTime > 0.3f)
            {
                boutonTime = Time.time;
                return false;
            }
            return true;
        }

        public void Option()
        {
            if(NotClick())
            {   
                return;
            }
            animator.SetTrigger("Option");
        }

        public void Retour()
        {
            if(NotClick())
            {   
                return;
            }
            animator.SetTrigger("Retour");
        }

        public void Personnage()
        {
            if(NotClick())
            {   
                return;
            }
            animator.SetTrigger("Personnage");
        }

        public void Carte()
        {
            if(NotClick())
            {   
                return;
            }
            animator.SetTrigger("Carte");
        }

        public void LoadCarte()
        {
            if(NotClick())
            {   
                return;
            }
            FadeScreenManager.FadeIn();
            FadeScreenManager.OnFadeInComplete += LoadScene;
        }

        private void LoadScene()
        {
            SceneManager.LoadScene("Game",LoadSceneMode.Single);
        }

        public void Quitter()
        {
            Application.Quit();
        }
    }
}