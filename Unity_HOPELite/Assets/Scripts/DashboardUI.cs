using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DashboardUI : UIBase
{
  enum DashElement{None, Banner, Header, Footer, Body, LeftColumn, Checklist, ChecklistFull};
  Dictionary<DashElement, Rect> rects;

  Vector2 checkSrollPos;

  public override void Initialize()
  {
    base.Initialize();

    rects = new Dictionary<DashElement, Rect>();

    // Banner
    Rect r = new Rect(0,0,maxWidth, 100*scaleY);
      rects[DashElement.Banner] = r;
    // Header
    r = new Rect(0,r.y+r.height,maxWidth, 250*scaleY);
      rects[DashElement.Header] = r;
    // Footer
    Rect f = new Rect(0, maxHeight-250*scaleY, maxWidth, 250*scaleY);
      rects[DashElement.Footer] = f;
    // Body
    float p = r.y+r.height;
    r = new Rect(0,p, maxWidth, maxHeight-p-f.height);
      //rects[DashElement.Body] = r;
    // Left column
    r = new Rect(0,r.y, 300*scaleX, r.height);
      rects[DashElement.LeftColumn] = r;
    // Checklist
    r = new Rect(r.x+r.width,r.y, maxWidth-r.width, r.height);
      rects[DashElement.Checklist] = r;
    // Full checklist scrollview
    r = new Rect(0,0,r.width, 1800*scaleY);
      rects[DashElement.ChecklistFull] = r;
  }

  public override void RenderGUI()
  {
    GUI.Box(rects[DashElement.Banner], "");
    GUI.Box(rects[DashElement.Header], "");
    GUI.Box(rects[DashElement.LeftColumn], "");

    checkSrollPos = GUI.BeginScrollView(rects[DashElement.Checklist], checkSrollPos, rects[DashElement.ChecklistFull]);
      GUILayout.BeginHorizontal();
        GUILayout.Button("Help");
        GUILayout.Box("How much water did you drink today?");
      GUILayout.EndHorizontal();
    GUI.EndScrollView();

    GUI.Box(rects[DashElement.Footer], "");
  }
}
