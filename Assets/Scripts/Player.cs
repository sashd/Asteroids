using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float boost;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int bulletsPerSecond;
    [SerializeField] private float safetyTime = 3f;
    public static event Action OnPlayerDead;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float elapsedTimeShooting = 0f;
    private float elapsedTimeFlickering = 0f;
    private int shotCount = 0;
    private bool invulnerable = false;
    

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (rb is null || sr is null)
        {
            Debug.LogError("Null");
            return;
        }

        StartCoroutine(Respawn());
    }

    private void Update() 
    {
        float vx = Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);
        float vy = Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed);
        rb.velocity = new Vector2(vx, vy);

        elapsedTimeShooting += Time.deltaTime;
        if (elapsedTimeShooting > 1f)
        {
            shotCount = 0;
            elapsedTimeShooting = 0f;
        }

        if (invulnerable)
        {
            elapsedTimeFlickering += Time.deltaTime;
            if (elapsedTimeFlickering > 0.5f)
            {
                sr.color = sr.color.a == 1f? new Color(sr.color.r, sr.color.g, sr.color.b, 0) : new Color(sr.color.r, sr.color.g, sr.color.b, 1);
                elapsedTimeFlickering = 0f;
            }
        }
    }

    public void Thrust()
    {
        Vector2 force = transform.up * boost * Time.deltaTime;
        rb.AddForce(force);

        SoundManager.instance.PlayThrustSound();
    }

    public void StopThrust()
    {
        SoundManager.instance.StopThrustSound();
    }

    public void Rotate(float axis)
    {
        transform.Rotate(Vector3.forward * axis * rotationSpeed * 50f * Time.deltaTime);
    }

    public void FollowMouse(float angle)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation =  Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    public void Shot()
    {   
        if (shotCount >= bulletsPerSecond)
            return;

        GameObject bulletObject = ObjectPooler.instance.Get("Bullet");
        bulletObject.transform.position = transform.position;
        var bullet = bulletObject.GetComponent<Bullet>();
        Vector2 velocity = transform.up * bulletSpeed;
        bullet.Init(velocity);
        shotCount++;

        SoundManager.instance.PlayFireSound(1f);
    }
    private IEnumerator Respawn()
    {
        transform.position = Vector3.zero;
        invulnerable = true;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(safetyTime);
        invulnerable = false;
        sr.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid") || other.CompareTag("UFOBullet") || other.CompareTag("UFO"))
        {
            if (invulnerable)
                return;
            OnPlayerDead?.Invoke();
            StartCoroutine(Respawn());

            SoundManager.instance.PlayExplosionSound();
        }
    }
}
