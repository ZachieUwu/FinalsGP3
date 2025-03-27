using UnityEngine;

public class Mirrored : MonoBehaviour
{
    public Transform redPlayer;
    public Transform bluePlayer;

    [SerializeField] private Rigidbody2D redRb;
    [SerializeField] private Rigidbody2D blueRb;

    private bool isFacingRightRed = true;
    private bool isFacingRightBlue = false;

    private float speed = 8f;
    private float jumpPower = 10f;
    private LayerMask groundLayer;

    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        if (!IsComponentsValid()) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        HandleMovement(horizontalInput);
        HandleJumping();
        HandleFlipping(horizontalInput);
    }

    private bool IsComponentsValid()
    {
        return redPlayer != null && bluePlayer != null && redRb != null && blueRb != null;
    }

    private void HandleMovement(float horizontalInput)
    {
        redRb.linearVelocity = new Vector2(horizontalInput * speed, redRb.linearVelocity.y);
        blueRb.linearVelocity = new Vector2(-horizontalInput * speed, blueRb.linearVelocity.y);
    }

    private void HandleJumping()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded(redPlayer)) redRb.linearVelocity = new Vector2(redRb.linearVelocity.x, jumpPower);
            if (IsGrounded(bluePlayer)) blueRb.linearVelocity = new Vector2(blueRb.linearVelocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (redRb.linearVelocity.y > 0f) redRb.linearVelocity = new Vector2(redRb.linearVelocity.x, redRb.linearVelocity.y * 0.5f);
            if (blueRb.linearVelocity.y > 0f) blueRb.linearVelocity = new Vector2(blueRb.linearVelocity.x, blueRb.linearVelocity.y * 0.5f);
        }
    }

    private bool IsGrounded(Transform player)
    {
        Transform groundCheck = player.GetChild(0);
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void HandleFlipping(float horizontalInput)
    {
        if ((horizontalInput > 0 && !isFacingRightRed) || (horizontalInput < 0 && isFacingRightRed))
        {
            FlipCharacter(redPlayer, ref isFacingRightRed);
        }

        if ((horizontalInput > 0 && !isFacingRightBlue) || (horizontalInput < 0 && isFacingRightBlue))
        {
            FlipCharacter(bluePlayer, ref isFacingRightBlue);
        }
    }

    private void FlipCharacter(Transform player, ref bool isFacingRight)
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = player.localScale;
        scale.x *= -1;
        player.localScale = scale;
    }
}
