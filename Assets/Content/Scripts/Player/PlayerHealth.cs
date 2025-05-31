using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    
    public int maxHp = 5;
    private int _currentHp;
    private YouDieUI youDieUI;
    [SerializeField] private Image[] hearts;
    
    // Start is called before the first frame update
    void Awake()
    {
        youDieUI = FindObjectOfType<YouDieUI>();
    }

    void Start()
    {
        _currentHp = maxHp;
        UpdateHearts();
    }

    public int currentHealth()
    {
        return _currentHp;
    }

    public void TakeDamage(int amount)
    {
        _currentHp -= amount;
        if (_currentHp < 0) _currentHp = 0;

        UpdateHearts();

        if (_currentHp <= 0)
        {
            Die();
        }
    }

    void UpdateHearts()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < _currentHp;
        }
    }

    void Die()
    {
        youDieUI.ShowYouDieUI();
    }
}