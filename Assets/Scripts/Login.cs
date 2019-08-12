using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

    public InputField username;
    public InputField password;


    IEnumerator LoginUser(string username, string password)
    {
        string createUserURL = "http://localhost/nsirpg/login.php";
        WWWForm form = new WWWForm();
        form.AddField("username", username);    
        form.AddField("password", password);
        UnityWebRequest webRequest = UnityWebRequest.Post(createUserURL,form);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
    }
    public void UserLogin()
    {
        StartCoroutine(LoginUser(username.text, password.text));
    }
}
