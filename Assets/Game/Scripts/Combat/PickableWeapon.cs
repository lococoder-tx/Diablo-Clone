using RPG.Combat;
using RPG.Controller;
using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    private void OnTriggerEnter(Collider other)
    {
        Fighter fighter = other.GetComponent<Fighter>();
        if (fighter && other.GetComponent<PlayerController>())
        {
           
            fighter.EquipWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
