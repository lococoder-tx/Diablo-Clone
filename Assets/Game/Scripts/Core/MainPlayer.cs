using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{

    public class MainPlayer : MonoBehaviour
    {
        private static MainPlayer _instance;
        
       //public static method that can be used to retrieve current Instance of main player 
        public static MainPlayer Instance
        {
            get { return _instance;  }
        }
        
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
    }
}