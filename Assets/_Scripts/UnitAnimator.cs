using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator; // Reference to the animator component.
    [SerializeField] private Transform bulletProjectilePrefab; // Prefab for bullet projectiles.
    [SerializeField] private Transform shootPoint; // Transform for shooting point.
    [SerializeField] private Transform rifleTransform; // Transform for the rifle.
    [SerializeField] private Transform swordTransform; // Transform for the sword.

    private GrenadeAction grenadeAction; // Reference to the grenade action component.
    private HealthSystem healthSystem; // Reference to the health system component.
    private InteractAction interactAction; // Reference to the interact action component.

    private void Awake()
    {
        // Subscribe to movement events if the object has the MoveAction component.
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        // Subscribe to shoot events if the object has the ShootAction component.
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        // Subscribe to sword action events if the object has the SwordAction component.
        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
        }

        // Get references to health system, grenade action, and interact action components.
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_onDead;
        healthSystem.OnDamaged += HealthSystem_onDamaged;

        grenadeAction = GetComponent<GrenadeAction>();
        grenadeAction.OnThrowGrenade += GrenadeAction_OnThrow;

        interactAction = GetComponent<InteractAction>();
        interactAction.OnInteraction += Interaction_OnInteraction;
    }

    private void Start()
    {
        EquipRifle(); // Start with the rifle equipped.
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle(); // Equip the rifle after completing a sword action.
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("Melee"); // Trigger melee animation.
        GameObject knife = Instantiate(Resources.Load("KnifeAudioVariant", typeof(GameObject))) as GameObject;
        Destroy(knife, 6); // Create and destroy a knife audio variant.
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("Ismoving", true); // Set the "Ismoving" parameter to true when moving.
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("Ismoving", false); // Set the "Ismoving" parameter to false when not moving.
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot"); // Trigger shooting animation.
        GameObject shoot = Instantiate(Resources.Load("ShotAudioVariant", typeof(GameObject))) as GameObject;
        Destroy(shoot, 6); // Create and destroy a shooting audio variant.

        // Create and set up a bullet projectile.
        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPoint.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldposition();
        targetUnitShootAtPosition.y = shootPoint.position.y;

        bulletProjectile.SetUp(targetUnitShootAtPosition);
    }

    private void HealthSystem_onDead(object sender, EventArgs e)
    {
        animator.SetTrigger("Die"); // Trigger death animation.
    }

    private void HealthSystem_onDamaged(object sender, EventArgs e)
    {
        if (healthSystem.health > 0)
        {
            animator.SetTrigger("Damage"); // Trigger damage animation when health is greater than 0.
        }
    }

    private void GrenadeAction_OnThrow(object sender, EventArgs e)
    {
        animator.SetTrigger("Grenade"); // Trigger grenade animation.
    }

    private void Interaction_OnInteraction(object sender, EventArgs e)
    {
        animator.SetTrigger("OpenDoor"); // Trigger door opening animation on interaction.
    }

    private void EquipSword()
    {
        // Activate the sword transform and deactivate the rifle transform.
        //swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }

    private void EquipRifle()
    {
        // Deactivate the sword transform and activate the rifle transform.
        //swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }
}
