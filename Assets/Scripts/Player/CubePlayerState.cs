using UnityEngine;

[System.Serializable]
public class CubePlayerState : PlayerState
{
    public float moveSpeed = 6f;
    public float jumpForce = 8f;
    public float gravityScale = 3f;
    public float maxFallSpeed = 15f;
    public Sprite cubeSprite;

    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
        if(maxFallSpeed < 0f) maxFallSpeed = 0f;
    }

    public override void Enter()
    {
        if(player.spriteRenderer != null && cubeSprite != null)
        {
            player.spriteRenderer.sprite = cubeSprite;
        }

        player.rigidbody.gravityScale = gravityScale;
    }

    public override void FixedUpdate()
    {
        Vector2 velocity = player.rigidbody.linearVelocity;
        velocity.x = moveSpeed;

        if(player.IsActionActive && player.CanJump && velocity.y <= 0.01f)
        {
            velocity.y = 0f;
            player.rigidbody.linearVelocity = velocity;
            player.rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            velocity = player.rigidbody.linearVelocity;

            player.ConsumeJump();
        }
        
        velocity.y = Mathf.Clamp(velocity.y, -maxFallSpeed, float.MaxValue);
        player.rigidbody.linearVelocity = velocity;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PortalRocket"))
        {
            player.Transit(player.rocketState);
        }
    }
}
