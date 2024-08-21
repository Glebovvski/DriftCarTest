using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public enum PropTypes
{
    Main,
    Garage,
}

public enum PropMoveDirection
{
    Vertical,
    Horizontal,
}

namespace GameTools
{
    [Serializable]
    public class PropData
    {
        public PropTypes PropType;
        public PropMoveDirection Direction;
        public GameObject Prop;
    }

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
            var transitionSequence = DOTween.Sequence();
            var currentData = propData.FirstOrDefault(x => x.PropType == currentProp);
            var nextData = propData.FirstOrDefault(x => x.PropType == type);
            Vector3 currentPropDirection = MoveDirection(currentData, false);
            Vector3 nextPropDirection = MoveDirection(nextData, true);

            transitionSequence.Join(currentData.Prop.transform.DOMove(currentPropDirection * 5f, 2f));
            transitionSequence.Join(nextData.Prop.transform.DOMove(nextPropDirection * 5f, 2f));
            transitionSequence.OnComplete(() => onComplete?.Invoke());
        }

        private Vector3 MoveDirection(PropData data, bool transitionToThis)
        {
            var direction = data.Direction == PropMoveDirection.Horizontal
                ? data.Prop.transform.forward
                : data.Prop.transform.up;
            return direction * (transitionToThis ? 1 : -1);
        }
    }
}