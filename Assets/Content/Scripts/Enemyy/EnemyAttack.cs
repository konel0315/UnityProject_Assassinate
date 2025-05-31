using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackCooldown = 1.5f;
    
    public AudioClip enemy_attack_Sound;
    
    
    private float attackTimer = 0f;
    private bool isPlayerVisible = false;
    
    private AudioSource audioSource;
    public PlayerHealth playerHealth;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (isPlayerVisible && attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    void Attack()
    {
        audioSource.PlayOneShot(enemy_attack_Sound);
        playerHealth.TakeDamage(1);
    }

    public void SetPlayerVisible(bool visible)
    {
        isPlayerVisible = visible;
    }
}

