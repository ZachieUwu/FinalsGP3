using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool canMove = true;

    public float horizontal;
    public float speed = 16f;
    public float jumpingPower = 40f;
    private bool isFacingRight = true;
    private bool isWalking;
    private bool wasGrounded;

    private IInteractable interactableObject;
    private GameObject interactItem;

    AudioManager sfx;
    AudioSource walksfx;

    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public GameObject door;
    [SerializeField] private int totalItems;
    [SerializeField] private int totalSecretItems; // Secret items count
    [SerializeField] public bool isSecondCharacter = false;
    [SerializeField] private bool hasEnoughItems = false;

    public Animator animator;
    //public PlayableDirector timeline;
    private Inventory saveItem;
    private Inventory normalInventory;
    private SItemMatch secretInventory;

    private void Awake()
    {
        sfx = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        normalInventory = GetComponent<Inventory>();
        secretInventory = GetComponent<SItemMatch>(); // Get secret inventory
        walksfx = GetComponent<AudioSource>();
        door.SetActive(false);
    }

    void Update()
    {
        if (canMove)
        {
            PlayerControl();
        }
    }

    public void PlayerControl()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            animator.SetBool("isJump", true);
            sfx.PlaySFX(sfx.jump);
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
                //timeline?.Play();
                CollectItem(interactItem, interactItem.CompareTag("SecretItem"));
            }
        }

        HandleWalkingSFX();
        HandleLandingSFX();
        Flip();
    }

    public void CanMove(bool value)
    {
        canMove = value;
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
        if (other.CompareTag("Item") || other.CompareTag("SecretItem"))
        {
            interactItem = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.CompareTag("Item") || other.CompareTag("SecretItem")) && interactItem == other.gameObject)
        {
            interactItem = null;
        }
    }

    private void CollectItem(GameObject item, bool isSecret)
    {
        if (isSecret)
        {
            if (secretInventory != null)
            {
                secretInventory.AddSecretItem(item);
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SetSecretItemCollected(isSecondCharacter);
                }
                else
                {
                    Debug.LogWarning("GameManager.Instance is null! Make sure GameManager is in the scene.");
                }
            }
        }
        else
        {
            if (normalInventory != null && normalInventory.collectedItems.Count < normalInventory.maxItems)
            {
                normalInventory.SetInteractableItem(item);
                CheckedItems();
            }
        }
    }

    private void CheckedItems()
    {
        if (!hasEnoughItems && normalInventory.collectedItems.Count == totalItems)
        {
            hasEnoughItems = true;
            door.SetActive(true); // Show door when all normal items are collected
        }
    }

}