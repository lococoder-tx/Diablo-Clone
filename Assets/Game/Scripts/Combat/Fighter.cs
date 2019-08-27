using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
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
        [Header("Fighter Stats")]
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float weaponDamage;
        
        [Header("Weapon")]
        [SerializeField] private Transform rightHandPosition = null;
        [SerializeField] private GameObject equippedWeapon = null;
        [SerializeField] private GameObject testWeaponToSpawn;
        
        [Header("")]
        //public float nextAttack = 0;
        public float timer = 20;

        //references 
        [SerializeField] Health target; //serialized for debug
        
        //cached
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator anim;
        void Awake()
        {
             mover = GetComponent<Mover>();
             actionScheduler = GetComponent<ActionScheduler>();
             anim = GetComponent<Animator>();
        }

        private void Start()
        {
            SpawnWeapon(testWeaponToSpawn);
        }

        void Update()
        {
            
            
            timer += Time.deltaTime;
            
            if (!target)
                return;
            
            
            
            if (!InRange())
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

        private bool InRange()
        {
            var distance =  Mathf.Abs(Vector3.Distance(transform.position, target.transform.position));
            return distance < weaponRange;
        }
        

        private void AttackBehavior()
        {
            if (target.IsDead())
            {
                Cancel();
                actionScheduler.CancelAction();
                
            }

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
       
        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject target)
        {
            return target && !target.GetComponent<Health>().IsDead();
        }

        public void SpawnWeapon(GameObject weaponToEquip)
        {
            equippedWeapon = Instantiate(weaponToEquip, rightHandPosition);
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
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, weaponRange);
        }
    }
}