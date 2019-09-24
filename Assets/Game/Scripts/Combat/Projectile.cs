
using RPG.Saving;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(GetTargetPosition());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //reset rotation to base of 90 since lookAt fucks with it
        transform.Rotate(90, 0 ,0);
    }

    private Vector3 GetTargetPosition()
    {
        CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
        return (target.position + Vector3.up * targetCollider.height / 2);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.gameObject)
            Destroy(gameObject);
    }
}
