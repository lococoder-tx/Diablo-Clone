using RPG.Combat;
using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Controller
{

    
    public class AIController : MonoBehaviour
    {
        //ai floats
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTimer = 10f;
        [SerializeField] private PatrolPath patrolPath;
        private int currentWayPointIndex = 0;
        [SerializeField] private float tolerance = 1f;
        
        //ai dwell times
        [SerializeField] private float minDwellTime;
        [SerializeField] private float maxDwellTime;

        [SerializeField] private float currentDwellTime; //serialized for debug purposes
        //ai varaibles
        private Vector3 lastKnownLocation;
        private Vector3 guardLocation;
        private Quaternion guardRotation;
        [SerializeField] private float timeSinceLastSawPlayer = Mathf.Infinity;
        
        
        
        //cached
        private Fighter fighter;
        private Mover mover;
        private Health health;
        
        void Awake()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();

            //store initial guard transform state (also used if no patrol is assigned, in which case this is the 
            //default locaion the guard is placed
            guardLocation = transform.position;
            guardRotation = transform.rotation;
            

            //set this guards starting location to first pos on waypoint path
            //if (patrolPath && patrolPath.transform.childCount > 0)
            //{
            //    transform.position = patrolPath.transform.GetChild(currentWayPointIndex).position;
            //}
        }
        
        void Update()
        {
            if (health.IsDead())
                return;
            
            InteractWithCombat();

            timeSinceLastSawPlayer += Time.deltaTime;

        }

        private float DistanceToPlayer()
        {
            return Mathf.Abs(Vector3.Distance(MainPlayer.Instance.transform.position, transform.position));
        }

        private void InteractWithCombat()
        {
            //IF ENEMY WITHIN DISTANCE AND PLAYER IS STILL ALIVE 
            if (DistanceToPlayer() <= chaseDistance && fighter.CanAttack(MainPlayer.Instance.gameObject) || IsAttacked())
            {
                //agent currently sees player
                AttackBehavior();
            }
            else if (suspicionTimer > timeSinceLastSawPlayer)
            {
                //initiate suspicion phase
                SuspicionBehavior();
                
            }
            else
            {
                PatrolBehavior();
            }

        }

        private bool IsAttacked()
        {
            var player = MainPlayer.Instance;
            bool isAttacked = player.GetComponent<Fighter>().target == this.gameObject.GetComponent<Health>();
            return (isAttacked);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        private void PatrolBehavior()
        {
            Vector3 nextPos = guardLocation;
            
            if(patrolPath)
            {
                
                if (AtWayPoint())
                {
                    if (currentDwellTime > 0)
                        currentDwellTime -= Time.deltaTime;
                    
                    else
                        GoToNextWayPoint();
                }

                nextPos = GetCurrentWayPoint();

            }
            
            mover.StartMoveAction(nextPos);
            
            //set rotation for guarding enemy to whatever it was before
            if (!patrolPath && mover.IsAtLocation(tolerance))
            {
                transform.rotation = guardRotation;
            }

        }

        private void GoToNextWayPoint()
        {
            if (currentWayPointIndex < patrolPath.transform.childCount)
            {
                currentWayPointIndex++;
            }

            if (currentWayPointIndex == patrolPath.transform.childCount)
            {
                currentWayPointIndex = 0;
            }

            currentDwellTime = Random.Range(minDwellTime, maxDwellTime);
        }

        private bool AtWayPoint()
        {
            return Vector3.Distance(transform.position, patrolPath.transform.GetChild(currentWayPointIndex).position) <
                   tolerance;
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.transform.GetChild(currentWayPointIndex).position;
        }

        private void SuspicionBehavior()
        {
            mover.StartMoveAction(lastKnownLocation);
        }
        
        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(MainPlayer.Instance.gameObject);
            lastKnownLocation = MainPlayer.Instance.transform.position;
        }
        
    }


}