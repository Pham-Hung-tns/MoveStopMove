using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private FixedJoystick _Joystick;
    [SerializeField] private TextMeshProUGUI aliveAmount;
    [SerializeField] private GameObject guide;

    // Update is called once per frame
    private void Update()
    {
        UpdateAliveNumber();
        if (guide.activeSelf)
        {
            if (Input.GetMouseButtonDown(0)) guide.SetActive(false);
        }
        if (GameManager.Instance.gameState == GameManager.GameState.gameOver)
        {
            StartCoroutine(GameOver());
        }
        else if (GameManager.Instance.gameState == GameManager.GameState.gameWin)
        {
            StartCoroutine(GameWin());
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        guide.SetActive(true);
        _Joystick.gameObject.SetActive(true);
    }

    public void OpenSetting()
    {
        AudioManager.Instance.PlayClickSound();
        UIManager.Instance.OpenUI(UIName.Setting);
    }

    public void UpdateAliveNumber()
    {
        aliveAmount.text = "Alive: " + GameManager.Instance.TotalCharAlive;
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        UIManager.Instance.OpenUI(UIName.GameOver);
    }

    private IEnumerator GameWin()
    {
        yield return new WaitForSeconds(2);
        UIManager.Instance.OpenUI(UIName.Victory);
    }
}