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
            var dir = (Vector3.up + Random.onUnitSphere).normalized;
            var potato = Instantiate(Scrap, transform.position + dir, Quaternion.LookRotation(dir));
            if (potato.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                rb.velocity = dir * 8.0f;
            } else {
                var arr = potato.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody crb in arr) {
                    crb.velocity = dir * Random.Range(5.0f, 10.0f);
                }
            }
            if (ScrapMesh.Length != 0) {
                Instantiate(ScrapMesh[Random.Range(0, ScrapMesh.Length - 1)], potato.transform);
            }
        }
    }
}
