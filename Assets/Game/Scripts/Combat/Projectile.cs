
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
