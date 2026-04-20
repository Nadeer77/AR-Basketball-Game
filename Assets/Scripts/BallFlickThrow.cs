using UnityEngine;

public class BallFlickThrow : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameManager gameManager;

    private Vector2 startTouch;
    private Vector2 endTouch;

    private bool isDragging = false;
    private bool canThrow = true;

    private GameObject currentBall;
    private Rigidbody currentRB;

    void Update()
    {
        if (!canThrow) return;

        // TAP → spawn ball
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;

            SpawnBall();

            isDragging = true;
        }

        // RELEASE → throw
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            endTouch = Input.mousePosition;

            ThrowBall();

            isDragging = false;
        }
    }

    void SpawnBall()
    {
        if (currentBall != null) return;

        Vector3 spawnPos =
            transform.position +
            transform.forward * 0.8f +
            Vector3.up * 0.3f;

        currentBall = Instantiate(ballPrefab, spawnPos, Quaternion.identity);

        currentRB = currentBall.GetComponent<Rigidbody>();

        // Disable physics initially
        currentRB.useGravity = false;
        currentRB.isKinematic = true;
    }

    void ThrowBall()
    {
        if (currentBall == null) return;

        Vector2 swipe = endTouch - startTouch;

        // Enable physics
        currentRB.isKinematic = false;
        currentRB.useGravity = true;

        Vector3 direction =
            transform.forward * 8f +
            Vector3.up * 5f +
            transform.right * (swipe.x * 0.02f);

        currentRB.AddForce(direction, ForceMode.Impulse);
        currentRB.AddTorque(Random.insideUnitSphere * 5f);

        canThrow = false;

        currentBall.tag = "Ball";

        currentBall.AddComponent<BallLife>().Init(this, gameManager);

        currentBall = null;
        currentRB = null;
    }
    public void ResetThrowState()
    {
        canThrow = true;
        isDragging = false;

        if (currentBall != null)
        {
            Destroy(currentBall);
            currentBall = null;
            currentRB = null;
        }
    }

    public void AllowNextBall()
    {
        canThrow = true;
    }
}