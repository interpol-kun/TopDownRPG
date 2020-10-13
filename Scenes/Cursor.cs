using Godot;
using System;

public class Cursor : Sprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseMode.Hidden);
    }
    public override void _Process(float delta)
    {
        Position = GetViewport().GetMousePosition();
    }
}
