using UnityEngine;
using RPG.Movement;
using RPG.Combat;


namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        //cached references
        private Fighter fighter;
        private Mover mover;

        private int enemyLayer = 9;
        
        // Start is called before the first frame update
        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent <Fighter>();
        }
    
        // Update is called once per frame
        void Update()
        {
            if(InteractWithCombat())
                return;
            
            if (InteractWithMovement())
                return;
            
            print("all methods in playerController return false...");
        }

        private bool InteractWithCombat()
        {
            //apply bitwise operation to get only enemy layer
            int layerMask = 1 << enemyLayer;
           
            var ray = GetMouseRay();
            
            RaycastHit [] hits;
            hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

            
            //since we are only hitting enemy layer, we can assume we are hitting enemy here
            foreach (var enemyHit in hits)
            {
                CombatTarget target = enemyHit.transform.gameObject.GetComponent<CombatTarget>();
                if (!fighter.CanAttack(target))
                    continue;
               
                //validEnemy is true
                if (Input.GetMouseButtonDown(0))
                    fighter.Attack(target);
                
                return true;
            }

            return false;
        }
        
        
        private bool InteractWithMovement()
        {
            //draw ray from camera position to wherever cursor is pointing
            Ray ray = GetMouseRay();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);

            if (!hasHit)
                return false;
            
            //if player clicks to move, then move to position and detarget current enemy in fighter
            if (Input.GetMouseButton(0))
            {
                mover.StartMoveAction(hit.point);
            }

            return true;
        }

        private static Ray GetMouseRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray;
        }
    }

}
