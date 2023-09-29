using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
The HealthSystem class in Unity represents an essential component 
for managing the health of game objects. It provides a flexible framework for tracking an object's health,
allowing it to take damage, receive healing, and trigger events upon damage and death
*/
public class HealthSystem : MonoBehaviour
{
    // Events to notify other scripts/components about health changes and death.
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    // The current health of the object.
    [SerializeField] public int health = 100;

    // Private variable to store the maximum health.
    private int healthMax;

    private void Awake()
    {
        // Initialize healthMax to the initial health value.
        healthMax = health;
    }

    // Inflict damage to the object.
    public void Damage(int damageAmount)
    {
        // Reduce health by the specified damageAmount.
        health -= damageAmount;

        // Ensure health doesn't go below 0.
        if (health < 0)
        {
            health = 0;
        }

        // Trigger the OnDamaged event to notify listeners.
        OnDamaged?.Invoke(this, EventArgs.Empty);

        // Check if health has reached 0, and call the Die method if so.
        if (health == 0)
        {
            Die();
        }
    }

    // Heal the object.
    public void Heal(int healAmount)
    {
        // Check if health is less than the maximum health.
        if (health < healthMax)
        {
            // Increase health by the specified healAmount.
            health += healAmount;

            // Trigger the OnDamaged event to notify listeners.
            OnDamaged?.Invoke(this, EventArgs.Empty);
        }
    }

    // Handle the death of the object.
    private void Die()
    {
        // Trigger the OnDead event to notify listeners.
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    // Get the current health as a normalized value between 0 and 1.
    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
