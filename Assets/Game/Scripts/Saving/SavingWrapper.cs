using UnityEngine;



namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFile = "save";
        private SavingSystem savingSystem;
        
        private void Start()
        {
            savingSystem = GetComponent<SavingSystem>();
            
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
        
    }
}