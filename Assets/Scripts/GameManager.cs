using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public delegate void OnHealthChangedDelegate(int newHealth);
    public event OnHealthChangedDelegate OnHealthChanged;

    private int health;
    public int Health
    {
        get => health;
        set
        {
            if (health != value)
            {
                health = value;
                OnHealthChanged?.Invoke(health);
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        GameStart();
    }

    private void Start()
    {
    }

    public void IncreaseHealth(int i)
    {
        Health += i;
    }

    public void DecreaseHealth(int i)
    {
        Health -= i;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

    public void GameStart()
    {
        Health = 100;
    }

}
