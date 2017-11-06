using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Api : MonoBehaviour
{
    public User user;
    public SceneLoader sceneLoader;
    public static string urlPath = "http://dev.pushstart.com.br/desafio/public/api/";
    //public Login login;
    public bool loaded = false;
    public string token;
    [HideInInspector]
    public string errorString = "";
    public string _response { get; set; }
    public string response
    {
        get
        {
            return _response;
        }
        set
        {
            _response = value;
        }
    }

    public string _responseGet;
    public string responseGet
    {
        get { return _responseGet; }
        set
        {
            _responseGet = value;
        }
    }

    [Header("Text fields")]
    public Text username;
    public InputField password;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    string SHA256(string randomString)
    {
        System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
        System.Text.StringBuilder hash = new System.Text.StringBuilder();
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString), 0, Encoding.UTF8.GetByteCount(randomString));
        foreach (byte theByte in crypto)
        {
            hash.Append(theByte.ToString("x2"));
        }
        return hash.ToString();
    }

    public void LoginRequestButton()
    {
        LoginRequest(username.text, password.text);
    }

    public void LoginRequest(string user, string password)
    {
        string hash = SHA256(password);

        Request reqObj = new Request(user, hash);
        string req = JsonUtility.ToJson(reqObj, true);
        string loginURL = urlPath + "auth/login";
        httpPost(loginURL, req);
    }

    public void httpPost(string url, string json)
    {
        StartCoroutine(IEnumeratorPost(url, json));
    }

    public void httpGet(string url, string token)
    {
        StartCoroutine(IEnumeratorGet(url, token));
    }

    public IEnumerator IEnumeratorPost(string url, string json)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] pData = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
        WWW webRequest = new WWW(url, pData, headers);
        yield return webRequest;
        if (webRequest.error != null)
        {
            string error = webRequest.error;
            switch (error)
            {
                case "400":
                    print("Invalid Data");
                    yield break;

                case "401":
                    print("User not found");
                    yield break;

                case "403":
                    print("Forbidden");
                    yield break;
            }
        }
        if (webRequest.text != null)
        {
            response = webRequest.text;
            token = JsonUtility.FromJson<Response>(response).token;
            httpGet(urlPath + "status", token);
        }
    }

    public IEnumerator IEnumeratorGet(string url, string token)
    {
        UnityEngine.Networking.UnityWebRequest webRequest = UnityEngine.Networking.UnityWebRequest.Get(url);
        webRequest.SetRequestHeader("X-Authorization", token);
        yield return webRequest.Send();
        if (webRequest.responseCode!=200)
        {
            switch (webRequest.responseCode)
            {
                case 400:
                    print("Invalid Data");
                    yield break;

                case 401:
                    print("User not found");
                    yield break;

                case 403:
                    print("Forbidden");
                    yield break;
            } 
        }
        user = JsonUtility.FromJson<User>(webRequest.downloadHandler.text);
        sceneLoader.ChangeToScene("main");
    }
    

    [System.Serializable]
    public class Request
    {
        public string username;
        public string password;
        public Request(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    [System.Serializable]
    public class Response
    {
        public Profile profile;
        public string token;
        public Response(string token)
        {
            this.token = token;
        }
    }

    public class Profile
    {
        public string name;
        public string type;
        public Profile(string name, string type)
        {
            this.name = name;
            this.type = type;
        }
    }
}