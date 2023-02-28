using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private Vector2 moveInput;
    private bool interactInput;

    private Vector2 facinfDir;

    public LayerMask interactLayerMask;

    public Rigidbody2D rig;
    public SpriteRenderer sr;

    private void Update() {
        if (moveInput.magnitude != 0.0f) {
            facinfDir = moveInput.normalized;
            sr.flipX = moveInput.x > 0;
        }

        if (interactInput) {
            TryInteractTile();
            interactInput = false;
        }
    }

    private void FixedUpdate() {
        rig.velocity = moveInput.normalized * moveSpeed;
    }

    private void TryInteractTile() {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + facinfDir, Vector2.up, 0.1f, interactLayerMask);

        if (hit.collider) {
            FieldTile tile = hit.collider.GetComponent<FieldTile>();
            tile.Interact();
        }
    }

    public void onMoveInput(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    public void onInteractInput(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            interactInput = true;
        }
    }
}
