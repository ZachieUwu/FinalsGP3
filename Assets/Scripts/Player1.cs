using UnityEngine;
using UnityEngine.Events;

public class Player1 : MonoBehaviour
{
    private float horizontal;
    private float speed = 10f;
    private float jumpingPower = 10f;
    private bool isFacingRight = true;
    private IInteractable interactableObject;
    private Inventory saveItem;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        saveItem = GetComponent<Inventory>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetKeyDown(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.E) && interactableObject != null)
        {
            interactableObject.Interact();
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            interactableObject = interactable;
        }
        else if (other.CompareTag("Item"))
        {
            CollectItem(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable) && interactableObject == interactable)
        {
            interactableObject = null;
        }
    }

    private void CollectItem(GameObject item)
    {
        if (saveItem != null && saveItem.collectedItems.Count < saveItem.maxItems)
        {
            saveItem.SetInteractableItem(item);
            item.SetActive(false);
        }
    }
}