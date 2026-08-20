using UnityEngine;

public class SwingBlade : MonoBehaviour
{

    [Header("Swing-Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float maxAngle;
    private float timer;
    private float previousAngle;
    private int pushDirection;

    [Header("Damage-Settings")]
    [SerializeField] private float bladeDamage;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Vector2 knockbackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockbackAbility knockbackAbility = collision.GetComponentInParent<KnockbackAbility>();

        PlayerStats stats = collision.GetComponent<PlayerStats>();

        knockbackAbility.StartKnockback(knockbackDuration, knockbackForce, transform,pushDirection);

        stats.DamagePlayer(bladeDamage);
    }

    void Update()
    {
        timer += speed * Time.deltaTime;
        float angle = maxAngle * Mathf.Sin(timer);
        transform.localRotation = Quaternion.Euler(0, 0,angle);

        if (previousAngle < angle)
            pushDirection = 1;
        else pushDirection = -1;

        previousAngle = angle;
    }
}
