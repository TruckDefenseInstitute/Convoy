using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalStrikeProjectile : MonoBehaviour
{
    public float AdvanceWarning = 3;
    public float Speed = 100;
    public float Damage = 500;
    public DamageType DamageType = DamageType.Rocket;
    public GameObject Explosion;
    public GameObject Warning;

    private float _blastRadius = 3.0f;
    private Alignment _alignment = Alignment.Hostile;

    bool exploded = false;

    // Start is called before the first frame update
    void Start()
    {
        var a = transform.position;
        a.y = AdvanceWarning * Speed;
        transform.position = a;

        a.y = 1;
        Instantiate(Warning, a, transform.rotation).GetComponent<OrbitalStrikeAlert>().Warning = AdvanceWarning - 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (exploded) {
            return;
        }

        var a = transform.position;
        a.y -= Speed * Time.deltaTime;
        a.y = Mathf.Max(0, a.y);
        transform.position = a;

        if (transform.position.y <= 0) {
            Explode();
        }
    }

    void Explode() {
        exploded = true;
        Instantiate(Explosion, transform.position, transform.rotation);
        var sc = gameObject.AddComponent<SphereCollider>();
        sc.radius = _blastRadius;
        sc.isTrigger = true;
        Destroy(gameObject, 0.05f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<Unit>(out Unit u)) {
            if (u.Alignment != _alignment && Vector3.Distance(u.transform.position, transform.position) <= _blastRadius) {
                u.TakeDamage(new DamageMetadata(Damage, DamageType));
            }
        }
    }
}
