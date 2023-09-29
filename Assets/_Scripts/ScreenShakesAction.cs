using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ScreenShakesAction : MonoBehaviour
{
    private void Start()
    {
        // Subscribe to various events to trigger screen shakes.
        ExplosionCine.OnExplosion += ExplosionCine_OnExplosion;
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;

        // Find the "DoorScene" GameObject to determine if "Howitzer" is active.
        GameObject door = GameObject.Find("DoorScene");
        if (door.GetComponent<ChangeScene>().isHowizer == true)
        {
            // Start the Howitzer sound and screen shake coroutine.
            StartCoroutine(HowitzerSound());
        }
    }

    private void ExplosionCine_OnExplosion()
    {
        // Trigger a screen shake when an explosion occurs.
        ScreenShake.Instance.Shake(7f);
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        // Trigger a screen shake when a grenade explodes.
        ScreenShake.Instance.Shake(7f);
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        // Trigger a screen shake when shooting.
        ScreenShake.Instance.Shake(5f);
    }

    IEnumerator HowitzerSound()
    {
        yield return new WaitForSeconds(2);
        int seconds = Random.Range(5, 8);

        // Instantiate explosion audio and destroy it after a delay.
        GameObject explosion1 = Instantiate(Resources.Load("Explosion1AudioVariant", typeof(GameObject))) as GameObject;
        Destroy(explosion1, 6);

        // Trigger a screen shake.
        ScreenShake.Instance.Shake(6);

        yield return new WaitForSeconds(seconds);

        // Instantiate another explosion audio and destroy it after a delay.
        GameObject explosion2 = Instantiate(Resources.Load("Explosion1AudioVariant", typeof(GameObject))) as GameObject;
        Destroy(explosion2, 6);

        // Trigger a screen shake.
        ScreenShake.Instance.Shake(5);

        // Restart the Howitzer sound and screen shake coroutine.
        StartCoroutine(HowitzerSound());
    }
}
