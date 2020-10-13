using Godot;
using System;

public class Effect : AnimatedSprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("animation_finished", this, nameof(_on_Effect_animation_finished));
        this.Frame = 0;
        this.Play("Animate");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void _on_Effect_animation_finished()
    {
        QueueFree();
    }
}
