using UnityEngine;

public class BallFlickThrow : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameManager gameManager;
    public Transform hoopTarget;
    public GameObject scoreEffectPrefab;

    private Vector2 startTouch;
    private Vector2 endTouch;

    private bool isDragging = false;
    private bool canThrow = true;

    private GameObject currentBall;
    private Rigidbody currentRB;

    void Update()
    {
        if (!canThrow) return;

        // start swipe
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

        // 🎯 GET FROM POOL
        currentBall = BallPool.Instance.GetBall();

        // 🎯 POSITION IT
        currentBall.transform.position = spawnPos;
        currentBall.transform.rotation = Quaternion.identity;

        currentRB = currentBall.GetComponent<Rigidbody>();

        // 🔥 RESET PHYSICS (VERY IMPORTANT)
        currentRB.linearVelocity = Vector3.zero;
        currentRB.angularVelocity = Vector3.zero;

        currentRB.useGravity = false;
        currentRB.isKinematic = true;
    }

    void ThrowBall()
    {
        if (currentBall == null || hoopTarget == null) return;

        Vector2 swipe = endTouch - startTouch;

        float swipePower = Mathf.Clamp(swipe.magnitude / 300f, 0.5f, 2f);

        // 🎯 Target position (slightly above rim for arc)
        Vector3 targetPos = hoopTarget.position + Vector3.up * 0.3f;

        // 🎯 Direction toward hoop
        Vector3 direction = (targetPos - currentBall.transform.position).normalized;

        // 🏀 Add arc (VERY IMPORTANT)
        direction += Vector3.up * 0.7f;

        // Enable physics
        currentRB.isKinematic = false;
        currentRB.useGravity = true;

        // 🚀 Apply force
        currentRB.AddForce(direction * swipePower * 8f, ForceMode.Impulse);
        currentRB.AddTorque(Random.insideUnitSphere * 3f);

        canThrow = false;

        currentBall.tag = "Ball";
        BallLife bl = currentBall.GetComponent<BallLife>();
        bl.scoreEffectPrefab = scoreEffectPrefab;
        bl.Init(this, gameManager);


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