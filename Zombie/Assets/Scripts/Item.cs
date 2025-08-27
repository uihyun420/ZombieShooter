using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    public static readonly string IdPlayerShooter = "PlayerShooter";
    public enum Types
    {
        Coin,
        Ammo,
        Health,
    }

    public Types itemType;
    public int value = 10;
    public int totalCoin = 0;
    private const int coinValue = 10;

    public void Use(GameObject other)
    {
        if (other.CompareTag(IdPlayerShooter))
        {
            switch (itemType)
            {
                case Types.Coin:
                    {
                        totalCoin += coinValue;
                        Debug.Log("Coin");
                    }
                    break;
                case Types.Ammo:
                    {
                        var shooter = other.GetComponent<PlayerShooter>();

                        shooter?.gun?.AddAmmo(value);

                    }
                    break;
                case Types.Health:
                    {
                        var playerHealth = other.GetComponent<PlayerHealth>();
                        playerHealth?.Heal(value);
                    }
                    break;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(IdPlayerShooter))
        {
            Use(other.gameObject);
            Destroy(gameObject);
        }
    }
}
