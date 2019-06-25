using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class EmailLoginHandler : MonoBehaviour
{

    public InputField emailInputField;
    public InputField passwordInputField;
    private string email = "test2@gmail.com";
    private string password = "qwErty@123";

	Firebase.Auth.FirebaseAuth auth;

    private 
    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }
    
    public void SignUpClick()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
        if (task.IsCanceled) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
            return;
        }
        if (task.IsFaulted) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            return;
        }

        // Firebase user has been created.
        Firebase.Auth.FirebaseUser newUser = task.Result;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);        
        Debug.LogFormat("Firebase user created successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);
        });
    }

    public void SignInClick()
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
        if (task.IsCanceled) {
            Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
            return;
        }
        if (task.IsFaulted) {
            Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            return;
        }

        Firebase.Auth.FirebaseUser newUser = task.Result;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);

        Debug.LogFormat("User signed in successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);
        });
    }

    public void TakeEmailInput()
    {
        email = emailInputField.text;
    }

    public void TakePasswordInput()
    {
        password = passwordInputField.text;
    }
}
