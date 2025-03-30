using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 16f;
    private float jumpingPower = 40f;
    private bool isFacingRight = true;
    private IInteractable interactableObject;
    private GameObject interactItem;
    public Animator animator;
    private Inventory saveItem;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject door;
    [SerializeField] private int totalItems;

    void Start()
    {
        saveItem = GetComponent<Inventory>();
        door.SetActive(false);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            animator.SetBool("isJump", true);
        }

        if (!IsGrounded())
        {
            animator.SetBool("isJump", true);
        }

        if (rb.linearVelocity.y < 1f && IsGrounded())
        {
            animator.SetBool("isJump", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactableObject != null)
            {
                interactableObject.Interact();
            }
            else if (interactItem != null)
            {
                CollectItem(interactItem);
            }
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
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
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
            interactItem = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable) && interactableObject == interactable)
        {
            interactableObject = null;
        }
        else if (other.CompareTag("Item") && interactItem == other.gameObject)
        {
            interactItem = null;
        }
    }

    private void CollectItem(GameObject item)
    {
        if (saveItem != null && saveItem.collectedItems.Count < saveItem.maxItems)
        {
            saveItem.SetInteractableItem(item);
            item.SetActive(false);
            CheckedItems();
        }
    }

    private void CheckedItems()
    {
        if (saveItem.collectedItems.Count >= totalItems)
        {
            door.SetActive(true);
        }
    }
}
