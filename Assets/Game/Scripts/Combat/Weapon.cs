using UnityEngine;


namespace  RPG.Combat
{
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons", order = 0)]
    public class Weapon : ScriptableObject
    {
        [Header("Core")]
        [SerializeField] private AnimatorOverrideController weaponOverride;
        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile;
        
        [Header("Stats")]
        [SerializeField] private float weaponDamage;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks;

       

        private const string weaponName = "weapon";

        public void SpawnToPlayer(Transform rightHandPos, Transform lefthandPos, Animator anim)
        {
            //unequip preexisting weapon
            DestroyWeaponOnPlayer(rightHandPos, lefthandPos, anim);
            
            //fields not null (i.e not unarmed)
            if (weaponPrefab)
            {
                Transform handPos = FindTransformOfHand(rightHandPos, lefthandPos);
                GameObject wepInScene = Instantiate(weaponPrefab, handPos);
                wepInScene.name = weaponName;
            }

            if(weaponOverride)
                anim.runtimeAnimatorController = weaponOverride;
        }

        private Transform FindTransformOfHand(Transform rightHandPos, Transform lefthandPos)
        {
            return isRightHanded ? rightHandPos : lefthandPos;
        }

        public void DestroyWeaponOnPlayer(Transform rightHandPos, Transform leftHandPos, Animator anim)
        {
            DestroyWeaponOnHand(rightHandPos);
            DestroyWeaponOnHand(leftHandPos);

        }

        private void DestroyWeaponOnHand(Transform handPos)
        {
             Transform handWep = handPos.Find(weaponName);
             if (handWep)
             {
                 handWep.name = "DESTROYING";
                 Destroy(handWep.gameObject);
             }
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
        
        public float GetWeaponRange()
        {
            return weaponRange;
        }
        
        public float GetTimeBetweenAttacks()
        {
            return timeBetweenAttacks;
        }

        public void SpawnProjectile(Transform target, Transform rightHand, Transform leftHand)
        {
            var proj = Instantiate(projectile, FindTransformOfHand(rightHand, leftHand).position, Quaternion.identity);
            proj.SetTarget(target, weaponDamage);
        }

        public bool IsRanged()
        {
            return projectile;
        }
        
    }

}
