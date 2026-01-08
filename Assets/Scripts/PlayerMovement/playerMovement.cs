using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    public DPADControl dPad; // optional DPad input
    [HideInInspector] public bool canMove = true;

    [Header("Farming")]
    public FarmTileManager farmManager;
    public CropData defaultCrop; // Assign a default crop to plant

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dPad = GetComponent<DPADControl>();
    }

    // Input System movement callback
    public void OnMovement(InputValue value)
    {
        if (canMove)
        {
            movement = value.Get<Vector2>();
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = movement;

        // Use DPad input if available
        if (canMove && dPad != null)
            moveInput = dPad.inputVector;

        // Move Rigidbody
        if (moveInput != Vector2.zero)
        {
            rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);

            // Update animator
            animator.SetFloat("X", moveInput.x);
            animator.SetFloat("Y", moveInput.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    #region Farming Buttons (Call from UI Buttons)
    public void Hoe()
    {
        farmManager.HoeTile(transform.position);
        if (animator != null) animator.SetTrigger("Hoe");
    }

    public void Plant()
    {
        farmManager.PlantSeed(transform.position, defaultCrop);
        if (animator != null) animator.SetTrigger("Plant");
    }

    public void Water()
    {
        farmManager.WaterTile(transform.position);
        if (animator != null) animator.SetTrigger("Water");
    }

    public void Harvest()
    {
        farmManager.HarvestTile(transform.position);
        if (animator != null) animator.SetTrigger("Harvest");
    }
    #endregion
}
