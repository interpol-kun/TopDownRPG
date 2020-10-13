using Godot;
using System;

public class Stats : Node
{
    [Export(PropertyHint.Range, "0,1000")]
    int maxHealth;
    [Export]
    int health;
    public delegate void OnZeroHealthEvent();
    public delegate void OnHealthChangedEvent(int health);
    //For owner tracking
    public event OnZeroHealthEvent OnZeroHealth;

    public event OnHealthChangedEvent OnHealthChanged;

    public event OnHealthChangedEvent OnMaxHealthChanged;
    //For player tracking
    public static event OnZeroHealthEvent OnEnemyDeath;



    public int Health
    {
        get => health;
        set
        {
            health = value;
            OnHealthChanged?.Invoke(health);
            if (health <= 0)
            {
                OnZeroHealth?.Invoke();
                OnEnemyDeath?.Invoke();
            }
        }
    }

    public int MaxHealth
    {
        get => maxHealth; set
        {
            maxHealth = value;
            Health = Mathf.Min(Health, maxHealth);
            OnMaxHealthChanged?.Invoke(health);
        }
    }

    public override void _Ready()
    {
        Health = MaxHealth;
    }
}
