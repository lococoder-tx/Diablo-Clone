using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{
    
    //scripts uses:
    //actionscheduler.cs
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        //[SerializeField] Transform target;

        //cached
        private Animator animator;
        private NavMeshAgent thisNavAgent;
        //private Ray lastRay;
        private ActionScheduler actionScheduler;

        // Start is called before the first frame update
        void Start()
        {
            if(!thisNavAgent)
                thisNavAgent = GetComponent<NavMeshAgent>();
            if(!actionScheduler)
                actionScheduler = GetComponent<ActionScheduler>();
            
            animator = GetComponent<Animator>();
            
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();

            //Debug.DrawRay(lastRay.origin, lastRay.direction * 100)
        }

        public void StartMoveAction(Vector3 pos)
        {
            actionScheduler.StartAction(this);
            MoveTo(pos);
        }
        
        public void MoveTo(Vector3 pos)
        {
            thisNavAgent.destination = pos;
            thisNavAgent.isStopped = false;
        }
        
        private void UpdateAnimator()
        {
            //converts velocity  of nav mesh agent from world space to local space
            if (!thisNavAgent) return;
                
            Vector3 localVelocity = transform.InverseTransformDirection(thisNavAgent.velocity);

            ///update animator value
            animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void Cancel()
        {
            thisNavAgent.isStopped = true;
            
        }

        public bool IsAtLocation(float tolerance)
        {
            
            return Vector3.Distance(thisNavAgent.destination , transform.position) < tolerance;
        }

        
        //isaveable override
        public object CaptureState()
        {
            print("saving location state for " + gameObject);
            
            //if(gameObject == MainPlayer.Instance.gameObject)
                //print(transform.position);
            
            return new SerializableVector3(transform.position);
        }

        
        public void RestoreState(object state)
        {
            print("loading previous state for " + gameObject);
            
            //ex: state corresponds to the value that is associated with a unique identifier key
            SerializableVector3 oldPos = (SerializableVector3) state;
            
            //NOTE: due to function timing, start will be called after wrapper.load (From portal) so necessary to
            //retrieve references to preent null pointer
            actionScheduler = GetComponent<ActionScheduler>();
            thisNavAgent = GetComponent<NavMeshAgent>();
            
            //need to disable the navAgent before adjusting the position
            thisNavAgent.enabled = false;
            
            transform.position = oldPos.ToVector3();
            //Warning: this throws warning if player is not near navmesh...
            thisNavAgent.enabled = true;
            
        }
    }
}