using UnityEngine;
using System.Collections.Generic;

public class BallPool : MonoBehaviour
{
    public static BallPool Instance;

    public GameObject ballPrefab;
    public int poolSize = 5;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ball = Instantiate(ballPrefab);
            ball.SetActive(false);
            pool.Enqueue(ball);
        }
    }

    public GameObject GetBall()
    {
        if (pool.Count > 0)
        {
            GameObject ball = pool.Dequeue();
            ball.SetActive(true);
            return ball;
        }

        // fallback
        GameObject newBall = Instantiate(ballPrefab);
        return newBall;
    }

    public void ReturnBall(GameObject ball)
    {
        ball.SetActive(false);
        pool.Enqueue(ball);
    }
}