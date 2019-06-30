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

    public InputField CreateEmailInputField;
    public InputField CreatePasswordInputField;

    public GameObject createNewAccountPanel;
    private string email;
    private string password;

    private string createEmail;
    private string createPassword;

    public GameObject onLogedIn;
	public GameObject ErrorPanel;
	public Text ErrorMessage;

	Firebase.Auth.FirebaseAuth auth;

    private 
    // Start is called before the first frame update
    void Start()
    {
        createNewAccountPanel.SetActive(false);
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }
    
    public void SignUpClick()
    {
        auth.CreateUserWithEmailAndPasswordAsync(createEmail, createPassword).ContinueWith(task => {
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
        onLogedIn.SetActive(true);
        // SceneManager.LoadScene("Game", LoadSceneMode.Single);        
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
		    onLogedIn.SetActive(false);
            ErrorPanel.SetActive(true);            
            return;
        }

        Firebase.Auth.FirebaseUser newUser = task.Result;
        onLogedIn.SetActive(true);
        // SceneManager.LoadScene("Game", LoadSceneMode.Single);

        Debug.LogFormat("User signed in successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);
        });
    }

	public void SignOut_Click()
	{
		auth.SignOut();
		onLogedIn.SetActive(false);
	}    

    public void TakeEmailInput()
    {
        email = emailInputField.text;
        Debug.Log(email);
    }

    public void TakePasswordInput()
    {
        password = passwordInputField.text;
        Debug.Log(password);
    }

    public void TakeCreateEmailInput()
    {
        createEmail = CreateEmailInputField.text;
        Debug.Log(createEmail);
    }

    public void TakeCreatePasswordInput()
    {
        createPassword = CreatePasswordInputField.text;
        Debug.Log(createPassword);
    }
    public void ShowCreateNewAccount()
    {
        createNewAccountPanel.SetActive(true);
    }
}
