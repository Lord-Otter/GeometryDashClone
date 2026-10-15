using UnityEngine;

[System.Serializable]
public class RocketPlayerState : PlayerState
{
     public float moveSpeed = 6f;
     public float gravityScale = 1.5f;
     public float maxFallSpeed = 7f;
     public Sprite rocketSprite;

    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
        if (maxFallSpeed < 0f)maxFallSpeed = 0f;
        if (gravityScale < 0f)gravityScale = Mathf.Abs(gravityScale);
    }

    public override void Enter()
    {
        if(player.spriteRenderer != null && rocketSprite != null)
        {
            player.spriteRenderer.sprite = rocketSprite;
        }

        player.rigidbody.gravityScale = gravityScale;
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        player.rigidbody.gravityScale = player.IsActionActive ? -gravityScale : gravityScale;

        Vector2 velocity = player.rigidbody.linearVelocity;
        velocity.x = moveSpeed;
        velocity.y = Mathf.Clamp(velocity.y, -maxFallSpeed, maxFallSpeed);
        player.rigidbody.linearVelocity = velocity;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PortalCube"))
        {
            player.Transit(player.cubeState);
        }
    }
}