using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance;

    public static T Instance {
        get {
            return _instance;
        }
        private set {
            if (_instance == null) {
                _instance = value;
                //DontDestroyOnLoad(_instance.gameObject);
            } else if (_instance != value) {
                Destroy(value.gameObject);
            }
        }
    }
    public virtual void Awake() {
        Instance = this as T;
    }
}