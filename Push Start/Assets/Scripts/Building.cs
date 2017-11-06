using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Building : MonoBehaviour
{
    public float pointsTimeRate;
    public int points;
    public float timeToBuild;
    WaitForSeconds wait;
    bool isWaiting;
    SpriteRenderer spriteRendererComp;
    public bool canBuild;
    public Color blockBuildingColor;
    Color canBuildColor = new Color(1,1,1,1);
    [SerializeField]
    Collider2D boxCollider;
    [SerializeField]
    GameObject coinPrefab;

    public void Start()
    {
        canBuild = true;
        wait = new WaitForSeconds(pointsTimeRate);
        spriteRendererComp = GetComponent<SpriteRenderer>();
        boxCollider.isTrigger = false;
        SetVisibility(.5f);
    }

    public void AddToManagerList()
    {
        boxCollider.isTrigger = true;
        GameManager.managerSingleton.buildings.Add(this);
    }

    public void Tick()
    {
        if (timeToBuild > 0)
        {
            timeToBuild -= Time.deltaTime;
            if (timeToBuild <= 0) SetVisibility(1f);
        }
        else
        {
            StartCoroutine("WaitSeconds");
        }
    }

    void SetVisibility(float a)
    {
        if (!spriteRendererComp) return;
        spriteRendererComp.color = new Color(
           spriteRendererComp.color.r,
           spriteRendererComp.color.g,
           spriteRendererComp.color.b,
           a);
    }

    IEnumerator WaitSeconds()
    {
        if (isWaiting) yield return null;
        else
        {
            isWaiting = true;
            yield return wait;
            isWaiting = false;
            GameManager.managerSingleton.AddCash(points);
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Building")
        {
            canBuild = false;
            spriteRendererComp.color = blockBuildingColor;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Building")
        {
            canBuild = true;
            spriteRendererComp.color = canBuildColor;
        }
    }
}
