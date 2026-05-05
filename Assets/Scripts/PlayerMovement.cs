using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    
    private bool isJumping;
    private bool isGrounded;
	public bool isClimbing;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask collisionLayers;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    
    private Vector3 velocity = Vector3.zero;
	private float horizontalMovement;
	private float verticalMovement;

	void Update()
	{
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;
		verticalMovement = Input.GetAxis("Vertical") * moveSpeed;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }

        Flip(rb.linearVelocity.x);

        float characterVelocity = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", characterVelocity);
		animator.SetBool("isClimbing", isClimbing);
	}
    
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
        MovePlayer(horizontalMovement, verticalMovement);
    }

    void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
		if (!isClimbing)
		{
			Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        	rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);

        	if (isJumping)
        	{
            	rb.AddForce(new Vector2(0f, jumpForce));
            	isJumping = false;
        	}
		}
		else
		{
			Vector3 targetVelocity = new Vector2(0, _verticalMovement);
        	rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
		}
    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }else if (_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
