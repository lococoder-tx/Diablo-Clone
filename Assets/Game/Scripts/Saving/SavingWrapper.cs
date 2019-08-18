using System.Collections;
using RPG.SceneManagement;
using UnityEngine;



namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFile = "defaultSave";
        private SavingSystem savingSystem;
        
        private IEnumerator Start()
        {
            savingSystem = GetComponent<SavingSystem>();
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOutImmediately();
            yield return savingSystem.LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn();

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
               Save();

            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }

        public void LoadLastScene()
        {
            StartCoroutine(savingSystem.LoadLastScene(defaultSaveFile));
        }


       
    }
}