using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;
    private NavMeshAgent thisNavAgent;

    private Ray lastRay;
    
    // Start is called before the first frame update
    void Start()
    {
        thisNavAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //left mouse button clicked
        if (Input.GetMouseButtonDown(0))
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
}
