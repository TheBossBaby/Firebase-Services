using Facebook.Unity;
using Firebase.Auth;
using Google;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoogleSignInHandler : MonoBehaviour
{
    //Google auth variables
	public string webClientId = "839353381085-qsqi1lcpupf3qb72jbdvvusefbt1s9bp.apps.googleusercontent.com";
	private GoogleSignInConfiguration configuration;

	public GameObject onLogedIn;
	public GameObject ErrorPanel;
	public Text ErrorMessage;
	Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    void Awake()
	{
		//Setup for Google Sign In
		configuration = new GoogleSignInConfiguration
		{
			WebClientId = webClientId,
			RequestIdToken = true
		};

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
	}
	
	/// <summary>
	/// Google Sign-In Click
	/// </summary>
	public void GoogleSignIn_Click()
	{
		//Sign-In with Google as first to get token for Firebase Auth
		OnGoogleSignIn();
	}

	public void GoofleSignOut_Click()
	{
		auth.SignOut();
		GoogleSignIn.DefaultInstance.SignOut ();
		onLogedIn.SetActive(false);		
	}

	void OnGoogleSignIn()
	{
		GoogleSignIn.Configuration = configuration;
		GoogleSignIn.Configuration.UseGameSignIn = false;
		GoogleSignIn.Configuration.RequestIdToken = true;

		GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
		  OnGoogleAuthenticationFinished);
	}

	//Handle when Google Sign In successfully
	void OnGoogleAuthenticationFinished(Task<GoogleSignInUser> task)
	{
		if (task.IsFaulted)
		{
			using (IEnumerator<System.Exception> enumerator =
					task.Exception.InnerExceptions.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					GoogleSignIn.SignInException error =
							(GoogleSignIn.SignInException)enumerator.Current;
					UnityEngine.Debug.Log("Got Error: " + error);
					ErrorPanel.SetActive(true);
					// ErrorMessage.text = (string)error;
				}
				else
				{
					UnityEngine.Debug.Log("Got Unexpected Exception?!?");
				}
			}
		}
		else if (task.IsCanceled)
		{
			UnityEngine.Debug.Log("Canceled");
		}
		else
		{
			UnityEngine.Debug.Log("Google Sign-In successed");

			UnityEngine.Debug.Log("IdToken: " +task.Result.IdToken);
			UnityEngine.Debug.Log("ImageUrl: " + task.Result.ImageUrl.AbsoluteUrlOrEmptyString());
			UnityEngine.Debug.Log("Email: " + task.Result.Email);

			//Start Firebase Auth
			Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
			auth.SignInWithCredentialAsync(credential).ContinueWith(t =>
			{
				if (t.IsCanceled)
				{
					UnityEngine.Debug.Log("SignInWithCredentialAsync was canceled.");
					return;
				}
				if (t.IsFaulted)
				{
					UnityEngine.Debug.Log("SignInWithCredentialAsync encountered an error: " + t.Exception);
					return;
				}

				user = auth.CurrentUser;
				UnityEngine.Debug.Log("Email: " + user.Email);
								
                // SceneManager.LoadScene("Game", LoadSceneMode.Single);
				onLogedIn.SetActive(true);
			});
		}
	}
}
