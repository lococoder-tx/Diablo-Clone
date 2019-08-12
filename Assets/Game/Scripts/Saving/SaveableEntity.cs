using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";
        
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            
            print("saving state for " + gameObject);
            return new SerializableVector3(transform.position);
        }
        
        public void RestoreState(object state)
        {
           print("loading previous state for " + gameObject);
            
            //ex: state corresponds to the value that is associated with a unique identifier key
            SerializableVector3 vecState = (SerializableVector3) state;
            CancelAction();
            
            transform.position = vecState.ToVector3();
        }

        private void Update()
        {
            if (Application.IsPlaying(gameObject) || string.IsNullOrEmpty(gameObject.scene.path)) 
                return;
            
            
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            
            //only modify property if string value is ""
            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                
                //necessary to apply changes to values
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void CancelAction()
        {
            GetComponent<ActionScheduler>().CancelAction();
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = true;

        }
    }
}