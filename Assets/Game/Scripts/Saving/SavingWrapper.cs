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
                savingSystem.Save(defaultSaveFile);

            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
               
                savingSystem.Load(defaultSaveFile);
                
            }
            
        }
    }
}