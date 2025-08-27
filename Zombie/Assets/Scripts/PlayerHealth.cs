using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;
    public static readonly int IdDie = Animator.StringToHash("Die");
    public static readonly string IdHeart = "Heart";


    public Ui ui;
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemPickupClip;

    private PlayerMovement movement;
    private PlayerShooter shooter;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        shooter = GetComponent<PlayerShooter>();

    }

    protected override void OnEnable()
    {
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.value = Health / MaxHealth;

        movement.enabled = true;
        shooter.enabled = true;
    }

    public override void Ondamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
        {
            return;
        }

        base.Ondamage(damage, hitPoint, hitNormal);
        healthSlider.value = Health / MaxHealth;
        audioSource.PlayOneShot(hitClip);
    }

    public void Heal(int amount)
    {
        Health = Mathf.Min(Health + (float)amount, MaxHealth);
        healthSlider.value = Health / MaxHealth;
    }
    //public override void Heal(int amount)
    //{
    //    Health += amount;
    //    if (Health >= MaxHealth)
    //    {
    //        Health = MaxHealth;
    //    }
    //}
    public override void Die()
    {
        base.Die();

        healthSlider.gameObject.SetActive(false);
        animator.SetTrigger(IdDie); // ¼öÁ¤
        audioSource.PlayOneShot(deathClip);

        movement.enabled = false;
        shooter.enabled = false;
        ui.OnPlayerDead();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ondamage(10f, Vector3.zero, Vector3.zero);
        }
    }

}
