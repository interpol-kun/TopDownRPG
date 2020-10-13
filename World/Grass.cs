using Godot;
using System;

public class Grass : Node2D
{
    [Export]
    PackedScene effect;

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    private void CreateGrassEffect()
    {
        var grassEffect = effect.Instance() as Node2D;
        GetParent().AddChild(grassEffect);
        grassEffect.Position = this.Position;
    }

    public void _on_Hurtbox_area_entered(Area2D area)
    {
        CreateGrassEffect();
        QueueFree();
    }
}
