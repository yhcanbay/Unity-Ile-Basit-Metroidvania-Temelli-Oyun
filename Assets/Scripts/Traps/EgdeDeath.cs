using UnityEngine;

public class EgdeDeath : MonoBehaviour
{
    [SerializeField] private int damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats stats = collision.gameObject.GetComponent<PlayerStats>();

        stats.DamagePlayer((float) damage);
    }
}
