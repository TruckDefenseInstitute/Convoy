using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public Alignment Alignment;
    public int MaxHP;
    public int RemainingHP;

    public Alignment GetAlignment()
    {
        return Alignment;
    }

    public void TakeDamage(int damage)
    {
        int predictedHP = RemainingHP - damage;

        RemainingHP = predictedHP <= 0 ? 0 : predictedHP;

        if (RemainingHP == 0)
        {
            Die();
        }
    }

    private void Die()
    {
            Debug.Log(gameObject.name + " is ded!");
            gameObject.transform.root.gameObject.SetActive(false);
            Destroy(gameObject.transform.root.gameObject);
    }
}
