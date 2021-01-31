using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

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
    //private LineRenderer laserLineRenderer;
    private Vector3 worldLookLocation;
    public string WallTag = "Wall";
    public Transform GunTip;
    public Game GameManager;
    public Light2D gunLight;
    public GameObject bulletPrefab;
    public int startingHealth = 25;
    public int currentHealth = 25;
    public Animator playerAnimator;
    public bool IsMoving = false;
    private float rateOfFire = 1.0f;
    private float nextFire = -1f;
    private bool canFire = true;

    void Awake()
    {
        currentHealth = startingHealth;
        controls = new Player1Controls();
        controls.Player.Look.performed += Aim;
        controls.Player.Fire.performed += Fire;
        controls.Player.Move.canceled += StopMoving;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        //laserLineRenderer = GetComponent<LineRenderer>();
        //laserLineRenderer.startWidth = AimWidth;
        //laserLineRenderer.endWidth = AimWidth;
    }

    public void RemoveHealth(int damage) {
        if (currentHealth > 0) {
            currentHealth -= damage;
            Debug.Log(currentHealth);
        }
    }

    private void Aim(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
        worldLookLocation = Camera.main.ScreenToWorldPoint(lookInput);

        Vector2 direction = worldLookLocation - transform.position;

        RaycastHit2D raycastHit = Physics2D.Raycast(GunTip.position, direction, Mathf.Infinity, LayerMask.GetMask("Wall"));
        if (raycastHit.rigidbody != null)
        {
            worldLookLocation = new Vector3(raycastHit.point.x, raycastHit.point.y, 0);
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

        if (!IsMoving)
        {
            SoundManager.PlaySound("footsteps");
        }
        IsMoving = true;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector2(movementX, movementY);
        if (rigidBody.velocity.sqrMagnitude < maxVelocity)
        {
            rigidBody.AddForce(movement * speed);
        }

        // Aim Sight
        //laserLineRenderer.SetPosition(0, GunTip.position);
        //laserLineRenderer.SetPosition(1, new Vector3(worldLookLocation.x, worldLookLocation.y, 0));

        // Handle Rotation
        Vector2 direction = worldLookLocation - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = rotation;

        float gunLightRange = Vector2.Distance(rigidBody.position, worldLookLocation);

        if (gunLight) { gunLight.pointLightOuterRadius = gunLightRange; }


        UpdateCharacterAnimation(playerAnimator);

        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
            canFire = false;
        }
        else
        {
            canFire = true;
        }
    }

    void UpdateCharacterAnimation(Animator anim)
    {

        // Update animation on key press
        anim.SetFloat("velocity", Math.Abs(rigidBody.velocity.x) + Math.Abs(rigidBody.velocity.y));

        anim.SetBool("hasFlashlight", false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager != null)
        {
            if (collision.CompareTag("Finish"))
            {
                //Launch keypad
                GameManager.DisplayKeypad();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (GameManager != null)
        {
            if (other.CompareTag("Finish"))
            {
                GameManager.RemoveKeypad();
            }
        }
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (canFire == false) { return; }
        nextFire = rateOfFire;

        SoundManager.PlaySound("gunshot");
        ShakeCamera.Instance.Shake(2f, 0.05f);

        Vector2 direction = worldLookLocation - transform.position;

        GameObject go = Instantiate(bulletPrefab, GunTip.position, transform.rotation);
        go.GetComponent<Rigidbody2D>().velocity = direction * 3f;
        playerAnimator.SetTrigger("attack");
    }

    private void StopMoving(InputAction.CallbackContext context)
    {
        IsMoving = false;
        SoundManager.StopSound();
    }
}