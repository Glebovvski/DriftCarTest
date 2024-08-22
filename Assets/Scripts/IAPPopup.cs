using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Purchasing;
using UnityEngine.UIElements;
using Zenject;
using Button = UnityEngine.UI.Button;

namespace Popup
{
    public class IAPPopup : Popup
    {
        [SerializeField] private IAPProduct productPrefab;

        [SerializeField] private Transform content;
        [SerializeField] private Button closeBrn;
        [SerializeField] private IAPData iapData;

        [Inject] private DiContainer _diContainer;
        
        protected void Awake()
        {
            Init();
            base.Awake();
        }

        private void Init()
        {
            var catalog = ProductCatalog.LoadDefaultCatalog().allProducts;

            foreach (var product in catalog)
            {
                var productItem = _diContainer.InstantiatePrefab(productPrefab, content).GetComponent<IAPProduct>();
                productItem.Init(product, iapData);
            }

            closeBrn.onClick.AddListener(Hide);
        }
    }
}