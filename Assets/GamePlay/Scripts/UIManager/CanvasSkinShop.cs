using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ShopType
{ HatShop, PantShop, ShieldShop, SetShop }

public enum ClothState
{ CantBuy, CanBuy, Select, Equipped }

public enum ClothLockState
{ Lock, UnLock }

public enum ClotherPosition
{
    Head,
    Tail,
    Back,
    LeftHand,
    Pan,
    SetFull
}

public enum ClotherType
{
    Rau, Crown, Ear, Hat, HatCap, HatYellow, Arrow, CowBoy, HeadPhone, Khien, Shield,
    ChamBi, Comy, DaBao, Onion, RainBow, VanTim, BatMan, Pikachu, Skull, Devil, Angle, Witch, DeadPool, Thor
}

public class CanvasSkinShop : UICanvas
{
    public int[] ClothesPrice ={
                        50, 100, 150, 160, 170, 210, 220, 250, 280,
                        250, 290, 300, 330, 350, 380, 400, 450, 550,
                        700, 850,
                        1500, 2000, 2500, 3000, 3500};

    [SerializeField] private TextMeshProUGUI _coinAmountText;
    [SerializeField] private TextMeshProUGUI _CantBuypriceText;
    [SerializeField] private TextMeshProUGUI _CanBuypriceText;
    [SerializeField] private Transform _Shop;
    [SerializeField] private Transform[] _shopBackGround;
    [SerializeField] private Transform[] _shopScrollView;   //Scroll View
    [SerializeField] private Image[] _Frame;
    [SerializeField] private Image[] _Lock;
    [SerializeField] private GameObject[] _stateButton;
    private Dictionary<ClotherType, ClothState> ClothesShopInfo = new Dictionary<ClotherType, ClothState>();
    private Dictionary<ClotherType, ClothLockState> ClothesLockInfo = new Dictionary<ClotherType, ClothLockState>();
    public int ClothesID;

    private void Awake()
    {
        ClothesID = 0;
        for (int i = 0; i < ClothesPrice.Length; i++)    //Tạo List để quản lý trạng thái của từng sản phẩm
        {
            ClothesShopInfo.Add((ClotherType)i, ClothState.CantBuy);
            ClothesLockInfo.Add((ClotherType)i, ClothLockState.Lock);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < ClothesPrice.Length; i++)
        {
            if (PlayerPrefs.GetInt("ClothesShop" + (ClotherType)i) == 1)
            {
                ClothesShopInfo.Remove((ClotherType)i);
                ClothesShopInfo.Add((ClotherType)i, ClothState.CantBuy);
            }
            else if (PlayerPrefs.GetInt("ClothesShop" + (ClotherType)i) == 3)
            {
                ClothesShopInfo.Remove((ClotherType)i);
                ClothesShopInfo.Add((ClotherType)i, ClothState.Select);
            }
            else if (PlayerPrefs.GetInt("ClothesShop" + (ClotherType)i) == 4)
            {
                ClothesShopInfo.Remove((ClotherType)i);
                ClothesShopInfo.Add((ClotherType)i, ClothState.Equipped);
            }
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.shopCamera.gameObject.SetActive(false);
        GameManager.Instance.CurrentPlayer.ChangeAnim(CacheString.ANIM_IDLE);
    }

    public override void OnInit()
    {
        _coinAmountText.text = "" + UIManager.Instance.coinAmount;
        HatShopClick();
        //GetRauID();
        GameManager.Instance.shopCamera.gameObject.SetActive(true);
        GameManager.Instance.CurrentPlayer.ChangeAnim(CacheString.ANIM_DANCE);
    }

    public void XButton()
    {
        AudioManager.Instance.PlayClickSound();
        UIManager.Instance.OpenUI(UIName.MainMenu);
    }

    #region Show Shop

    public void HatShopClick()
    {
        GetRauID();
        OpenShop(ShopType.HatShop);
    }

    public void PantShopClick()
    {
        GetChambiID();
        OpenShop(ShopType.PantShop);
    }

    public void ShieldShopClick()
    {
        GetShieldID();
        OpenShop(ShopType.ShieldShop);
    }

    public void SetShopClick()
    {
        GetDevilID();
        OpenShop(ShopType.SetShop);
    }

    private void OpenShop(ShopType _shopType)
    {
        for (int i = 0; i < _Shop.childCount; i++)
        {
            if (i == (int)_shopType)
            {
                _Shop.GetChild(i).gameObject.SetActive(true);
                _shopBackGround[i].gameObject.SetActive(false);
                _shopScrollView[i].gameObject.SetActive(true);
            }
            else
            {
                _shopBackGround[i].gameObject.SetActive(true);
                _shopScrollView[i].gameObject.SetActive(false);
            }
        }
    }

    #endregion Show Shop

    #region Get ID

    public void GetRauID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 0;
        ChangePrice();
    }

