using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float horizontal;
    private float speed = 16f;
    private float jumpingPower = 40f;
    private bool isFacingRight = true;
    private bool isWalking;
    private bool wasGrounded;
    private IInteractable interactableObject;
    private GameObject interactItem;
    public Animator animator;
    private Inventory saveItem;

    AudioManager sfx;
    AudioSource walksfx;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject door;
    [SerializeField] private int totalItems;

    private void Awake()
    {
        sfx = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        saveItem = GetComponent<Inventory>();
        walksfx = GetComponent<AudioSource>();
        door.SetActive(false);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            sfx.PlaySFX(sfx.jump);
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
                sfx.PlaySFX(sfx.portal);
                interactableObject.Interact();
            }
            else if (interactItem != null)
            {
                sfx.PlaySFX(sfx.pick);
                CollectItem(interactItem);
            }
        }

        HandleWalkingSFX();
        HandleLandingSFX();
        Flip();
    }

    private void HandleWalkingSFX()
    {
        if (IsGrounded() && Mathf.Abs(horizontal) > 0)
        {
            if (!isWalking && !walksfx.isPlaying)
            {
                walksfx.Play();
                isWalking = true;
            }
        }
        else
        {
            walksfx.Stop();
            isWalking = false;
        }
    }

    private void HandleLandingSFX()
    {
        if (!wasGrounded && IsGrounded())
        {
            sfx.PlaySFX(sfx.landing);
        }
        wasGrounded = IsGrounded();
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
