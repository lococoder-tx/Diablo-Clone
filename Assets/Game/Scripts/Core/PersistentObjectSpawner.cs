using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{

    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistentObjectPrefab;

        public static bool hasSpawned = false;
        
        private void Awake()
        {
            if (hasSpawned)
                return;
            
            hasSpawned = true;
            SpawnPersistentObjects();
            
            
        }

        private void SpawnPersistentObjects()
        {
          
            GameObject persistentObject = Instantiate(persistentObjectPrefab); 
            DontDestroyOnLoad(persistentObject);
            
        }
    }
}
