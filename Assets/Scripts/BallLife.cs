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
        if (scoreEffectPrefab != null && throwManager.hoopTarget != null)
        {
            Transform hoop = throwManager.hoopTarget;

            // 👉 center of hoop
            Vector3 center = hoop.position + Vector3.up * 2f;;

            // 👉 left & right direction
            Vector3 right = hoop.right;

            // 👉 positions
            Vector3 leftPos = center - right * 0.5f;
            Vector3 rightPos = center + right * 0.5f;

            // 💥 spawn effects
            Instantiate(scoreEffectPrefab, leftPos, Quaternion.LookRotation(-right));
            Instantiate(scoreEffectPrefab, rightPos, Quaternion.LookRotation(right));
        }

        GameManager.Instance.AddScore();

        BallPool.Instance.ReturnBall(gameObject);
        throwManager.AllowNextBall();
    }

    void EndFail()
    {
        BallPool.Instance.ReturnBall(gameObject);
        throwManager.AllowNextBall();

        GameManager.Instance.LoseLife();
    }
}