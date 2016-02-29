using UnityEngine;
using System.Collections;

public class UIBase : MonoBehaviour
{
  public static float maxWidth, maxHeight, scaleX, scaleY;

  public virtual void Initialize()
  {
    maxWidth = Screen.width;
    maxHeight = Screen.height;

    scaleX = Screen.width/1080f;
    scaleY = Screen.height/1920f;
  }

  public virtual void RenderGUI() {}
}
