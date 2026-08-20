using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float maxHealth;
    [SerializeField] private HealthBarControl healthBarControl;
    float currentHealth;

    [Header("Colliders")]
    [SerializeField] private Collider2D standingStatCollider;
    [SerializeField] private Collider2D crouchStatCollider;
    private Collider2D currentStatCollider;


    [Header("Flash")]
    [SerializeField] private float flashDuration;
    [SerializeField, Range(0f, 1f)] private float flashStrength;
    [SerializeField] private Color flashColor;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    private SpriteRenderer spriter;
    private bool canTakeDamage=true;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarControl.setSliderValue(currentHealth, maxHealth);
        spriter = GetComponentInParent<SpriteRenderer>();
        defaultMaterial = spriter.material;
    }

    public void DamagePlayer(float damage)
    {
        if (!canTakeDamage)
            return;
        currentHealth -= damage;
        healthBarControl.setSliderValue(currentHealth, maxHealth);
        StartCoroutine(Flash());
        if (currentHealth < 0)
        {
            if (player.stateMachine.currentState != PlayerStates.State.Knockback)
                player.stateMachine.ChangeState(PlayerStates.State.Death);
        }
    }

    public float GetHealth() { 
        return currentHealth;
    }

    public bool GetCanTakeDamage()
    {
        return canTakeDamage;
    }

    public void setCanTakeDamage(bool canTakeDamage)
    {
        this.canTakeDamage = canTakeDamage;
    }

    public IEnumerator Flash()
    {
        canTakeDamage = false;
        spriter.material = flashMaterial;
        flashMaterial.SetColor("_FlashColor", flashColor);
        flashMaterial.SetFloat("_FlashAmount", flashStrength);
        yield return new WaitForSeconds(flashDuration);
        spriter.material = defaultMaterial;
        if(currentHealth>0)
            canTakeDamage=true;
    }

    public void EnableStandingStatCollider()
    {
        if (currentHealth < 0)
            return;
        crouchStatCollider.enabled = false;
        standingStatCollider.enabled = true;
        currentStatCollider = standingStatCollider;
    }

    public void EnableCrounchStatCollider()
    {
        if (currentHealth < 0)
            return;
        standingStatCollider.enabled=false;
        crouchStatCollider.enabled=true;
        currentStatCollider = crouchStatCollider;
    }
}
