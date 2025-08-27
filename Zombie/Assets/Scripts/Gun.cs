using Mono.Cecil;
using System.Collections;
using System.Drawing.Text;
using UnityEngine;
using static UnityEngine.PlayerLoop.EarlyUpdate;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading,
    }

    private State currentState = State.Ready;
    public State CurrentState
    {

        get { return currentState; }
        private set
        {
            currentState = value;
            switch(currentState)
            {
                case State.Ready:
                    break;
                case State.Empty:
                    break;
                case State.Reloading:
                    break;
            }

        }
    }

    private void UpdateReady()
    {

    }
    private void UpdateEmpty()
    {

    }
    private void UpdateReloading()
    {

    }

    public GunData gunData;

    private LineRenderer lineRenderer;

    public ParticleSystem muzzleEffect;
    public ParticleSystem shellEffect;

    private AudioSource audioSource;

    public Transform firePosition;



    public int ammoRemain = 100;
    public int magAmmo = 0;
    private float lastFireTime;
 

    private void Update()
    {

        switch (currentState)
        {
            case State.Ready:
                UpdateReady();
                break;
            case State.Empty:
                UpdateEmpty();
                break;
            case State.Reloading:
                UpdateReloading();
                break;
        }
        
    }


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        lastFireTime = 0;

        CurrentState = State.Ready;
    }

    private IEnumerator CoShotEffect(Vector3 hitPosition)
    {
        audioSource.PlayOneShot(gunData.shootClip);

        muzzleEffect.Play();
        shellEffect.Play();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePosition.position);
        lineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(1f);
        lineRenderer.enabled = false;
    }


    public void Fire()
    {
        if(currentState == State.Ready &&  Time.time > (lastFireTime + gunData.timeBetFire))
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }
    private void Shoot()
    {
        Vector3 hitPosition = Vector3.zero;

        RaycastHit hit;
        if(Physics.Raycast(firePosition.position, firePosition.forward, out hit, gunData.fireDistance))
        {
            hitPosition = hit.point;

            var target = hit.collider.GetComponent<IDamagable>();
            if(target != null)
            {
                target.Ondamage(gunData.damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = firePosition.position + firePosition.forward * gunData.fireDistance;
        }

        StartCoroutine(CoShotEffect(hitPosition));
        magAmmo--;
        if(magAmmo == 0)
        {
            currentState = State.Empty;
        }

    }

    public bool Reload()
    {
     if(CurrentState == State.Reloading || ammoRemain == 0 || magAmmo == gunData.magCapacity)
        {
            return false;
        }
        StartCoroutine(CoReload());
        return true;
    }

    private IEnumerator CoReload()
    {
        currentState = State.Reloading;
        audioSource.PlayOneShot(gunData.reloadClip);

        yield return new WaitForSeconds(gunData.reloadTime);

        magAmmo += ammoRemain;

        if(magAmmo >= gunData.magCapacity)
        {
            magAmmo = gunData.magCapacity;
            ammoRemain -= magAmmo;
        }
        else
        {
            ammoRemain = 0;
        }
        currentState = State.Ready;
    }


    public void AddAmmo(int amount)
    {
        ammoRemain = Mathf.Min(ammoRemain + amount, gunData.startAmmoRemain);
    }
}

