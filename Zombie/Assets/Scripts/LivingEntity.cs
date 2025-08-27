using System;
using Unity.Services.Analytics;
using UnityEngine;


public class LivingEntity : MonoBehaviour, IDamagable
{
    public float MaxHealth = 100f;
    public float Health { get; set; }
    public bool IsDead { get; set; }

    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        IsDead = false;
        Health = MaxHealth;
    }
    
    public virtual void Ondamage(float damage, Vector3 hiPoint, Vector3 hitNormal)
    {
        Health -= damage;
        if(Health <= 0 && !IsDead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if(OnDeath != null)
        {
            OnDeath();
        }
       IsDead = true;
    }

    //public virtual void Heal(int amount)
    //{
    //    Health += amount;
    //    if (Health >= MaxHealth)
    //    {
    //        Health = MaxHealth;
    //    }
    //}
}
