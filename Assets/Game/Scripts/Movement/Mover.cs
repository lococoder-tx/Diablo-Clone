using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    
    //scripts uses:
    //actionscheduler.cs
    public class Mover : MonoBehaviour, IAction
    {
        //[SerializeField] Transform target;

        //cached
        private Animator animator;
        private NavMeshAgent thisNavAgent;
        private Ray lastRay;
        private ActionScheduler actionScheduler;

        // Start is called before the first frame update
        void Start()
        {
            thisNavAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
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
        
    }
}