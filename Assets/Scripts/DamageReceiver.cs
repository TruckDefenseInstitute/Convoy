using System;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public Alignment Alignment;
    public Collider Collider;
    public MonoBehaviour Behaviour;
    public GameObject DamageTarget;

    public void TakeDamage(float damage)
    {
        IDamageReceiver d = Behaviour as IDamageReceiver;
        if (d == null)
        {
            throw new Exception("Behaviour does not implement 'IDamageReceiver'.");
        }
        d.Damage(damage);
    }
}
