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

   public Text name;
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
                SceneManager.LoadScene("Game", LoadSceneMode.Single);

			});
		}
	}
}
