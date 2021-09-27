using UnityEngine;
using System;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidType
    {
        LargeAsteroid,
        MediumAsteroid,
        SmallAsteroid
    }

    [SerializeField] private AsteroidType type;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    private float angle = 45;

    private Rigidbody2D rb;

    public static event Action OnAsteroidCreate;
    public static event Action<AsteroidType> OnAsteroidDestroy;
    
    public void Init()
    {
        Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        SetVelocity(direction.normalized * GetRandomSpeed());
    }

    private void SetVelocity(Vector2 velocity)
    {
        OnAsteroidCreate?.Invoke();
        GetComponent<Rigidbody2D>().velocity = velocity;  
    }

    private void Split(AsteroidType type)
    {
        float randomSpeed = GetRandomSpeed();

        for (int i = -1; i <= 1; i+=2)
        {
            GameObject gameObject = ObjectPooler.instance.Get(type.ToString());
            gameObject.transform.position = transform.position;
            var asteroid = gameObject.GetComponent<Asteroid>();
            Vector2 currentVelocity = GetComponent<Rigidbody2D>().velocity.normalized;
            float a = (currentVelocity.x * Mathf.Cos(angle * Mathf.Deg2Rad) + i * currentVelocity.y * Mathf.Sin(angle * Mathf.Deg2Rad))  * randomSpeed;
            float b = (-i * currentVelocity.x * Mathf.Sin(angle * Mathf.Deg2Rad) + currentVelocity.y * Mathf.Cos(angle * Mathf.Deg2Rad)) * randomSpeed;
            Vector2 childVelocity = new Vector2(a, b);
            asteroid.SetVelocity(childVelocity);
        }
    }

    private float GetRandomSpeed()
    {
        return UnityEngine.Random.Range(minSpeed, maxSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Bullet"))
        {

            if (type == AsteroidType.LargeAsteroid)
            {
                Split(AsteroidType.MediumAsteroid);
            }
            if (type == AsteroidType.MediumAsteroid)
            {
                Split(AsteroidType.SmallAsteroid);
            }

            OnAsteroidDestroy?.Invoke(type);
            ObjectPooler.instance.Return(gameObject, type.ToString());
            SoundManager.instance.PlayExplosionSound();
        }   

        if (other.CompareTag("Player") || other.CompareTag("UFO"))
        {
            OnAsteroidDestroy?.Invoke(type);
            ObjectPooler.instance.Return(gameObject, type.ToString());
        } 
    }
}
