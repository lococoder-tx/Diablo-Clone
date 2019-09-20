using UnityEngine;


namespace  RPG.Combat
{
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons", order = 0)]
    public class Weapon : ScriptableObject
    {
        [Header("Core")]
        [SerializeField] private AnimatorOverrideController weaponOverride;
        [SerializeField] private GameObject weaponPrefab;
        
        [Header("Stats")]
        [SerializeField] private float weaponDamage;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks;

        public void Spawn(Transform handPos, Animator anim)
        {
            //fields not null (i.e not unarmed)
            if(weaponPrefab)
                Instantiate(weaponPrefab, handPos);
            
            if(weaponOverride)
                anim.runtimeAnimatorController = weaponOverride;
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

    }

}
