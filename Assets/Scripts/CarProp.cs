using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarProp : MonoBehaviour
{
    [field: SerializeField] public CarKey Car;
    public void Toggle(bool value) => gameObject.SetActive(value);
}
