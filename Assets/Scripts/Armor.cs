using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour {
    public float GunDamageMult = 1f;
    public float CannonDamageMult = 1f;
    public float RocketDamageMult = 1f;
    public float GrenadeDamageMult = 1f;
    public float SniperDamageMult = 1f;

    public float ReduceDamage(DamageMetadata dm) {
        float dmg = dm.Damage;
        switch (dm.DamageType) {
            case DamageType.Gun:
                return dmg *= GunDamageMult;
            case DamageType.Cannon:
                return dmg *= CannonDamageMult;
            case DamageType.Grenade:
                return dmg *= GrenadeDamageMult;
            case DamageType.Rocket:
                return dmg *= RocketDamageMult;
            case DamageType.Sniper:
                return dmg *= SniperDamageMult;
            default:
                return dmg;
        }
    }
}