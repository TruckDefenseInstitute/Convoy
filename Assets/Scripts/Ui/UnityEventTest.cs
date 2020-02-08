using UnityEngine;
using UnityEngine.Events;
using System;

// Go ahead and put this component on a GameObject!
public class UnityEventTest : MonoBehaviour {
	
    // Similarly to how a UI Button has an OnClick event,
	// this component now has an OnMouseClick event which allows
	// you to hook up functions in the inspector.
	// Go ahead and hook up MouseClicked.
	
    [SerializeField]
	private UnityEvent OnMouseClick = new UnityEvent();

	private void Update() {
		// If left mouse button pressed
		if(Input.GetMouseButtonDown(0)) {
			OnMouseClick.Invoke();
		}
	}

	// Hook this up in the inspector.
	public void MouseClicked() {
		Debug.Log("Left Mouse Button has been pressed down!");
	}
}