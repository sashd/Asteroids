using UnityEngine;
using System;

public class UFO : MonoBehaviour
{
    [SerializeField] private float overflyTime = 10f;
    [SerializeField] private GameObject ship;
    [SerializeField] private float minAppearTime = 20f;
    [SerializeField] private float maxApperTime = 40f;
    [SerializeField] private float minShotPeriod = 2f;
    [SerializeField] private float maxShotPeriod = 5f;
    [SerializeField] private float bulletSpeed = 0f;
    public static event Action OnUFODestroy;

    private float flyCountdown;
    private float shotCountdown;

    private float speed = 0f;
    private float startPostion;
    private bool moving = false;
    private float direction = 0f;
    private float borderX;

    private void Start()
    {
        borderX = ScreenBounds.Borders.x;
        speed = 2 * borderX / overflyTime; 
        ship.SetActive(false);
        flyCountdown = UnityEngine.Random.Range(minAppearTime, maxApperTime);
    }

    private void Update()
    {
        if (moving)
        {
            ship.transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
            
            shotCountdown -= Time.deltaTime;
            if (shotCountdown < 0)
            {
                Shot();
                shotCountdown = UnityEngine.Random.Range(minShotPeriod, maxShotPeriod); 
            }

            if (Mathf.Abs(ship.transform.position.x) > borderX) 
            {
                Deactivate();
            }
        }

        flyCountdown -= Time.deltaTime;
        if (flyCountdown < 0 && !moving) 
        {
            StartFly();
            moving = true;
        }
    }

    private void StartFly()
    {
        ship.SetActive(true);
        direction = UnityEngine.Random.value < .5f? 1: -1;
        ship.transform.position = new Vector2(-direction * borderX, GetRandomPosition());
        shotCountdown = UnityEngine.Random.Range(minShotPeriod, maxShotPeriod);
    }

    private void Shot()
    {
        var player = FindObjectOfType<Player>();
        if (player == null)
        {
            return;
        }
        GameObject bulletObject = ObjectPooler.instance.Get("UFOBullet");
        bulletObject.transform.position = ship.transform.position;
        var bullet = bulletObject.GetComponent<Bullet>();

        Vector3 direction = player.transform.position - ship.transform.position;
        Vector2 velocity = direction.normalized * bulletSpeed;
        
        bullet.Init(velocity);

        SoundManager.instance.PlayFireSound(0.6f);
    }

    private float GetRandomPosition()
    {
        return UnityEngine.Random.Range(-ScreenBounds.Borders.y * 0.8f,  ScreenBounds.Borders.y * 0.8f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            OnUFODestroy?.Invoke();
            SoundManager.instance.PlayExplosionSound();
            Deactivate();
        }

        if (other.CompareTag("Asteroid") || other.CompareTag("Player")) 
        {
            SoundManager.instance.PlayExplosionSound();
            Deactivate();
        }
    }

    private void Deactivate()
    {
        ship.SetActive(false);
        moving = false;
        flyCountdown = UnityEngine.Random.Range(minAppearTime, maxApperTime);
    }
}
