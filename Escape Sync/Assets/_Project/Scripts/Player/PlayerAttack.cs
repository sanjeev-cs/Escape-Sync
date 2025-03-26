using System;
using UnityEngine;

namespace COMP305
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float attackDelay; // Delay between attacks.
    //[SerializeField] private Transform firePoint; // Position where fireballs are instantiated.
    //[SerializeField] private GameObject[] fireballs; // Pool of fireball objects.
    // private Animator anim; // Reference to the Animator component.
    private PlayerController playerController; // Reference to the PlayerMovement script.
    private float delayTimer = Mathf.Infinity; // Timer to track attack cooldown.

    [Header("Attack Sound")]
    [SerializeField] private AudioClip playerAttackSound;

    [Header("Enemy Parameter")]
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Enemy Layer")]
    [SerializeField] private LayerMask enemyLayer;
    private Health enemyHealth;
    
    private bool isAttacking = false;

    private void OnEnable()
    {
        InteractionEventManager.AttackKeyPressed += Attacking;
    }

    private void OnDisable()
    {
        InteractionEventManager.AttackKeyPressed -= Attacking;
    }

    private void Awake()
    {
        // anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        
    }

    private void Update()
    {
        // Checks if the attack button is pressed, the cooldown has passed, and the player can attack.
        if (isAttacking && delayTimer >= attackDelay)
        {
            Debug.Log("inside update");
            Attack();
            delayTimer = 0f; // Reset the attack cooldown
        }

        delayTimer += Time.deltaTime; // Increment the delay timer
    }

    private void Attack()
    {
        SoundManager.instance.playeSound(playerAttackSound);
        // anim.SetTrigger("attack"); // Trigger the attack animation.
        DamageEnemy();
        Debug.Log("Inside Attack");
        delayTimer = 0; // Reset the attack cooldown.
        isAttacking = false;
    }

    private bool EnemyInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * (range * transform.localScale.x * colliderDistance),
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, enemyLayer);

        Debug.Log($"BoxCast Position: {boxCollider.bounds.center + transform.right * (range * transform.localScale.x * colliderDistance)}");
        Debug.Log($"BoxCast Size: {new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)}");

        if (hit.collider != null)
        {
            Debug.Log($"Hit: {hit.collider.gameObject.name}");
            enemyHealth = hit.transform.GetComponent<Health>();
            if (enemyHealth != null)
            {
                Debug.Log("Enemy health got");
            }
            else
            {
                Debug.Log("Enemy does not have Health component");
            }
            return true;
        }
        else
        {
            Debug.Log("No enemy detected");
        }

        return false;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * (transform.localScale.x) * colliderDistance,
    //         new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    // }

    private void DamageEnemy()
    {
        // If player in range damage it.
        if (EnemyInSight())
        {
            Debug.Log("Damage works");
            enemyHealth.TakeDamage(damage);
        }
    }

    private void Attacking()
    {
        if (delayTimer >= attackDelay) // Only allow attacking if the cooldown has passed
        {
            isAttacking = true;
        }
    }
    
    }
}
