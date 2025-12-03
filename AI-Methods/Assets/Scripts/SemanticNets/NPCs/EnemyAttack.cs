using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;

    private Transform player;
    private PlayerHealth playerHealth;
    private float lastAttackTime = 0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            DealDamage();
        }
    }

    void DealDamage()
    {
        if (playerHealth != null)
        {
            Debug.Log("Enemy hit the player!");
            playerHealth.TakeDamage(attackDamage);
        }
    }
}
