using Godot;
using System;

public class Hitbox : Area2D
{
    [Export]
    private int damage = 1;

    public int Damage { get => damage; set => damage = value; }

    public override void _Ready()
    {

    }
}
