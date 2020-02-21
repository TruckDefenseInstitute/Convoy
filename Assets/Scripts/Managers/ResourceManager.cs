using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    float _resource;


    // Start is called before the first frame update
    void Start()
    {
        _resource = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _resource += GetResourceGainByTruck();
    }

    public float GetResourceAmount()
    {
        return _resource;
    }

    public void DeductResource(float deductAmount)
    {
        // Assume that there is enough resource to deduct.
        _resource -= deductAmount;
    }

    public bool ResourcesEqualOrGreaterThan(float compareAmount)
    {
        return _resource >= compareAmount;
    }

    float GetResourceGainByTruck()
    {
        return 1f;
    }
}
