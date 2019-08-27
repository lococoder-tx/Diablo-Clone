using System.Collections;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class Health: MonoBehaviour, ISaveable
    {
        public float maxHealth;
        public float currentHealth;
        private bool isDead = false;
        private bool removed = false;
        
        
        //Note: the way that this is currently written, the value of health when you load a new scene gets overriden by this method
        //because of script execution order
        void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0);
            print("Health of " + currentHealth);
            if (currentHealth == 0)
            {
               Die();
            }
        }

        private void Die()
        {
            //todo: stop enemy from attacking, change enemy to empty model?
            
            GetComponent<Animator>().SetTrigger("dead");
            isDead = true;
            GetComponent<ActionScheduler>().CancelAction();
            
            
            //dESTROY//deactivate STUFF
            if (!removed)
            {
                
                GetComponent<NavMeshAgent>().enabled = false;
                removed = true;
            }


            Destroy(this.gameObject, 5f);

        }

        public bool IsDead()
        {
            return isDead;
        }


        public object CaptureState()
        {
            return currentHealth;
        }

       
        //Note: the way that this is currently written, the value of health when you load a new scene gets overriden by this method
        //because of script execution order. change start to awake to to fix issue
        public void RestoreState(object state)
        {
            currentHealth = (float) state;
            if(currentHealth <= 0)
                Die();
        }
    }
}