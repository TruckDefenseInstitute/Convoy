using UnityEngine;

public abstract class ProjectileBehaviour : MonoBehaviour
{
    public int Damage;

    // Passed from Tower
    protected Alignment Alignment;
    protected DamageReceiver Target;

    public void Init(Alignment alignment, DamageReceiver target) {
        this.Alignment = alignment;
        this.Target = target;
    }
}
