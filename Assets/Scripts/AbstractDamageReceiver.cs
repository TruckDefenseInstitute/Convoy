using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDamageReceiver : MonoBehaviour
{
    public Alignment Alignment;

    public Alignment GetAlignment()
    {
        return Alignment;
    }
}
