using Godot;
using System;

public class PlayerDetection : Area2D
{
    private KinematicBody2D player;

    public KinematicBody2D Player { get => player; set => player = value; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {

    }
    public void _on_PlayerDetection_body_entered(KinematicBody2D body)
    {
        Player = body;
    }
    public void _on_PlayerDetection_body_exited(KinematicBody2D body)
    {
        Player = null;
    }
    public bool CanSeePlayer()
    {
        return Player != null;
    }
}
