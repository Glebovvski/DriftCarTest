using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarProp : MonoBehaviour
{
    [field: SerializeField] public CarKey Car;
    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);
}
