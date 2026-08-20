using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float bladeDamage;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Vector2 knockbackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockbackAbility knockbackAbility = collision.GetComponentInParent<KnockbackAbility>();

        PlayerStats stats = collision.GetComponent<PlayerStats>();

        knockbackAbility.StartKnockback(knockbackDuration, knockbackForce, transform);

        stats.DamagePlayer(bladeDamage);
    }
    void Update()
    {
        transform.Rotate(0, 0, rotatingSpeed * Time.deltaTime); 
    }
}
