using System.Collections;
using UnityEngine;

public class LaserProjectileBehaviour : ProjectileBehaviour
{
    public LineRenderer LineRenderer;

    void Start()
    {
        StartCoroutine(DestroyCoroutine());

        // Setup LineRenderer
        LineRenderer.SetPosition(0, transform.position);
        LineRenderer.SetPosition(1, transform.position);

        Raycast(transform.position);
    }

    void Raycast(Vector3 v)
    {
        RaycastHit hit;
        if (Physics.Raycast(v, transform.forward, out hit))
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

            // The collider belongs to DamageReceiver
            // DamageReceiver is not on the same Team
            if (d.Collider == hit.collider && d.Alignment != Alignment)
            {
                // Damage over time
                // Ensure that enemy will recieve 'Damage' total damage
                d.TakeDamage(Damage / ShootDuration * Time.deltaTime);
                LineRenderer.SetPosition(1, hit.point);
                return;
            }

            // Hit some trigger, continue raycasting
            Raycast(hit.point);
            return;
        }

        LineRenderer.SetPosition(1, v);
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(ShootDuration);
        Destroy(gameObject);
    }
}
