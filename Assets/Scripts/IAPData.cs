using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum IAPProductType
{
    Gold=0,
}

[CreateAssetMenu(fileName = "IAP Data", menuName = "IAP/IAP Data", order = 1)]
public class IAPData : ScriptableObject
{
    public List<ProductIconData> IconData;
}

[Serializable]
public class ProductIconData
{
    [FormerlySerializedAs("ProductType")] public IAPProductType iapProductType;
    public Sprite ProductIcon;
}