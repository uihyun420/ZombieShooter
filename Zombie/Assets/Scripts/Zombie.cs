using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal.Internal;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    public ParticleSystem bloodEffect;
    public Ui ui;

    private NavMeshAgent agent;
    private Animator animator;
    private Collider zombieCollider;
    private AudioSource audioSource;

    public AudioClip zombieDie;
    public AudioClip zombieDamage;

    private Transform target;
    public float traceDistance;
    public float attackDistance;

    public float damage = 10f;
    public float lastAttackTime;
    public float attackInterval = 0.5f;

    private Status currnetStatus;

    public Status CurrnetStatus
    {
        get { return currnetStatus; }
        set
        {
            var preStatus = currnetStatus;
            currnetStatus = value;
            switch (currnetStatus)
            {
                case Status.Idle:
                    animator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    animator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    animator.SetBool("HasTarget", false);

                    agent.isStopped = true;
                    break;
                case Status.Die:
                    animator.SetTrigger("Die");
                    agent.isStopped = true;
                    zombieCollider.enabled = false;

                    break;
            }
        }
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        zombieCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            agent.SetDestination(new Vector3(10f, 0f, 0f));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            agent.SetDestination(new Vector3(0f, 0f, 0f));
        }

        switch (currnetStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                break;
        }


    }

    private void UpdateIdle()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrnetStatus = Status.Trace;
        }

        target = FindTarget(traceDistance);
    }

    private void UpdateTrace()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrnetStatus = Status.Attack;
            return;
        }

        if (target == null  || Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            CurrnetStatus = Status.Idle;
            return;
        }
        agent.SetDestination(target.position);
    }
    private void UpdateAttack()
    {
        if (target == null || (target != null && Vector3.Distance(transform.position, target.position) > attackDistance))
        {
            CurrnetStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        if(lastAttackTime + attackInterval < Time.time )
        {
            lastAttackTime = Time.time;
            var damagable = target.GetComponent<IDamagable>();
            if(damagable != null)
            {
                damagable.Ondamage(damage, transform.position, -transform.forward);


                audioSource.PlayOneShot(zombieDamage);
            }
        }
    }
    private void UpdateDie()
    {

    }


    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Ondamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.Ondamage(damage, hitPoint, hitNormal);
        if (bloodEffect != null)
        {
            bloodEffect.transform.position = hitPoint;
            bloodEffect.transform.forward = hitNormal;
            bloodEffect.Play();
        }
    }

    public override void Die()
    {
        base.Die();
        CurrnetStatus = Status.Die;
        audioSource.PlayOneShot(zombieDie);
        ui.AddScore(10);
    }


    public LayerMask targetLayer;

    protected Transform FindTarget(float radius)
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, targetLayer.value);
        if(colliders.Length ==0)
        {
            return null;
        }

        var target = colliders.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();

        return target.transform;
    }

}

