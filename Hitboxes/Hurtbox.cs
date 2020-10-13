using Godot;
using System;

public class Hurtbox : Area2D
{
    [Export]
    bool showHit;

    [Export]
    PackedScene hitEffect;

    bool invincible = false;

    public delegate void OnInvincibilityTriggered();
    //For owner tracking
    public event OnInvincibilityTriggered OnInvincibilityStarted;
    //For player tracking
    public event OnInvincibilityTriggered OnInvincibilityEnded;

    CollisionShape2D collisionShape = null;

    Timer timer;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        OnInvincibilityStarted += InvincibilityStart;
        OnInvincibilityEnded += InvincibilityEnd;

        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public bool Invincible
    {
        get => invincible;

        set
        {
            invincible = value;
            if (invincible)
            {
                OnInvincibilityStarted?.Invoke();
            }
            else
            {
                OnInvincibilityEnded?.Invoke();
            }
        }
    }

    public void StartInvincibility(float duration)
    {
        Invincible = true;
        timer.Start(duration);
    }
    public void CreateHitEffect()
    {
        var hitEffectInstance = hitEffect.Instance() as Node2D;
        var main = GetTree().CurrentScene;
        main.AddChild(hitEffectInstance);
        hitEffectInstance.GlobalPosition = GlobalPosition;
    }

    public void _on_Timer_timeout()
    {
        Invincible = false;
    }

    private void InvincibilityStart()
    {
        collisionShape.SetDeferred("disabled", true);
    }
    private void InvincibilityEnd()
    {
        collisionShape.SetDeferred("disabled", false);
    }
}
