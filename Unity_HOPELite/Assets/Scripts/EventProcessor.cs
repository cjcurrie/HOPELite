using UnityEngine;
using System.Collections;

public class EventProcessor : MonoBehaviour
{
  EventBase activeEvent;

  public void Initialize()
  {
    activeEvent = null;
  }

  void OnGUI()
  {
    if (activeEvent != null)
    {
      activeEvent.RenderGUI();
    }
  }

  public void ProcessUIEvent(EventBase e)
  {
    StartCoroutine(DoProcessUI(e));
  }
  public IEnumerator DoProcessUI(EventBase e)
  {
    Debug.Log("Begin UI Event");
    activeEvent = e;
    yield return StartCoroutine(e.Action());
    activeEvent = null;
  }
}

public abstract class EventBase
{
  public abstract IEnumerator Action(int counter = 500);
  public abstract void RenderGUI();
}
