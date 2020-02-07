using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{

    public Transform player;

    private void LateUpdate()
    {
        // To be used if minimap only shows a portion of the whole map.
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }

}