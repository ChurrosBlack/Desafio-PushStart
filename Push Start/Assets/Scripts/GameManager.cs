using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager managerSingleton;

    public User user;
    public CameraMovement cameraMov;
    public AudioManager audioManager;

    public int money;
    public List<Building> buildings;
    public List<Coin> coins;


    [Header("HUD Management")]
    public Image coinHUD;
    public Text scoreText;
    public Text nickname;
    public List<BuildingHUD> buildingHUDList = new List<BuildingHUD>();
    public Image pauseScreen;

    [Header("Building Prefabs")]
    [SerializeField]
    GameObject factoryPrefab;
    [SerializeField]
    GameObject farmPrefab;
    [SerializeField]
    GameObject parkPrefab;
    [SerializeField]
    GameObject housesPrefab;
    [SerializeField]
    GameObject mallPrefab;

    void Awake()
    {
        Resume();
        managerSingleton = this;
        //cash = user.cash;
        Api apiRef = GameObject.FindGameObjectWithTag("ApiManager").GetComponent<Api>();
        user = apiRef.user;
        apiRef.enabled = false;
        money = user.money;
        scoreText.text = ": " + money;
        nickname.text = user.nickname;
    }

    public void PayCash(int amount)
    {
        money -= amount;
        scoreText.text = (": " + this.money);
        UpdateHUDList();
    }

    public void AddCash(int money)
    {
        this.money += money;
        scoreText.text = (": " + this.money);
        UpdateHUDList();
    }

    //Test
    public void UpdateHUDList()
    {
        foreach (BuildingHUD bHUD in buildingHUDList)
        {
            if (money < bHUD.buildData.cashCost)
            {
                bHUD.buildData.enabled = false;
                bHUD.img.color = Color.red;
            }
            else
            {

                bHUD.buildData.enabled = true;
                bHUD.img.color = Color.white;
            }
        }
    }

    public void PauseButton()
    {
        if (pauseScreen.IsActive())
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void Pause()
    {
        pauseScreen.enabled = true;
        Time.timeScale = 0f;
        print("PAUSED");
    }

    void Resume()
    {
        pauseScreen.enabled = false;
        Time.timeScale = 1f;
        print("RESUMED");
    }


    void Update()
    {
        cameraMov.Tick();
        foreach (Building buildingInList in buildings)
        {
            buildingInList.Tick();
        }

        for (int i = coins.Count - 1; i >= 0; i--)
        {
            coins[i].MoveToTarget();
        }
    }

    public GameObject InstantiateBuilding(BuildingType type, Vector3 position)
    {
        audioManager.PlayBuildingSound(type);
        switch (type)
        {
            case BuildingType.FARM:
                return Instantiate(farmPrefab, position, Quaternion.identity);
            case BuildingType.HOUSES:
                return Instantiate(housesPrefab, position, Quaternion.identity);
            case BuildingType.MALL:
                return Instantiate(mallPrefab, position, Quaternion.identity);
            case BuildingType.PARK:
                return Instantiate(parkPrefab, position, Quaternion.identity);
            case BuildingType.FACTORY:
                return Instantiate(factoryPrefab, position, Quaternion.identity);
            default:
                break;
        }
        return null;
    }
}

public enum BuildingType
{
    FARM,
    HOUSES,
    MALL,
    PARK,
    FACTORY
}

[System.Serializable]
public class BuildingHUD
{
    public Image img;
    public BuildingData buildData;
    public BuildingHUD(Image img, BuildingData buildData)
    {
        this.img = img;
        this.buildData = buildData;
    }
}