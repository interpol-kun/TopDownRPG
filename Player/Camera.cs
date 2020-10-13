using Godot;
using System;

public class Camera : Camera2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Position2D topLeft = null;
    Position2D bottomRight = null;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        topLeft = GetNode<Position2D>("Limits/TopLeft");
        bottomRight = GetNode<Position2D>("Limits/BottomRight");

        LimitTop = (int)topLeft.Position.y;
        LimitLeft = (int)topLeft.Position.x;
        LimitBottom = (int)bottomRight.Position.y;
        LimitRight = (int)bottomRight.Position.x;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
