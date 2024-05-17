using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSetting : UICanvas
{
    public void ContinueButton()
    {
        AudioManager.Instance.PlayClickSound();
        UIManager.Instance.OpenUI(UIName.GamePlay);
    }

    public void HomeButton()
    {
        AudioManager.Instance.PlayClickSound();
        //TODO: RETURN TO HOME
    }
}