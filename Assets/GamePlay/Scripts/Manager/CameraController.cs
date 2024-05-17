using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Transform playerPosition;
    public Vector3 positionOffset = new Vector3(20f, 15f, 0f);

    public Transform PlayerPosition { get => playerPosition; set => playerPosition = value; }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (playerPosition != null)
        {
            transform.position = playerPosition.position + positionOffset;
            transform.LookAt(playerPosition.position);
        }
    }
}