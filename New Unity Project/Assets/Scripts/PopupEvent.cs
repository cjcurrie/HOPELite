using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupEvent : EventBase
{
  enum PopupElement {None, Window, TitleArea, TextArea, ReturnButton, Counter};
  Dictionary<PopupElement, Rect> rects;

  string title, text;
  bool showReturn, doReturn;
  int counter;

  public PopupEvent(Rect w, string title_in, string text_in)
  {
    text = text_in;
    title = title_in;
    rects = new Dictionary<PopupElement, Rect>();
    rects[PopupElement.Window] = w;

    float paddingX = 30 * UIBase.scaleX, paddingY = 40 * UIBase.scaleY;

    float third = w.width/3f;
    float h_third = w.height/3f;
    Rect t = new Rect(w.x+w.width*.1f, w.y + h_third-paddingY, w.width*.8f, paddingY*2);
    rects[PopupElement.TitleArea] = t;

    Rect x = new Rect(w.x+paddingX, w.y+h_third+h_third-paddingY, w.width-paddingX*2, h_third);
    rects[PopupElement.TextArea] = x;

    rects[PopupElement.ReturnButton] = new Rect(w.x+w.width/2-50*UIBase.scaleX, w.y+w.height-paddingY-50*UIBase.scaleY, 100*UIBase.scaleX, 40*UIBase.scaleY);

    rects[PopupElement.Counter] = new Rect(w.x+20, w.y+w.height-30, 100,40);
  }

  public override IEnumerator Action(int countdown)
  {
    yield return new WaitForSeconds(1);

    showReturn = true;
    doReturn = false;

    counter = countdown;    // opt. 250
    while (!doReturn && counter > 0)
    {
      counter --;
      yield return null;
    }
  }

  public override void RenderGUI()
  {
    GUI.Box(rects[PopupElement.Window], "");
    GUI.Label(rects[PopupElement.TitleArea], title);
    GUI.Label(rects[PopupElement.TextArea], text);

    if (showReturn)
    {
      if (GUI.Button(rects[PopupElement.ReturnButton], "Return"))
      {
        showReturn = false;
        doReturn = true;
      }
    }

    if (counter > 0)
    {
      GUI.Label(rects[PopupElement.Counter], "Timeout: "+counter);
    }
  }
}
