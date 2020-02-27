using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketVolleyProjectile : MonoBehaviour {
    public GameObject Rocket;
    public Alignment TargetedAlignment;
    public int NumberOfRockets = 2;
    public float SecondsBetweenRockets = 0.05f;

    [HideInInspector]
    public DamageMetadata DamageMetadata;
    [HideInInspector]
    public GameObject Target;

    private int _rocketsFired = 0;

    // Start is called before the first frame update
    void Start()
    {
        DamageMetadata = new DamageMetadata(DamageMetadata.Damage / NumberOfRockets, DamageMetadata.DamageType);
        FireRocket();
    }

    void FireRocket() {
        if (_rocketsFired >= NumberOfRockets) {
            Destroy(gameObject);
            return;
        }

        Quaternion q = Quaternion.identity;
        q.SetLookRotation(transform.forward * 0.5f + Random.Range(-1f, 1f) * Vector3.Cross(transform.forward, Vector3.up));
        var rocket = Instantiate(Rocket, transform.position, q).GetComponent<RocketProjectile>();
        rocket.DamageMetadata = DamageMetadata;
        rocket.Target = Target.gameObject;
        rocket.TargetedAlignment = TargetedAlignment;

        ++_rocketsFired;
        Invoke("FireRocket", SecondsBetweenRockets);
    }
}
