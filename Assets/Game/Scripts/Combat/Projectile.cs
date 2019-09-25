using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] private Transform target;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float thisProjectileDamage = 0f;
        private float damage;

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(GetTargetPosition());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
            if (other.gameObject == target.gameObject)
            {
                other.GetComponent<Health>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
