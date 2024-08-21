using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum PropTypes
{
    Main,
    Garage,
}

namespace GameTools
{
    public class MainMenuPropsTransition : MonoBehaviour
    {
        [SerializeField] private List<PropData> propData;

        private PropTypes currentProp;

        private void Awake()
        {
            currentProp = PropTypes.Main;
        }

        public void TransitionTo(PropTypes type, Action onComplete)
        {
            var currentPropData = propData.FirstOrDefault(x => x.type == currentProp);
            var nextPropData = propData.FirstOrDefault(x => x.type == type);
            Toggle(currentPropData.objectsToHide, 0);
            Toggle(nextPropData.objectsToHide,  1);
            onComplete?.Invoke();
            currentProp = type;
        }

        private void Toggle(List<Transform> currentPropObjectsToHide, int fadeValue)
        {
            var sequence = DOTween.Sequence();
            foreach (var obj in currentPropObjectsToHide)
            {
                sequence.Join(obj.DOScale(fadeValue, 1));
            }
        }
    }

    [Serializable]
    internal class PropData
    {
        public PropTypes type;
        public List<Transform> objectsToHide;
    }
}