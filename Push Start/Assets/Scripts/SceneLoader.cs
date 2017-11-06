using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Text loadingHUD;
    public void ChangeToScene(string targetScene)
    {
        loadingHUD.text = "CARREGANDO";
        SceneManager.LoadScene(targetScene);
    }

}
