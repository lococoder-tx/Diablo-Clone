using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RPG.Saving
{
    
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            //load previous serialized dictionary
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile,state);
        }

        private void SaveFile(string saveFile, object state)
        {
            string pathFromSaveFile = GetPathFromSaveFile(saveFile);
            //if stream is valid
            using (FileStream stream = File.Open(pathFromSaveFile, FileMode.Create))
            {

                BinaryFormatter formatter = new BinaryFormatter();

                //object graph is whatever you want to serialize 
                formatter.Serialize(stream, state);
            }
        }


        public void Load(string saveFile)
        {
            /*
            string pathFromSaveFile = GetPathFromSaveFile(saveFile);
            print("loading " + pathFromSaveFile);

            using (FileStream stream = File.Open(pathFromSaveFile, FileMode.Open))
            { 
                //deserialize object using binaryFormatter (should return vector3)
                BinaryFormatter formatter = new BinaryFormatter();
                
                //we are deserializing a dictionary
                RestoreState(formatter.Deserialize(stream));
                /*
                //reading through the stream byte by byte 
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                Vector3 posFromSaveFile = DeserializeVector3(buffer);
            */
            RestoreState(LoadFile(saveFile));



        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string pathFromSaveFile = GetPathFromSaveFile(saveFile);
            if (!File.Exists(pathFromSaveFile))
                return new Dictionary<string, object>();                
            
            using (FileStream stream = File.Open(pathFromSaveFile, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>) formatter.Deserialize(stream);
            }
            
        }


        //add items to a serializable dictionary and captures their curretn states
        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                //in this dictionary, each identifier is associated with the object's state at savetime
                string uniqueIdentifier = saveable.GetUniqueIdentifier();
                if(!state.ContainsKey(uniqueIdentifier))
                    state.Add(uniqueIdentifier, saveable.CaptureState());
                else
                    state[uniqueIdentifier] = saveable.CaptureState();
            }
           
        }
        
   
        private void RestoreState( Dictionary<string, object> state)
        {
            if (state == null) return;
            
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                //looks at all saveable entities in scene, and then restores their state based on their old stats
                //from dictionary entry
                string uniqueIdentifier = saveable.GetUniqueIdentifier();
                if(state.ContainsKey(uniqueIdentifier))
                    saveable.RestoreState(state[uniqueIdentifier]);
            }
        }
        

       
        //if using stream.write and writing to file manually (float is 4 bytes)
        private byte[] SerializeVector3(Vector3 vector)
        {
            byte [] vectorBytes = new byte[12];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes,0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);
            return vectorBytes;
        }
        
        //if using stream.read and reading from file manually (float = 4 bytes)
        private Vector3 DeserializeVector3(byte[] bytes)
        {
           float x =  BitConverter.ToSingle(bytes, 0);
           float y = BitConverter.ToSingle(bytes, 4);
           float z = BitConverter.ToSingle(bytes, 8); 
           return new Vector3(x,y,z);
        }

        
        //returns path of save file.sav - file paht is in appdata/locallow/defaultcompany
        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }

    
        
    }
}