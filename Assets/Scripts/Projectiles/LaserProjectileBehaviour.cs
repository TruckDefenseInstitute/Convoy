using System.Collections;
using UnityEngine;

public class LaserProjectileBehaviour : ProjectileBehaviour
{
    public LineRenderer LineRenderer;

    Vector3 _position;

    void Start()
    {
        StartCoroutine(DestroyCoroutine());
        _position = Target.transform.position;
    }

    void Update()
    {
        LineRenderer.SetPosition(0, transform.position);
        LineRenderer.SetPosition(1, _position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            DamageReceiver d = hit.collider.GetComponent<DamageReceiver>();

            if (d == null)
            {
                // Collider is some terrain
                if (!hit.collider.isTrigger)
                {
                    LineRenderer.SetPosition(1, hit.point);
                }
                return;
            }

            if (d.Alignment != Alignment)
            {
                // Damage over time
                // Ensure that enemy will recieve 'Damage' total damage
                d.TakeDamage(Damage / ShootDuration * Time.deltaTime);
            }
        }
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(ShootDuration);
        Destroy(gameObject);
    }
}
