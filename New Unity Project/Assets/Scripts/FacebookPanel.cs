﻿using UnityEngine;
using System.Collections;
using Facebook.Unity;

public class FacebookPanel : UIBase
{
	public void Initialize()
  {
    if (!FB.IsInitialized) {
      // Initialize the Facebook SDK
      FB.Init(InitCallback, OnHideUnity);
    }
    else {
      // Already initialized, signal an app activation App Event
      FB.ActivateApp();
    }

    FBLogin();
  }

  void FBLogin()
  {
    var perms = new List<string>(){"public_profile", "email", "user_friends"};
    FB.LogInWithReadPermissions(perms, AuthCallback);
  }

  private void AuthCallback (ILoginResult result)
  {
    if (FB.IsLoggedIn)
    {
      // AccessToken class will have session details
      var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
      // Print current access token's User ID
      Debug.Log(aToken.UserId);
      // Print current access token's granted permissions
      foreach (string perm in aToken.Permissions) {
          Debug.Log(perm);
      }
    }
    else {
      Debug.Log("User cancelled login");
    }
  }

  public override void RenderGUI()
  {

  }
}