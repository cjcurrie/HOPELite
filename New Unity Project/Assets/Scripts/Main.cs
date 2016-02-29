using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
  // === Global ===
  public static string appName = "HOPELite";

  // === Cache ===
  public GameObject interfaceObject, eventProcessorObject;
  UIBase currentUI;
  EventProcessor eventProcessor;

  // Starting point of app
  public void Awake()
  {
    InitializeScripts();
    StartCoroutine(FirstTimePopup()); // @TODO: queue event instead
  }

  // Load scripts
  void InitializeScripts()
  {
    currentUI = interfaceObject.GetComponent<DashboardUI>();
    currentUI.Initialize();
    eventProcessor = eventProcessorObject.GetComponent<EventProcessor>();
    eventProcessor.Initialize();
  }

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

  void OnGUI()
  {
    currentUI.RenderGUI();
  }
}
