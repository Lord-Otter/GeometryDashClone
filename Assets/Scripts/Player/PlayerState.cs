using UnityEngine;

[System.Serializable]
public class PlayerState
{
    [HideInInspector] public PlayerBehaviour player = null;

    public virtual void OnValidate(PlayerBehaviour player) 
    {
        this.player = player;
    }
    
    public virtual void Awake() {}
    public virtual void Start() {}
    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void FixedUpdate() {}
    public virtual void OnTriggerEnter2D(Collider2D collision) {}
}
