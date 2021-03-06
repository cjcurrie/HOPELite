﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
  // === Global ===
  public static string appName = "HOPELite";

  // === Cache ===
  public GameObject interfaceObject, eventProcessorObject;
  UIBase[] activeInterfaces;
  EventProcessor eventProcessor;
  ServerInterface serverInterface;
  

  // Starting point of app
  public void Awake()
  {
    InitializeScripts();
    StartCoroutine(FirstTimePopup()); // @TODO: queue event instead
  }

  // Load scripts
  void InitializeScripts()
  {
    // --- Web sockets
    serverInterface = new ServerInterface();
    serverInterface.Initialize();

    // --- Interfaces
    List<UIBase> interfaces = new List<UIBase>();

    // Dashboard
    UIBase dashboard = interfaceObject.GetComponent<DashboardUI>();
    dashboard.Initialize();
    interfaces.Add(dashboard);

    // Facebook
    UIBase facebookPanel = interfaceObject.GetComponent<FacebookPanel>();
    dashboard.Initialize();
    interfaces.Add(dashboard);

    activeInterfaces = interfaces.ToArray();

    // --- Events
    eventProcessor = eventProcessorObject.GetComponent<EventProcessor>();
    eventProcessor.Initialize();

    // --- Get main page content
    //Debug.Log("pinging server for content....");
  }

  void OnGUI()
  {
    for (int i=0; i<activeInterfaces.Length; i++)
      activeInterfaces[i].RenderGUI();
  }


  // === Scripted events ===
  IEnumerator FirstTimePopup()
  {
    PopupEvent p = new PopupEvent(new Rect(Screen.width*.15f, 300, Screen.width*.7f, 400), "Welcome to "+appName+"!", "Begin by exploring the toolbox of useful daily metrics. If you see a that metric that looks useful to you, tap to add it to your checklist.\n\nThe checklist is your custom list of daily reminders. Add anything you want to it – but we recommend starting with the basics. You'll unlock more tools as you level up.");
    yield return StartCoroutine(eventProcessor.DoProcessUI(p));
    
    ExplainLevelsPopup();
  }

  void ExplainLevelsPopup()
  {
    PopupEvent p = new PopupEvent(new Rect(Screen.width*.15f, 300, Screen.width*.7f, 400), "How does leveling work?", "Every day you will be given random quests to explore different parts of "+appName+". Complete these to develop your Pillars of Awesomeness and unlock more functionality. Check the store or the toolbox often to find new features.");
    eventProcessor.ProcessUIEvent(p);
  }


  // === Callback hooks ===
  private void OnHideUnity (bool isGameShown)
  {
    if (!isGameShown) {
      // Pause the game - we will need to hide
      Time.timeScale = 0;
    }
    else {
      // Resume the game - we're getting focus again
      Time.timeScale = 1;
    }
  }
}
