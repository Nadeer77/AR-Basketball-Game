using UnityEngine;

public class BallLife : MonoBehaviour
{
    private BallFlickThrow throwManager;
    private bool canCheckCollision = false;

    public GameObject scoreEffectPrefab;

    void Update()
    {
        // safety check (if ball falls too far)
        if (transform.position.y < -2f)
        {
            EndFail();
        }
    }

    public void Init(BallFlickThrow t, GameManager g)
    {
        throwManager = t;

        Invoke("EnableCollision", 0.5f);
    }

    void EnableCollision()
    {
        canCheckCollision = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Score"))
        {
            EndSuccess();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!canCheckCollision) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            EndFail();
        }
    }

    void EndSuccess()
    {
        // 🎉 Spawn effect in front of camera
        if (scoreEffectPrefab != null)
        {
            Transform cam = Camera.main.transform;

            Vector3 spawnPos =
                cam.position +
                cam.forward * 1.0f +
                Vector3.up * 0.2f;

            Instantiate(scoreEffectPrefab, spawnPos, Quaternion.identity);
        }

        GameManager.Instance.AddScore();

        gameObject.SetActive(false);
        throwManager.AllowNextBall();
    }

    void EndFail()
    {
        gameObject.SetActive(false);
        throwManager.AllowNextBall();

        GameManager.Instance.LoseLife();
    }
}