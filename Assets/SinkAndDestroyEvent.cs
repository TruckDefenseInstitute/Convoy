using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkAndDestroyEvent : ScriptedEvent
{
    public GameObject[] StuffToDestroy;
    public float SinkTime = 10;
    public float SinkSpeed = 1;

    bool started = false;

    public override void Trigger() {
        started = true;
        Invoke("DestroyAll", SinkTime);
    }

    private void Update() {
        if (!started) {
            return;
        }

        foreach (GameObject go in StuffToDestroy) {
            var p = go.transform.position;
            p.y -= SinkSpeed * Time.deltaTime;
            go.transform.position = p;
        }
    }

    void DestroyAll() {
        foreach (GameObject go in StuffToDestroy) {
            Destroy(go);
        }
        Destroy(this);
    }
}
