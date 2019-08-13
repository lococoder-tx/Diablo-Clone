using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class Health: MonoBehaviour
    {
        public float maxHealth;
        public float currentHealth { get; set; }
        private bool isDead = false;
        private bool removed = false;
       
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

        
    }
}