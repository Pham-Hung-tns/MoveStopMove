using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponState
{ CantBuy, CanBuy, Select, Equipped }

public enum weaponType
{ Arrow, Axe_0, Axe_1, boomerang, candy_0, candy_1, candy_2, candy_4, Hammer, knife, uzi, Z }

public class CanvasWeaponShop : UICanvas
{
    public int[] weaponPrices = { 2500, 800, 1500, 400, 100, 200, 1000, 600, 30, 200, 3000, 2000 };
    [SerializeField] private TextMeshProUGUI _coinAmountText, _cantBuyText, _canBuyText, _weaponName;
    [SerializeField] private Button nextButton, prevButton;
    [SerializeField] private List<GameObject> _ListWeapon;
    [SerializeField] private Transform weaponStateButton;
    private Dictionary<weaponType, WeaponState> WeaponShopInfo = new Dictionary<weaponType, WeaponState>();
    private int ShopWeaponID;

    private void Awake()
    {
        ShopWeaponID = 0;
        for (int i = 0; i < weaponPrices.Length; i++)
        {
            WeaponShopInfo.Add((weaponType)i, WeaponState.CantBuy);
        }
        PlayerPrefs.SetInt("WeaponShop" + (weaponType)((int)(weaponType.Hammer)), 3);    //Đặt vũ khí mặc định hammer là đã mua
        PlayerPrefs.Save();
    }

    private void OnEnable()
    {
        nextButton.onClick.AddListener(NextButton);
        prevButton.onClick.AddListener(BackButton);

        for (int i = 0; i < weaponPrices.Length; i++)
        {
            if (PlayerPrefs.GetInt("WeaponShop" + (weaponType)i) == 1)
            {
                WeaponShopInfo.Remove((weaponType)i);
                WeaponShopInfo.Add((weaponType)i, WeaponState.CantBuy);
            }
            else if (PlayerPrefs.GetInt("WeaponShop" + (weaponType)i) == 2)
            {
            }
            else if (PlayerPrefs.GetInt("WeaponShop" + (weaponType)i) == 3)
            {
                WeaponShopInfo.Remove((weaponType)i);
                WeaponShopInfo.Add((weaponType)i, WeaponState.Select);
            }
            else if (PlayerPrefs.GetInt("WeaponShop" + (weaponType)i) == 4)
            {
                WeaponShopInfo.Remove((weaponType)i);
                WeaponShopInfo.Add((weaponType)i, WeaponState.Equipped);
            }
        }
    }

    public void OpenMainMenu()
    {
        AudioManager.Instance.PlayClickSound();
        UIManager.Instance.OpenUI(UIName.MainMenu);
    }

    private void OnDisable()
    {
        GameManager.Instance.shopCamera.gameObject.SetActive(false);
        GameManager.Instance.CurrentPlayer.ChangeAnim(CacheString.ANIM_IDLE);

        nextButton.onClick.RemoveListener(NextButton);
        prevButton.onClick.RemoveListener(BackButton);
    }

    public override void OnInit()
    {
        _coinAmountText.text = "" + UIManager.Instance.coinAmount;
        UpdateWeaponShopState();
        ShowWeaponImage((weaponType)ShopWeaponID);
        SetPriceText(weaponPrices[ShopWeaponID]);
        ShowState();
        GameManager.Instance.shopCamera.gameObject.SetActive(true);
        GameManager.Instance.CurrentPlayer.ChangeAnim(CacheString.ANIM_DANCE);
    }

    public void NextButton()
    {
        AudioManager.Instance.PlayClickSound();
        if (ShopWeaponID < 11)
        {
            ShopWeaponID++;
            ShowWeaponImage((weaponType)ShopWeaponID);
            UpdateWeaponShopState();
            SetPriceText(weaponPrices[ShopWeaponID]);
            ShowState();
        }
    }

