using Godot;
using System;

public class WanderController : Node2D
{
    [Export]
    int wanderRange = 32;
    Vector2 startPosition;
    Vector2 targetPosition;

    Timer timer = null;

    public Vector2 TargetPosition { get => targetPosition; set => targetPosition = value; }

    public override void _Ready()
    {
        startPosition = GlobalPosition;
        TargetPosition = GlobalPosition;
        timer = GetNode<Timer>("Timer");
        UpdateTargetPosition();
    }

    void UpdateTargetPosition()
    {
        var targetVector = new Vector2((float)GD.RandRange(-wanderRange, wanderRange), (float)GD.RandRange(-wanderRange, -wanderRange));
        TargetPosition = startPosition + targetVector;
    }

    public float GetTimeLeft()
    {
        return timer.TimeLeft;
    }

    public void StartWanderTimer(float duration)
    {
        timer.Start(duration);
    }
    public void _on_Timer_timeout()
    {
        UpdateTargetPosition();
    }
}
