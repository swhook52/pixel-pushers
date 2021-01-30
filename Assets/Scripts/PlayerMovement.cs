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
    private Vector2 lookInput;
    public float AimWidth = 0.1f;
    public float AimLength = 20f;
    private LineRenderer laserLineRenderer;
    private Vector3 worldLookLocation;
    public string WallTag = "Wall";
    public Transform GunTip;

    void Awake()
    {
        controls = new Player1Controls();
        controls.Player.Look.performed += Aim;
    }

    void Start()
    {
<<<<<<< Updated upstream:Assets/Scripts/PlayerMovement.cs
        lookDirection = context.ReadValue<Vector2>();
        Debug.Log(lookDirection);
        Vector2 direction = Camera.main.ScreenToWorldPoint(lookDirection) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        var rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
        transform.rotation = rotation;
=======
        rigidBody = GetComponent<Rigidbody2D>();
        laserLineRenderer = GetComponent<LineRenderer>();
        laserLineRenderer.startWidth = AimWidth;
        laserLineRenderer.endWidth = AimWidth;
>>>>>>> Stashed changes:Assets/Scripts/PlayerController.cs
    }

    private void Aim(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
        worldLookLocation = Camera.main.ScreenToWorldPoint(lookInput);

        Vector2 direction = worldLookLocation - transform.position;
        var ray = new Ray2D(this.transform.position, direction);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 3, false);

        RaycastHit2D raycastHit = Physics2D.Raycast(GunTip.position, Vector2.right, Mathf.Infinity, LayerMask.NameToLayer("Wall"));
        Debug.Log(raycastHit);
        if (raycastHit.rigidbody != null)
        {
            //Debug.Log(raycastHit.rigidbody.name);
            //worldLookLocation = new Vector3(raycastHit.point.x, raycastHit.point.y, 0);
        }
        else
        {
            Debug.Log("Did not hit anything");
        }
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

        // Aim Sight
        laserLineRenderer.SetPosition(0, this.transform.position);
        laserLineRenderer.SetPosition(1, new Vector3(worldLookLocation.x, worldLookLocation.y, 0));

        // Handle Rotation
        Vector2 direction = worldLookLocation - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
        transform.rotation = rotation;
    }
}