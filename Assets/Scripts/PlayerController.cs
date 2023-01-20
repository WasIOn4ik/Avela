using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : NetworkBehaviour
{
    protected Vector3 moveDirection;
    [SerializeField] float speed = 2f;

    public void OnMovement(InputValue value)
    {
        var vec = value.Get<Vector2>();
        moveDirection = Vector3.zero;
        moveDirection.x += vec.x;
        moveDirection.z += vec.y;
    }

    public void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
