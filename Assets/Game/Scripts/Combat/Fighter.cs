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
        [Header("Fighter Stats")]
        //[SerializeField] private float weaponRange = 2f;
        //[SerializeField] private float timeBetweenAttacks;
        //[SerializeField] private float weaponDamage;
        
        [Header("Weapon")]
        [SerializeField] private Transform rightHandPosition = null;
        [SerializeField] private Transform leftHandPosition = null;
        [SerializeField] private Weapon defaultWeapon = null;
        [SerializeField] private Weapon equippedWeapon = null;

        [Header("")]
        public float timer = 20;
        
        //references 
        public Health target; //serialized for debug
        
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
            EquipWeapon(defaultWeapon);

        }

        public void EquipWeapon(Weapon weapon)
        {
            equippedWeapon = weapon;
            weapon.SpawnToPlayer(rightHandPosition, leftHandPosition, anim);
        }

        public void UnequipWeapon()
        {
            if (equippedWeapon == defaultWeapon) return;
            equippedWeapon.DestroyWeaponOnPlayer(rightHandPosition, leftHandPosition, anim);
        }
        
        
        void Update()
        {
            
            
            timer += Time.deltaTime;
            
            
            if (!target)
                return;
            
            
            
            if (!InRange())
            {
                    Debug.Log("hello");
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
            return distance < equippedWeapon.GetWeaponRange();
        }
        

        private void AttackBehavior()
        {
            if (target.IsDead())
            {
                Cancel();
                actionScheduler.CancelAction();
                
            }

            else if (timer >equippedWeapon.GetTimeBetweenAttacks())
            {
                anim.ResetTrigger("stopAttack");
                anim.SetTrigger("attack");
                
                if(equippedWeapon.IsRanged())
                    equippedWeapon.SpawnProjectile(target.transform, rightHandPosition, leftHandPosition);
                
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
        
        public bool HasTarget()
        {
            return target;
        }

        //animation event
        void Hit()
        {
            Debug.Log("here");
            if (!target) return;
            
            target.TakeDamage(equippedWeapon.GetWeaponDamage());
            
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if(equippedWeapon)
                Gizmos.DrawWireSphere(transform.position,equippedWeapon.GetWeaponRange());
        }

        public void SetTarget(Health other)
        {
            target = other;
        }
    }
}