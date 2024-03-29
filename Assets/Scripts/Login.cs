﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

public class Login : MonoBehaviour
{

    public InputField username;
    public InputField password;
    public InputField email;
    public InputField resetCode;
    public GameObject basePanel, loginPanel;
    public GameObject resetButton;
    private string characters = "0123456789abcdefghijklmnopqrstuvwxABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private string code = "";

    private string _username;

    void CreateCode()
    {
        for (int i = 0; i < 5; i++)
        {
            int a = UnityEngine.Random.Range(0, characters.Length);
            code = code + characters[a];
        }

        Debug.Log(code);
    }

    IEnumerator LoginUser(string username, string password)
    {
        string createUserURL = "http://localhost/nsirpg/login.php";
        WWWForm form = new WWWForm();
        form.AddField("username", username);    
        form.AddField("password", password);
        UnityWebRequest webRequest = UnityWebRequest.Post(createUserURL,form);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
        if(webRequest.downloadHandler.text == "Login Successful")
        {
            NextScene.LoadSceneNext();
        }
        else
        {
            basePanel.SetActive(true);
            loginPanel.SetActive(false);
        }
    }

    public void UserLogin()
    {
        StartCoroutine(LoginUser(username.text, password.text));
    }

    public void SendEmail(InputField _email)
    {
        CreateCode();
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress ("sqlunityclasssydney@gmail.com");
        mail.To.Add(_email.text);
        mail.Subject = "NSIRPGPassword Reset";
        mail.Body = "Hello " + _username + "\nReset using this code: " + code;

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 25;
        smtpServer.Credentials = new NetworkCredential("sqlunityclasssydney@gmail.com", "sqlpassword") as ICredentialsByHost;
        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        { return true; };

        smtpServer.Send(mail);
        Debug.Log("Sending Email");
    }
    IEnumerator ForgotPassword(InputField _email)
    {
        string forgottonPasswordURL = "http://localhost/nsirpg/CheckEmail.php";
        WWWForm form = new WWWForm();
        form.AddField("email_Post", _email.text);
    
        UnityWebRequest webRequest = UnityWebRequest.Post(forgottonPasswordURL, form);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
        if (webRequest.downloadHandler.text == "User Not Found")
        {
           // notification.text = webRequest.downloadHandler.text;
        }
        else
        {
            _username = webRequest.downloadHandler.text;
            Debug.Log("User set to " + _username);
            SendEmail(_email);
        }
    }
    public void ResetPassword(InputField _email)
    {
        Debug.Log(_email.text);
        StartCoroutine(ForgotPassword(_email));
    }
    IEnumerator UpdatePassword(string _password)
    {
        string forgottonPasswordURL = "http://localhost/nsirpg/UpdatePassword.php";
        WWWForm form = new WWWForm();
        form.AddField("password_Post", _password);
        form.AddField("username_Post", _username);

        UnityWebRequest webRequest = UnityWebRequest.Post(forgottonPasswordURL, form);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
    }
    public void ChangePassword(InputField _password)
    {
        string pass = _password.text;
        StartCoroutine(UpdatePassword(pass));
    }

    public void CheckCode()
    {
        if (resetCode.text == code)
        {
            resetButton.SetActive(true);
        }
    }
}

