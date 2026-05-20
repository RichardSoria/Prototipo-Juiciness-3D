using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;

    [SerializeField] private AudioSource oneShotSource;
    [SerializeField] private AudioSource movementSource;

    private Vector2 moveInput;
    private bool sprintInput;
    private bool wasGrounded;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 verticalVelocity;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip landClip;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip runClip;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        wasGrounded = controller.isGrounded;

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        MoveCharacter();
        UpdateAnimator();
        HandleLandingSound();
        HandleMovementSounds();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        sprintInput = value.isPressed;
    }

    public void OnJump(InputValue value)
    {
        // SOLUCIÓN AL SALTO: Se ejecuta instantáneamente al presionar el botón.
        // No hay variables booleanas que se pierdan por el camino.
        if (value.isPressed && controller.isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (jumpClip != null)
            {
                oneShotSource.PlayOneShot(jumpClip);
            }
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            animator.SetTrigger("attackTrigger");
            if (attackClip != null)
            {
                oneShotSource.PlayOneShot(attackClip);
            }
        }
    }

    private void MoveCharacter()
    {
        if (cameraTransform == null) return;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            float speed = sprintInput ? runSpeed : walkSpeed;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (controller.isGrounded)
        {
            if (verticalVelocity.y < 0)
            {
                verticalVelocity.y = -2f;
            }
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    private void UpdateAnimator()
    {
        bool hasMovement = moveInput.magnitude > 0.01f;

        animator.SetBool("isWalking", hasMovement && !sprintInput);
        animator.SetBool("isRunning", hasMovement && sprintInput);
        animator.SetBool("isJumping", !controller.isGrounded);
    }

    private void HandleLandingSound()
    {
        if (!wasGrounded && controller.isGrounded)
        {
            if (landClip != null)
            {
                oneShotSource.PlayOneShot(landClip);
            }
        }
        wasGrounded = controller.isGrounded;
    }

    private void HandleMovementSounds()
    {
        // 1. Si no toca el suelo, corta los pasos al instante.
        if (!controller.isGrounded)
        {
            if (movementSource.isPlaying) movementSource.Stop();
            return;
        }

        bool isMoving = moveInput.magnitude > 0.01f;

        if (isMoving)
        {
            AudioClip clipToPlay = sprintInput ? runClip : walkClip;

            if (movementSource.clip != clipToPlay || !movementSource.isPlaying)
            {
                movementSource.clip = clipToPlay;
                movementSource.loop = true;
                movementSource.Play();
            }
        }
        else
        {
            if (movementSource.isPlaying)
            {
                movementSource.Stop();
            }
        }
    }
    // --- MÉTODO PARA FUERZAS EXTERNAS (TRAMPOLÍN) ---
    public void AplicarRebote(float alturaRebote)
    {
        // Usamos la misma fórmula de tu salto, pero con la altura que nos dé el trampolín
        verticalVelocity.y = Mathf.Sqrt(alturaRebote * -2f * gravity);

        // Opcional: Si quieres que suene el salto al rebotar
        if (jumpClip != null && oneShotSource != null)
        {
            oneShotSource.PlayOneShot(jumpClip);
        }
    }
}