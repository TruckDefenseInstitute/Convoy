using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathrattle : MonoBehaviour
{
    public GameObject[] ScrapMesh;
    public GameObject Scrap;
    public int ScrapsToSpawn = 3;

    // Start is called before the first frame update
    void Start()
    {
        Explode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode() {
        for (int i = 0; i < ScrapsToSpawn; ++i) {
            var dir = Vector3.up + Random.onUnitSphere;
            var potato = Instantiate(Scrap, transform.position, Quaternion.LookRotation(dir));
            potato.GetComponent<Rigidbody>().velocity = dir.normalized * 8.0f;
            Instantiate(ScrapMesh[Random.Range(0, ScrapMesh.Length - 1)], potato.transform);
        }
    }
}
