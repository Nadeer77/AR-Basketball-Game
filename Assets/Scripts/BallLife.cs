using UnityEngine;

public class BallLife : MonoBehaviour
{
    private BallFlickThrow throwManager;

    private bool canCheckCollision = false;
    void Update()
    {
        // 💥 Safety check: if ball falls too much
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
            GameManager.Instance.AddScore(); // ✅ FIXED
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
        gameObject.SetActive(false);
        throwManager.AllowNextBall();
    }

    void EndFail()
    {
        gameObject.SetActive(false);
        throwManager.AllowNextBall();
        GameManager.Instance.GameOver(); // ✅ FIXED
    }
}