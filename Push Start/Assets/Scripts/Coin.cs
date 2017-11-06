using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Vector3 targetPos;
    [SerializeField]
    float speed = 5;
    GameManager gameManager;
    void Start()
    {
        GameManager.managerSingleton.coins.Add(this);
        gameManager = GameManager.managerSingleton;
    }

    public void MoveToTarget()
    {
        targetPos = gameManager.coinHUD.transform.position;
        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            GameManager.managerSingleton.coins.Remove(this);
            GameManager.managerSingleton.audioManager.PlayCoinSound();
            Destroy(gameObject);
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}
