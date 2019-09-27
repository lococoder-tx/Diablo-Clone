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
        [SerializeField] private float projectileDecaySpeed = 5f;
        [SerializeField] private bool homing = false;
        
        [Header("FX")]
        [SerializeField] private GameObject hitEffect = null;

        [Header("Arrow Effect Settings")] 
        [SerializeField] private float minHitVariance = 0f;
        [SerializeField] private float maxHitVariance = 1f;
        [SerializeField] private GameObject[] destroyOnHit;

        private float damage;
        private bool reachedCollider = false;
        
        // Update is called once per frame
        void Start()
        {
            transform.LookAt(GetTargetPosition());
        }
        
        void Update()
        {
            bool isDead = target.GetComponent<Health>().IsDead();
            
            if (!reachedCollider || isDead)
            {
                if (homing)
                    transform.LookAt(GetTargetPosition());
                
                transform.Translate(Vector3.forward * speed * Time.deltaTime); 
               
                //handle proj that spawn after target is dead
                if (isDead)
                {
                    Destroy(gameObject, projectileDecaySpeed);
                    homing = false;
                }
            }
        }

        private Vector3 GetTargetPosition()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            return (target.position + Vector3.up * (targetCollider.height + Random.Range(minHitVariance, maxHitVariance) ) / 2);
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
                //do damage
                other.GetComponent<Health>().TakeDamage(damage);
                
                //calculate decayed arrow (sticking) pos on hit
                Vector3 targetPos = GetTargetPosition();
                transform.position = targetPos;
                transform.parent = other.transform;

                if (hitEffect)
                {
                    GameObject effect = Instantiate(hitEffect, GetTargetPosition(), Quaternion.identity);
                }

                //handle last arrow that hits and kills enemy
                if (other.GetComponent<Health>().IsDead())
                {
                    Destroy(gameObject);
                    return;
                }

                
                //Destroy incrementally
                foreach (GameObject gameObjectToDestroy in destroyOnHit)
                {
                    Destroy(gameObjectToDestroy);
                }

                ;
                //destroy total gameobject
                Destroy(gameObject, projectileDestroySpeed);

                TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
                if (trail)
                    trail.enabled = false;
                
                
                reachedCollider = true;
            }
        }
    }
}
