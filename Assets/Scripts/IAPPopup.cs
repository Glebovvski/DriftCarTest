using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Popup
{
    public class IAPPopup : Popup
    {
        [SerializeField] private IAPProduct productPrefab;

        [SerializeField] private Button closeBrn;
        
        protected void Awake()
        {
            Init();
            base.Awake();
        }

        private void Init()
        {
            closeBrn.onClick.AddListener(Hide);
        }
    }
}