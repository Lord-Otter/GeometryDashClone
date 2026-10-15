using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Rigidbody2D rigidbody = null;
    public SpriteRenderer spriteRenderer = null;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float raycastLength = 0.05f;
    public Vector2 raycastOffset = new Vector2(0f, -0.5f);

    [Header("Input Settings")]
    public float inputBufferTime = 0.5f;
    public float coyoteTime = 0.1f;

    [SerializeField] public CubePlayerState cubeState = new CubePlayerState();
    [SerializeField] public RocketPlayerState rocketState = new RocketPlayerState();

    private PlayerState currentState = null;

    private bool isInputHeld = false;
    private float inputBufferCounter = 0f;
    private float coyoteTimeCounter = 0f;

    public bool IsActionActive => isInputHeld || (inputBufferCounter > 0f);
    public bool CanJump => coyoteTimeCounter > 0f;

    public void OnValidate()
    {
        if(rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();
        if(spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if(raycastLength < 0f) raycastLength = 0.1f;
        if(inputBufferTime < 0f) inputBufferTime = 0f;
        if(coyoteTime < 0f) coyoteTime = 0f;

        cubeState.OnValidate(this);
        rocketState.OnValidate(this);
    }

    public void Awake()
    {
        cubeState.Awake();
        rocketState.Awake();
    }

    public void Start()
    {
        cubeState.Start();
        rocketState.Start();

        currentState = cubeState;
        currentState.Enter();
    }

    public void Update()
    {
        isInputHeld = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            inputBufferCounter = inputBufferTime;
        }
        else if(inputBufferCounter > 0f)
        {
            inputBufferCounter -= Time.deltaTime;
        }
    }

    public void FixedUpdate()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else if(coyoteTimeCounter > 0f)
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        if(currentState != null)
        {
            currentState.FixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState != null)
        {
            currentState.OnTriggerEnter2D(collision);
        }
    }

    public void Transit(PlayerState targetState)
    {
        if(currentState != null) currentState.Exit();
        currentState = targetState;
        if(currentState != null) currentState.Enter();
    }

    public bool IsGrounded()
    {
        Vector2 rayOrigin = (Vector2)transform.position + raycastOffset;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, raycastLength, groundLayer);

        Debug.DrawRay(rayOrigin, Vector2.down * raycastLength, hit.collider != null ? Color.green : Color.red);

        return hit.collider != null;
    }

    public void ConsumeJump()
    {
        inputBufferCounter = 0f;
        coyoteTimeCounter = 0f;
    }
}
