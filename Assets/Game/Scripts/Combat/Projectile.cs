using JetBrains.Annotations;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float thisProjectileDamage = 0f;
        [SerializeField] private float projectileDestroySpeed = 5f;
        [SerializeField] private bool homing = false;

        private float damage;
        private bool reachedDestination = false;
        
        // Update is called once per frame
        void Start()
        {
            transform.LookAt(GetTargetPosition());
        }
        
        void Update()
        {
            bool isDead = target.GetComponent<Health>().IsDead();
            if (!reachedDestination || isDead)
            {
                if (homing)
                    transform.LookAt(GetTargetPosition());
                
                transform.Translate(Vector3.forward * speed * Time.deltaTime); 
               
                //handle proj that spawn after target is dead
                if (isDead)
                {
                    Destroy(gameObject, 5f);
                    homing = false;
                }
            }
        }

        private Vector3 GetTargetPosition()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            return (target.position + Vector3.up * targetCollider.height / 2);
        }

        public void SetTarget(Transform target, float damage)
        {
            this.target = target;
            this.damage = damage + thisProjectileDamage;
        }

        private void OnTriggerEnter(Collider other)
        {
            
            //only deal damage if enemy alive and = to target
            if (other.gameObject == target.gameObject && !target.GetComponent<Health>().IsDead())
            {
                other.GetComponent<Health>().TakeDamage(damage);
                transform.parent = other.transform;
                
                //handle last arrow that hits and kills enemy
                if (other.GetComponent<Health>().IsDead())
                {
                    Destroy(gameObject);
                    return;
                }

                Destroy(gameObject, projectileDestroySpeed);
                GetComponentInChildren<TrailRenderer>().enabled = false;
                reachedDestination = true;
            }
        }
    }
}
