using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

//uses:
//actionScheduler
//mover
//health
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        //these properties relate to weapons and need to be moved
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float weaponDamage;
        
        //public float nextAttack = 0;
        public float timer = 0;

        //references 
        [SerializeField] Health target; //serialized for debug
        
        //cached
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator anim;
        void Start()
        {
             mover = GetComponent<Mover>();
             actionScheduler = GetComponent<ActionScheduler>();
             anim = GetComponent<Animator>();
        }
        
        void Update()
        {
            timer += Time.deltaTime;
            
            if (!target)
                return;
            
            var distance =  Mathf.Abs(Vector3.Distance(transform.position, target.transform.position));
            
            if (distance > weaponRange)
            {
                    //player has not reached enemy
                    mover.MoveTo(target.transform.position);
            }
            else //witihn range of target, safe to attack
            {
                    
                    transform.LookAt(target.transform);
                    mover.Cancel();
                    //do attacking stuff here
                    AttackBehavior();

            }
            
        }

        private void AttackBehavior()
        {
            if (target.IsDead())
                Cancel();
            
            else if (timer > timeBetweenAttacks)
            {
                anim.ResetTrigger("stopAttack");
                anim.SetTrigger("attack");
                timer = 0;

            }
        }

        public void Cancel()
        {
            target = null;
            anim.SetTrigger("stopAttack");
           
        }
       
        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(CombatTarget target)
        {
            return target && !target.GetComponent<Health>().IsDead();
        }

        public bool HasTarget()
        {
            return target;
        }

        //animation event
        void Hit()
        {
            if (!target) return;
            target.TakeDamage(weaponDamage);
        }
    }
}