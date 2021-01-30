using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private float movementX;
    private float movementY;
    public float speed = 1;
    public float maxVelocity = 20f;
    private Player1Controls controls;
    private Vector2 lookDirection;

    void Awake()
    {
        controls = new Player1Controls();
        controls.Player.Look.performed += Aim;
    }

    private void Aim(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>();
        Debug.Log(lookDirection);
        Vector2 direction = Camera.main.ScreenToWorldPoint(lookDirection) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        var rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
        transform.rotation = rotation;
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 1f);
        //Quaternion rotation = Quaternion.LookRotation(lookDirection);
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = rotation;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
 
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {    
        controls.Player.Disable();
    }

   private void OnMove(InputValue movementValue)
   {
       Vector2 movementVector = movementValue.Get<Vector2>();
       movementX = movementVector.x;
       movementY = movementVector.y;
   }
  
   void FixedUpdate()
   {
       Vector3 movement = new Vector2(movementX, movementY);
       if (rigidBody.velocity.sqrMagnitude < maxVelocity)
       {
           rigidBody.AddForce(movement * speed);
       }
   }
}