using System.Globalization;
using System.Linq;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Zenject;

public class IAPProduct : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text price;
    [SerializeField] private Button buyBtn;

    private string id;

    [Inject] private IAPManager iapManager;
    [Inject] private AudioManager audio;

    public void Init(ProductCatalogItem item, IAPData data)
    {
#if UNITY_ANDROID || UNITY_EDITOR
        price.text = item.googlePrice.value.ToString(CultureInfo.InvariantCulture);
#elif UNITY_IPHONE
        price.text = item.applePriceTier.ToString(CultureInfo.InvariantCulture);
#endif

        id = item.id;
        icon.sprite = GetProductSpriteFromData(item.Payouts[0], data);
        name.text = item.defaultDescription.Title;
        buyBtn.onClick.AddListener(OnPurchaseBtnClick);
    }

    private void OnPurchaseBtnClick()
    {
        audio.Play(Sounds.BtnClick);
        iapManager.BuyConsumable(id);
    }

    private Sprite GetProductSpriteFromData(ProductCatalogPayout payout, IAPData data)
    {
        switch (payout.type)
        {
            case ProductCatalogPayout.ProductCatalogPayoutType.Currency:
                return data.IconData.FirstOrDefault(x => string.Equals(x.iapProductType.ToString(), payout.subtype))
                    .ProductIcon;
            case ProductCatalogPayout.ProductCatalogPayoutType.Item:
            case ProductCatalogPayout.ProductCatalogPayoutType.Other:
            case ProductCatalogPayout.ProductCatalogPayoutType.Resource:
                break;
        }
        return data.IconData[0].ProductIcon;
    }
}