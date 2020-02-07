using UnityEngine;
using System.Collections.Generic;

public class UnitControlSystem : MonoBehaviour
{

    public Unit selectedUnit;

    // Update is called once per frame
    void Update() {

        // LMB Pressed
        if (Input.GetMouseButtonDown(0)) {
            Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            //Shoots a ray into the 3D world starting at our mouseposition
            if (Physics.Raycast(mouseToWorldRay, out hitInfo)) {
                //We check if we clicked on an object with a Selectable component
                Unit selectedObject = hitInfo.collider.GetComponent<Unit>();
                if (selectedObject == null) {
                    selectedUnit = null;
                } else {
                    //If we clicked on a Selectable, we don't want to enable our SelectionBox
                    selectedUnit = selectedObject;
                }
            }
        }

        // RMB Held
        if (Input.GetMouseButtonDown(1) && selectedUnit != null) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                selectedUnit.Move(hit);
            }
        }

    }

}
