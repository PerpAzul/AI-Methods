using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public int damage = 15;

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
