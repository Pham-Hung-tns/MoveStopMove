using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasGameOver : UICanvas
{
    [SerializeField] private Transform GuideText;
    [SerializeField] private TextMeshProUGUI RankText;
    [SerializeField] private TextMeshProUGUI KillerName;
    [SerializeField] private TextMeshProUGUI CoinAmount;
    private PlayerController playerController;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GuideText.gameObject.activeSelf)
        {
            AudioManager.Instance.PlayClickSound();
            GuideText.gameObject.SetActive(false);
            UIManager.Instance.OpenUI(UIName.MainMenu);
            GameManager.Instance.ResetStats();
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        AudioManager.Instance.PlayLoseAudio();
        StartCoroutine(ShowGuide());
        playerController = GameManager.Instance.CurrentPlayer;
        RankText.text = "#" + GameManager.Instance.TotalCharAlive;
        KillerName.text = "" + playerController.KillerName;
        CoinAmount.text = "" + playerController.Level;
        UIManager.Instance.coinAmount += playerController.Level;
        PlayerPrefs.SetInt("Score", UIManager.Instance.coinAmount);
        PlayerPrefs.Save();
    }

    private IEnumerator ShowGuide()
    {
        yield return new WaitForSeconds(3);
        GuideText.gameObject.SetActive(true);
    }
}