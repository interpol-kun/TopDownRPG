using Godot;
using System;

public class SoftCollision : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public bool isColliding()
    {
        var areas = GetOverlappingAreas();
        return areas.Count > 0;
    }

    public Vector2 GetPushVector()
    {
        var areas = GetOverlappingAreas();
        var pushVector = Vector2.Zero;
        if (areas.Count > 0)
        {
            Area2D area = areas[0] as Area2D;
            pushVector = area.GlobalPosition.DirectionTo(GlobalPosition);
        }
        return pushVector;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
