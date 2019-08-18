using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";
        private static Dictionary<string, SaveableEntity> globalSaveableEntities = new Dictionary<string, SaveableEntity>();
        
        
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        //returns dictionary of states of components
        public object CaptureState()
        {
            
            //declare new state dictionary to keep track of all isaveable components for this object
            Dictionary<string, object> state = new Dictionary<string, object>();
            
            foreach (ISaveable saveable in  GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            
            
            
            return state;
        }
        
        public void RestoreState(object state)
        {
            
            Dictionary<string, object> stateDict =  (Dictionary<string, object>) state;
            
            //for each saveable component in dic, restore its state to what was saved using its key
            foreach (ISaveable saveable in  GetComponents<ISaveable>())
            {
                string key = saveable.GetType().ToString();
                if(stateDict.ContainsKey(key)) 
                    saveable.RestoreState(stateDict[key]);
            }
            
            /*
            print("loading previous state for " + gameObject);
            
            //ex: state corresponds to the value that is associated with a unique identifier key
            SerializableVector3 vecState = (SerializableVector3) state;
            GetComponent<ActionScheduler>().CancelAction();
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = true;
            
            transform.position = vecState.ToVector3();
            */
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject) || string.IsNullOrEmpty(gameObject.scene.path)) 
                return;
            
            
            
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");


            
            //only modify property if string value is "", this section generates new uuid
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                
                //necessary to apply changes to values
                serializedObject.ApplyModifiedProperties();
            }

            globalSaveableEntities[property.stringValue] = this;
        }
#endif
        private bool IsUnique(string stringValue)
        {

            //if the entity is not currently in dictionary, or the key corresponds only to this element, return true
            if (!globalSaveableEntities.ContainsKey(stringValue) || globalSaveableEntities[stringValue] == this)
                return true;
            
            //if scene is deloaded, entity is set to null...so remove it from the dictionary
            if (globalSaveableEntities[stringValue] == null)
            {
                globalSaveableEntities.Remove(stringValue);
                return true;
            }

            if (globalSaveableEntities[stringValue].GetUniqueIdentifier() != stringValue)
            {
                globalSaveableEntities.Remove(stringValue);
                return true;
            }

            return false;

        }
     
    }
    
    
}