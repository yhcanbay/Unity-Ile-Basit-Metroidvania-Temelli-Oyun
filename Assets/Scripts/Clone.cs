using UnityEngine;

public class Clone : MonoBehaviour, IDamagable
{
    [SerializeField] private float health = 100;
    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
