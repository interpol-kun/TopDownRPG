using Godot;

public class Player : KinematicBody2D
{
    Vector2 velocity = Vector2.Zero;
    Vector2 rollVector = Vector2.Left;

    [Export]
    float maxSpeed = 80;
    [Export]
    float rollSpeed = 125;

    AnimationPlayer animPlayer = null;
    AnimationTree animTree = null;

    AnimationNodeStateMachinePlayback animState = null;

    PlayerHurtSound hurtSound = null;

    Hurtbox hurtbox = null;

    AnimationPlayer blinkAnimationPlayer = null;

    Stats playerStats;

    enum AnimationState
    {
        MOVE,
        ROLL,
        ATTACK
    }

    AnimationState state = AnimationState.MOVE;

    Vector2 lookDirection;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        playerStats = GetNode<Stats>("/root/PlayerStats");
        playerStats.OnZeroHealth += Die;

        hurtbox = GetNode<Hurtbox>("Hurtbox");
        hurtbox.OnInvincibilityStarted += BlinkStart;
        hurtbox.OnInvincibilityEnded += BlinkStop;

        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animTree = GetNode<AnimationTree>("AnimationTree");
        animState = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
        animTree.Active = true;

        blinkAnimationPlayer = GetNode<AnimationPlayer>("BlinkAnimationPlayer");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        lookDirection = GlobalPosition.DirectionTo(GetGlobalMousePosition());
        switch (state)
        {
            case AnimationState.MOVE:
                MoveState(delta);
                break;
            case AnimationState.ATTACK:
                AttackState(delta);
                break;
            case AnimationState.ROLL:
                RollState(delta);
                break;
            default:
                break;
        }
    }

    void MoveState(float delta)
    {
        Vector2 input_vector = Vector2.Zero;

        input_vector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        input_vector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        input_vector = input_vector.Normalized();

        if (input_vector != Vector2.Zero)
        {
            rollVector = input_vector;
            animTree.Set("parameters/Idle/blend_position", lookDirection);
            animTree.Set("parameters/Run/blend_position", lookDirection);
            animTree.Set("parameters/Attack/blend_position", lookDirection);
            animTree.Set("parameters/Roll/blend_position", lookDirection);
            animState.Travel("Run");
            velocity = input_vector * maxSpeed;
        }
        else
        {
            rollVector = lookDirection;
            animState.Travel("Idle");
            animTree.Set("parameters/Idle/blend_position", lookDirection);
            animTree.Set("parameters/Attack/blend_position", lookDirection);
            animTree.Set("parameters/Roll/blend_position", lookDirection);
            velocity = Vector2.Zero;
        }

        Move();

        if (Input.IsActionJustPressed("attack"))
        {
            state = AnimationState.ATTACK;
        }
        if (Input.IsActionJustPressed("roll"))
        {
            state = AnimationState.ROLL;
        }
    }

    void AttackState(float delta)
    {
        velocity = Vector2.Zero;
        animState.Travel("Attack");
    }

    void Move()
    {
        velocity = MoveAndSlide(velocity);
    }
    void RollState(float delta)
    {
        velocity = rollVector * rollSpeed;
        animState.Travel("Roll");
        Move();
    }

    void AttackAnimationFinished()
    {
        state = AnimationState.MOVE;
    }

    void RollAnimationFinished()
    {
        state = AnimationState.MOVE;
    }
    public void _on_Hurtbox_area_entered(Area2D area)
    {
        playerStats.Health -= (area as Hitbox).Damage;
        hurtbox.StartInvincibility(0.6f);
        hurtbox.CreateHitEffect();
        hurtSound = (PlayerHurtSound)(ResourceLoader.Load("res://Player/PlayerHurtSound.tscn") as PackedScene).Instance(); //ugh ugly
        GetTree().CurrentScene.AddChild(hurtSound);
    }

    void Die()
    {
        QueueFree();
    }

    void BlinkStart()
    {
        blinkAnimationPlayer.Play("Start");
    }

    void BlinkStop()
    {
        blinkAnimationPlayer.Play("Stop");
    }
}
