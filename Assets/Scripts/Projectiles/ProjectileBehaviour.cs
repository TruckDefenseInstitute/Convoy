using UnityEngine;

public abstract class ProjectileBehaviour : MonoBehaviour
{
    public int Damage;

    // Passed from Tower
    protected Alignment Alignment;
    protected DamageReceiver Target;
    protected float ShootDuration;

    public void Init(Alignment alignment, DamageReceiver target, float duration)
    {
        this.Alignment = alignment;
        this.Target = target;
        this.ShootDuration = duration;
    }
}
