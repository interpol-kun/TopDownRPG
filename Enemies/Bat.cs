using Godot;
using System.Collections.Generic;

public class Bat : KinematicBody2D
{
    [Export]
    PackedScene deathEffect;
    Vector2 knockback = Vector2.Zero;

    Vector2 velocity = Vector2.Zero;
    [Export]
    float maxSpeed = 50;
    [Export]
    float acceleration = 100;

    Stats stats = null;

    PlayerDetection playerZone = null;
    AnimatedSprite batSprite = null;

    SoftCollision softCollision = null;

    RandomNumberGenerator rng = null;

    WanderController wanderController = null;

    AnimationPlayer animPlayer = null;

    enum AIState
    {
        IDLE,
        WANDER,
        CHASE
    }

    AIState state = AIState.IDLE;

    Hurtbox hurtbox = null;

    List<AIState> stateArray;
    public override void _Ready()
    {
        GD.Randomize();

        stats = GetNode<Stats>("Stats");
        stats.OnZeroHealth += Death;

        hurtbox = GetNode<Hurtbox>("Hurtbox");
        hurtbox.OnInvincibilityEnded += BlinkStop;
        hurtbox.OnInvincibilityStarted += BlinkStart;

        playerZone = GetNode<PlayerDetection>("PlayerDetection");
        batSprite = GetNode<AnimatedSprite>("AnimatedSprite");

        softCollision = GetNode<SoftCollision>("SoftCollision");

        wanderController = GetNode<WanderController>("WanderController");

        rng = new RandomNumberGenerator();

        stateArray = new List<AIState> { AIState.IDLE, AIState.WANDER };

        state = PickRandomState(stateArray);

        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public override void _PhysicsProcess(float delta)
    {
        knockback = knockback.MoveToward(Vector2.Zero, 200 * delta);
        knockback = MoveAndSlide(knockback);

        Vector2 dir = Vector2.Zero;
        switch (state)
        {
            case AIState.IDLE:
                velocity = velocity.MoveToward(Vector2.Zero, 200 * delta);
                SeekPlayer();
                RestartWander();
                break;
            case AIState.WANDER:
                SeekPlayer();
                RestartWander();
                dir = GlobalPosition.DirectionTo(wanderController.TargetPosition);
                velocity = velocity.MoveToward(dir * maxSpeed, acceleration * delta);
                batSprite.FlipH = velocity.x < 0;
                if (GlobalPosition.DistanceTo(wanderController.TargetPosition) <= maxSpeed * delta)
                {
                    state = PickRandomState(stateArray);
                    wanderController.StartWanderTimer(rng.RandfRange(1, 3));
                }
                break;
            case AIState.CHASE:
                var player = playerZone.Player;
                if (player != null)
                {
                    dir = GlobalPosition.DirectionTo(player.GlobalPosition);
                    velocity = velocity.MoveToward(dir * maxSpeed, acceleration * delta);
                }
                else
                {
                    state = AIState.IDLE;
                }
                batSprite.FlipH = velocity.x < 0;
                break;
            default:
                break;
        }
        if (softCollision.isColliding())
        {
            velocity += softCollision.GetPushVector() * delta * 300;
        }
        velocity = MoveAndSlide(velocity);
    }

    private void RestartWander()
    {
        if (wanderController.GetTimeLeft() == 0)
        {
            state = PickRandomState(stateArray);
            wanderController.StartWanderTimer(rng.RandfRange(1, 3));
        }
    }

    public void _on_Hurtbox_area_entered(Area2D area)
    {
        var hitbox = (Hitbox)area;
        stats.Health -= hitbox.Damage;

        hurtbox.CreateHitEffect();

        var knockbackVector = (hurtbox.GlobalPosition - area.GetParent<Node2D>().GlobalPosition).Normalized();
        knockback = knockbackVector * 120;

        hurtbox.StartInvincibility(0.3f);
    }

    AIState PickRandomState(List<AIState> states)
    {
        return states[rng.RandiRange(0, states.Count - 1)];
    }

    private void SeekPlayer()
    {
        if (playerZone.CanSeePlayer())
        {
            state = AIState.CHASE;
        }
    }

    void Death()
    {
        CreateDeathEffect();
        QueueFree();
    }

    private void CreateDeathEffect()
    {
        var deathEffectInstance = deathEffect.Instance() as Node2D;
        GetParent().AddChild(deathEffectInstance);
        deathEffectInstance.Position = this.Position;
    }

    void BlinkStart()
    {
        animPlayer.Play("Start");
    }

    void BlinkStop()
    {
        animPlayer.Play("Stop");
    }
}
