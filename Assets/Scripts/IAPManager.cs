using System;
using System.Linq;
using Core;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

    [Inject] private PlayerData _playerData;

    private string currentPurchaseID = string.Empty;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (storeController == null)
        {
            InitializePurchasing();
        }
    }

    private void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));

        foreach (var product in ProductCatalog.LoadDefaultCatalog().allProducts)
        {
            builder.AddProduct(product.id, product.type);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    public void BuyConsumable(string id)
    {
        BuyProductID(id);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = storeController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                currentPurchaseID = productId;
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.LogError(
                    "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase.");
            }
        }
        else
        {
            Debug.LogError("BuyProductID FAIL. Not initialized.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("OnInitializeFailed InitializationFailureReason:" + error + "\n" + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError(
            $"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, currentPurchaseID, StringComparison.Ordinal))
        {
            Debug.Log("ProcessPurchase: PASS. Product: " + args.purchasedProduct.definition.id);
            _playerData.AddGold((int)args.purchasedProduct.definition.payout.quantity);
        }
        else
        {
            Debug.LogError($"ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseComplete()
    {
        var product = ProductCatalog.LoadDefaultCatalog().allProducts.FirstOrDefault(x => x.id == currentPurchaseID);
        _playerData.AddGold((int)product.Payouts[0].quantity);
    }
}