using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NewUser : MonoBehaviour
{

    public InputField username;
    public InputField password;
    public InputField email;


    IEnumerator CreateUser(string username, string email, string password)
    {
        string createUserURL = "http://localhost/nsirpg/insertUser.php";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);
        UnityWebRequest webRequest = UnityWebRequest.Post(createUserURL,form);
        yield return webRequest.SendWebRequest();
    }
    public void CreateNewUser()
    {
        StartCoroutine(CreateUser(username.text, email.text, password.text));
    }
}
