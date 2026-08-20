using System;
using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float spikeDamage;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Vector2 knockbackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockbackAbility knockbackAbility = collision.GetComponentInParent<KnockbackAbility>();

        PlayerStats stats = collision.GetComponent<PlayerStats>();

        knockbackAbility.StartKnockback(knockbackDuration, knockbackForce, transform);

        stats.DamagePlayer(spikeDamage);
    }
}
