using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;
    
    //cached
    private Animator animator;
    private NavMeshAgent thisNavAgent;
    
    
    private Ray lastRay;
    
    // Start is called before the first frame update
    void Start()
    {
        thisNavAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
        
        //left mouse button clicked
        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }
        
        //Debug.DrawRay(lastRay.origin, lastRay.direction * 100)
    }

    private void MoveToCursor()
    {
        //draw ray from camera position to wherever cursor is pointing
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        if (hasHit)
        {
            thisNavAgent.destination = hit.point;
        }

    }

    private void UpdateAnimator()
    {
        //converts velocity  of nav mesh agent from world space to local space
        Vector3 localVelocity = transform.InverseTransformDirection(thisNavAgent.velocity);
       
        ///update animator value
        animator.SetFloat("forwardSpeed", localVelocity.z);
    }
}
