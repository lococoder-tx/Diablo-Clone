using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Controller
{


    public class PatrolPath : MonoBehaviour
    {
        
        //Unity function to keep track of waypaths
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int i = 0; i <= transform.childCount-1; i ++)
            {
                Transform firstWaypoint = transform.GetChild(i);
                Transform secondWayPoint = null;

                if (i + 1 >= transform.childCount)
                {
                    Gizmos.DrawLine(firstWaypoint.position, transform.GetChild(0).position);
                    break;
                }

                secondWayPoint = transform.GetChild(i + 1);
                Gizmos.DrawLine(firstWaypoint.position, secondWayPoint.position);
                
            }
        }
    }
}