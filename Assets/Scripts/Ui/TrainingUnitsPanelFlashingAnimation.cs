using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingUnitsPanelFlashingAnimation : MonoBehaviour
{
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }


    public void MakePanelFlash()
    {
        _animator.SetBool("isFlashing", true);
    }

    public void MakePanelStopFlashing()
    {
        _animator.SetBool("isFlashing", false);
    }
}
