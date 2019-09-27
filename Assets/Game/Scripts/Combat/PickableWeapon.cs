using System.Collections;
using RPG.Combat;
using RPG.Controller;
using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private float respawnTime = 5f;
    private void OnTriggerEnter(Collider other)
    {
        Fighter fighter = other.GetComponent<Fighter>();
        if (fighter && other.GetComponent<PlayerController>())
        {
           
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }
    }

    private IEnumerator HideForSeconds(float seconds)
    {
        ShowPickup(false);
        yield return new WaitForSeconds(seconds);
        ShowPickup(true);

    }

    private void ShowPickup(bool shouldShow)
    {
        //transform.Find("childname") returns the child with name childname
        GetComponent<SphereCollider>().enabled = shouldShow;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(shouldShow);
        }
    }

   
    
}
