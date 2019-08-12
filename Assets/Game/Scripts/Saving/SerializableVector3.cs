using UnityEngine;

namespace RPG.Saving
{
    
    [System.Serializable]
    public class SerializableVector3
    {
        public float x { get; }
        public float y { get; }
        public float z { get; }

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x,y,z);
        }
    }

}