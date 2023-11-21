using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    public float speed = 5f; // Movement speed
    public float rotationSpeed = 720f; // Rotation speed in degrees per second for spin attack

    private PlayerControls playerControls;
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private bool isMovementPressed;
    private bool isAttacking = false; // Flag to check if the character is currently attacking
    public bool isAlive = true;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerControls = new PlayerControls();

        playerControls.Player.Move.started += OnMovementInput;
        playerControls.Player.Move.canceled += OnMovementInput;
        playerControls.Player.Move.performed += OnMovementInput;

        playerControls.Player.Attack.started += OnAttack;
    }
    private void OnEnable()
    {
        playerControls.Player.Enable();
    }
    private void OnDisable()
    {
        playerControls.Player.Disable();
        GameManager.instance.OnHealthChanged -= HandleHealthChanged;
    }

    public void Start()
    {
        GameManager.instance.OnHealthChanged += HandleHealthChanged;
        Debug.Log(GameManager.instance.Health);
    }
    private void Update()
    {
        if(!isAlive)
        {
            return;
        }
        UpdateAnimation();
        HandleMovement();
        HandleRotation();
    }

    private void HandleHealthChanged(int newHealth)
    {
        Debug.Log($"Health: {newHealth}");
        animator.SetInteger("Health", newHealth);
        if (newHealth < 0)
        {
            StartCoroutine(Die());
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Damage Taken: {damage}");
        GameManager.instance.Health -= damage;
    }    

    private IEnumerator Die()
    {
        Debug.Log("dead");
        isAlive = false;
        // Wait for the animator to start the expected animation state.
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));
        // Now wait until the animation is no longer playing.
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));

    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!isAttacking)
        {
            Debug.Log("Attack!");
            StartCoroutine(PerformSpinAttack());
        }
    }

    private IEnumerator PerformSpinAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack"); // Trigger the attack animation

        // Wait for the animator to start the expected animation state.
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        // Now wait until the animation is no longer playing.
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));

        // Optionally wait for a transition to finish if you want to ensure complete smoothness.
        yield return new WaitWhile(() => animator.IsInTransition(0));

        isAttacking = false;
    }

    private void HandleMovement()
    {
        if (isMovementPressed)
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * currentMovement.z + right * currentMovement.x).normalized;
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        if (isMovementPressed)
        {
            Vector3 direction = new Vector3(currentMovement.x, 0, currentMovement.z);
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void UpdateAnimation()
    {
        // Set the speed based on the input magnitude
        float speedValue = isMovementPressed ? currentMovementInput.magnitude : 0f;
        animator.SetFloat("Speed", speedValue, 0.1f, Time.deltaTime);
    }
}
