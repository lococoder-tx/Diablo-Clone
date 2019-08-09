using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Core
{
    public class SceneLoader : MonoBehaviour
    {
        //script not used - revise and integrate anim calls with Portal.cs
        
        [SerializeField] private Animator animator;

        [SerializeField] private int levelToLoad = -1; //serialized for debugging purposes

        private static SceneLoader _instance;
        
        //public static method that can be used to retrieve current Instance of main player 
        public static SceneLoader Instance
        {
            get { return _instance;  }
        }
        
        //singleton pattern
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            
        }
        
        
        public void LoadScene(int levelToLoad, float timeToWait = 2f)
        {
            this.levelToLoad = levelToLoad;
            animator.SetBool("Fade", true);

        }

        public void OnFadeComplete()
        {
            animator.SetBool("Fade", false);
            SceneManager.LoadScene(levelToLoad);
        }
    }
}