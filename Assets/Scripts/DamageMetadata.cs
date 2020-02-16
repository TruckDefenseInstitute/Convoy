using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMetadata {
    public float Damage;
    public DamageType DamageType;

    public DamageMetadata(float damage, DamageType damageType) {
        Damage = damage;
        DamageType = damageType;
    }
}