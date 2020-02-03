using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDamageReceiver : MonoBehaviour
{
    public Alignment Alignment;
    public int MaxHP;
    public int RemainingHP;

    public Alignment GetAlignment()
    {
        return Alignment;
    }
}
