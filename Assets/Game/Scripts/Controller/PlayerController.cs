using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;


namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        //cached references
        private Fighter fighter;
        private Mover mover;
        private Health health;

        private int enemyLayer = 9;
        
        // Start is called before the first frame update
        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent <Fighter>();
            health = GetComponent<Health>();
        }
    
        // Update is called once per frame
        void Update()
        {
            //print(GetComponent<ActionScheduler>().currentAction);

            if (health.IsDead())
                return;

            if (InteractWithCombat())
                return;

            InteractWithKeyboard();

            if (InteractWithMovement())
                return;
            //print("all methods in playerController return false...");
        }

        private void InteractWithKeyboard()
        {
            //unequip button
            if (Input.GetKeyDown(KeyCode.D))
            {
                fighter.UnequipWeapon();
            }
               
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
                
                //if target has combatTarget(is targetable by player) or player cannot attack the selected npc
                if (!target || !fighter.CanAttack(target.gameObject))
                    continue;
               
                //validEnemy is true
                if (Input.GetMouseButtonDown(0))
                    fighter.Attack(target.gameObject);
                
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