    public void GetCrownID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 1;
        ChangePrice();
    }

    public void GetEarID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 2;
        ChangePrice();
    }

    public void GetHatID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 3;
        ChangePrice();
    }

    public void GetHatCapID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 4;
        ChangePrice();
    }

    public void GetHatYellowID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 5;
        ChangePrice();
    }

    public void GetArrowID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 6;
        ChangePrice();
    }

    public void GetCowboyID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 7;
        ChangePrice();
    }

    public void GetHeadphoneID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 8;
        ChangePrice();
    }

    public void GetChambiID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 11;
        ChangePrice();
    }

    public void GetComyID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 12;
        ChangePrice();
    }

    public void GetDabaoID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 13;
        ChangePrice();
    }

    public void GetOnionID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 14;
        ChangePrice();
    }

    public void GetRainBowID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 15;
        ChangePrice();
    }

    public void GetVantimID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 16;
        ChangePrice();
    }

    public void GetBatmanID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 17;
        ChangePrice();
    }

    public void GetPikachuID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 18;
        ChangePrice();
    }

    public void GetSkullID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 19;
        ChangePrice();
    }

    public void GetShieldID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 10;
        ChangePrice();
    }

    public void GetKhienID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 9;
        ChangePrice();
    }

    public void GetDevilID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 20;
        ChangePrice();
    }

    public void GetAngelID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 21;
        ChangePrice();
    }

    public void GetWitchID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 22;
        ChangePrice();
    }

    public void GetDeadpoolID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 23;
        ChangePrice();
    }

    public void GetThorID()
    {
        AudioManager.Instance.PlayClickSound();
        ClothesID = 24;
        ChangePrice();
    }

    #endregion Get ID

    private void ChangePrice()
    {
        for (int i = 0; i < ClothesPrice.Length; i++)
        {
            if (i == ClothesID)
            {
                _CantBuypriceText.text = "" + ClothesPrice[i];
                _CanBuypriceText.text = "" + ClothesPrice[i];
                _Frame[i].gameObject.SetActive(true);
            }
            else
            {
                _Frame[i].gameObject.SetActive(false);
            }
        }
        UpdateButtonState();
    }

    public void BuyClothes()
    {
        AudioManager.Instance.PlayClickSound();
        if (UIManager.Instance.coinAmount >= ClothesPrice[ClothesID])
        {
            UIManager.Instance.coinAmount -= ClothesPrice[ClothesID];
            ClothesShopInfo.Remove((ClotherType)ClothesID);
            ClothesShopInfo.Add((ClotherType)ClothesID, ClothState.Select);

            ClothesLockInfo.Remove((ClotherType)ClothesID);
            ClothesLockInfo.Add((ClotherType)ClothesID, ClothLockState.UnLock);

            PlayerPrefs.SetInt("ClothesShop" + (ClotherType)ClothesID, 3);
            PlayerPrefs.Save();
        }
        UpdateButtonState();
        UpdateCoinAmount();
        UpdateUnLockState();
        PlayerPrefs.SetInt("Score", UIManager.Instance.coinAmount);
        PlayerPrefs.Save();
    }

    public void Equip()
    {
        AudioManager.Instance.PlayClickSound();
        for (int i = 0; i < ClothesPrice.Length; i++)
        {
            if (ClothesShopInfo[(ClotherType)i] == ClothState.Equipped)   //Chuyển tất cả những vật phẩm đang ở trạng thái Equipped sang trạng thái Select
            {
                ClothesShopInfo.Remove((ClotherType)i);

                ClothesShopInfo.Add((ClotherType)i, ClothState.Select);
                PlayerPrefs.SetInt("ClothesShop" + (ClotherType)i, 3);
                PlayerPrefs.Save();
            }
        }
        if (ClothesShopInfo[(ClotherType)ClothesID] == ClothState.Select) //Chuyển trạng thái của vật phẩm đang được chọn sang Equipped
        {
            ClothesShopInfo.Remove((ClotherType)ClothesID);
            ClothesShopInfo.Add((ClotherType)ClothesID, ClothState.Equipped);
            GameManager.Instance.CurrentPlayer.IdentifyTypeOfClother((ClotherType)ClothesID);
            PlayerPrefs.SetInt("ClothesShop" + (ClotherType)ClothesID, 4);
            PlayerPrefs.Save();
        }
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        for (int i = 0; i < 25; i++)
        {
            if (UIManager.Instance.coinAmount < ClothesPrice[i] && ClothesShopInfo[(ClotherType)i] == ClothState.CanBuy)      //So sánh số tiền với giá. Nếu giá lớn hơn mà trạng thái vẫn để Canbuy thì đổi thành ContBuy
            {
                ClothesShopInfo.Remove((ClotherType)i);
                ClothesShopInfo.Add((ClotherType)i, ClothState.CantBuy);
            }
            else if (UIManager.Instance.coinAmount >= ClothesPrice[i] && ClothesShopInfo[(ClotherType)i] == ClothState.CantBuy)//So sánh số tiền với giá. Nếu giá nhỏ hơn mà trạng thái vẫn để Cantbuy thì đổi thành ContBuy
            {
                ClothesShopInfo.Remove((ClotherType)i);
                ClothesShopInfo.Add((ClotherType)i, ClothState.CanBuy);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (ClothesShopInfo[(ClotherType)ClothesID] == (ClothState)i)
            {
                _stateButton[i].SetActive(true);
            }
            else
            {
                _stateButton[i].SetActive(false);
            }
        }
    }

    private void UpdateUnLockState()
    {
        for (int i = 0; i < ClothesPrice.Length; i++)
        {
            if (ClothesLockInfo[(ClotherType)i] == ClothLockState.UnLock)
            {
                _Lock[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateCoinAmount()
    {
        _coinAmountText.text = "" + UIManager.Instance.coinAmount;
    }
}