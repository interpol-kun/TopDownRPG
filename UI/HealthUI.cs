using Godot;
using System;

public class HealthUI : Control
{
    TextureRect heartUIFull = null;
    TextureRect heartUIEmpty = null;


    int hearts = 4;
    int maxHearts;

    Stats playerStats = null;

    public int MaxHearts
    {
        get => maxHearts;
        set
        {
            maxHearts = Mathf.Max(value, 1);
            if (heartUIEmpty != null)
            {
                heartUIEmpty.RectSize = new Vector2(MaxHearts * 15f, heartUIEmpty.RectSize.y);
            }
        }
    }
    public int Hearts
    {
        get => hearts;
        set
        {
            hearts = Mathf.Clamp(value, 0, MaxHearts);
            if (heartUIFull != null)
            {
                heartUIFull.RectSize = new Vector2(Hearts * 15f, heartUIFull.RectSize.y);
            }
        }
    }

    public override void _Ready()
    {
        heartUIFull = GetNode<TextureRect>("HeartUIFull");
        heartUIEmpty = GetNode<TextureRect>("HeartUIEmpty");

        playerStats = GetNode<Stats>("/root/PlayerStats");

        playerStats.OnHealthChanged += ChangeHealthUI;
        playerStats.OnMaxHealthChanged += ChangeMaxHealthUI;

        MaxHearts = playerStats.MaxHealth;
        Hearts = playerStats.Health;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    void ChangeHealthUI(int health)
    {
        Hearts = health;
    }

    void ChangeMaxHealthUI(int health)
    {
        MaxHearts = health;
    }
}