    public void BackButton()
    {
        AudioManager.Instance.PlayClickSound();
        if (ShopWeaponID > 0)
        {
            ShopWeaponID--;
            ShowWeaponImage((weaponType)ShopWeaponID);
            UpdateWeaponShopState();
            SetPriceText(weaponPrices[ShopWeaponID]);
            ShowState();
        }
    }

    public void ShowWeaponImage(weaponType weaponType)
    {
        for (int i = 0; i < _ListWeapon.Count; i++)
        {
            _ListWeapon[i].gameObject.SetActive(false);
        }
        _ListWeapon[(int)weaponType].gameObject.SetActive(true);
        _weaponName.text = "" + weaponType;
    }

    public void UpdateWeaponShopState()
    {
        for (int i = 0; i < weaponPrices.Length; i++)
        {
            // enough coin and the status of current weapon is "CantBuy"
            if (UIManager.Instance.coinAmount >= weaponPrices[i] && WeaponShopInfo[(weaponType)i] == WeaponState.CantBuy)
            {
                WeaponShopInfo.Remove((weaponType)i);
                WeaponShopInfo.Add((weaponType)i, WeaponState.CanBuy); // Change status of current weapon to "CanBuy"
            }
            else if (UIManager.Instance.coinAmount < weaponPrices[i] && WeaponShopInfo[(weaponType)i] == WeaponState.CanBuy)
            {
                WeaponShopInfo.Remove((weaponType)i);
                WeaponShopInfo.Add((weaponType)i, WeaponState.CantBuy);
            }
        }
    }

    public void BuyWeapon()
    {
        if (WeaponShopInfo[(weaponType)ShopWeaponID] == WeaponState.CanBuy)
        {
            WeaponShopInfo.Remove((weaponType)ShopWeaponID);
            WeaponShopInfo.Add((weaponType)ShopWeaponID, WeaponState.Select);
            UIManager.Instance.coinAmount -= weaponPrices[(int)((weaponType)ShopWeaponID)];
            ShowState();
            UpdateCoinAmount();
            PlayerPrefs.SetInt("Score", UIManager.Instance.coinAmount);
            PlayerPrefs.Save();
            PlayerPrefs.SetInt("WeaponShop" + (weaponType)ShopWeaponID, 3);
            PlayerPrefs.Save();
            AudioManager.Instance.PlayClickSound();
        }
    }

    public void SelectWeapon()
    {
        if (WeaponShopInfo[(weaponType)ShopWeaponID] == WeaponState.Select)
        {
            for (int i = 0; i < WeaponShopInfo.Count; i++)
            {
                if (WeaponShopInfo[(weaponType)i] == WeaponState.Equipped)
                {
                    WeaponShopInfo.Remove((weaponType)i);
                    WeaponShopInfo.Add((weaponType)i, WeaponState.Select);
                    PlayerPrefs.SetInt("WeaponShop" + (weaponType)i, 3);
                    PlayerPrefs.Save();
                }
            }
            //TODO: SELECT WEAPON AND CHANGE MATERIAL
            GameManager.Instance.CurrentPlayer.weaponSwitching((Character.weaponType)ShopWeaponID, new Character.WeaponMaterialsType[] { Character.WeaponMaterialsType.Arrow });
            WeaponShopInfo.Remove((weaponType)ShopWeaponID);
            WeaponShopInfo.Add((weaponType)ShopWeaponID, WeaponState.Equipped);
            PlayerPrefs.SetInt("WeaponShop" + (weaponType)ShopWeaponID, 4);
            PlayerPrefs.Save();
            ShowState();
            AudioManager.Instance.PlayClickSound();
        }
    }

    private void ShowState()
    {
        for (int i = 0; i < weaponStateButton.transform.childCount; i++)
        {
            if (WeaponShopInfo[(weaponType)ShopWeaponID] == (WeaponState)i)
            {
                weaponStateButton.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                weaponStateButton.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void SetPriceText(int price)
    {
        _canBuyText.text = "" + price;
        _cantBuyText.text = "" + price;
    }

    private void UpdateCoinAmount()
    {
        _coinAmountText.text = "" + UIManager.Instance.coinAmount;
    }
}