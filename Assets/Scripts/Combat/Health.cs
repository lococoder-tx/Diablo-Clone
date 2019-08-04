using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
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
            
            //dESTROY//deactivate STUFF
            if (!removed)
            {
                Destroy(gameObject.GetComponent<NavMeshAgent>());
                removed = true;
            }


            Destroy(this.gameObject, 50f);

        }

        public bool IsDead()
        {
            return isDead;
        }

        
    }
}