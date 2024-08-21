using System;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Popup
{
    public abstract class Popup : MonoBehaviour
    {
        [SerializeField] protected bool hideOnAwake = true;
        [SerializeField] protected Image bgImg;
        [SerializeField] protected Image popup;
        [SerializeField] protected Color endFadeColor;

        protected Action OnCompleteShow;

        protected Sequence showSequence;

        [Inject] protected AudioManager audio;


        protected void Awake()
        {
            if (hideOnAwake)
                Hide();
        }

        public virtual void Show()
        {
            audio.Play(Sounds.BtnClick);
            showSequence = DOTween.Sequence();

            showSequence.Append(bgImg.transform.DOScale(1, 0));
            showSequence.Append(bgImg.DOColor(endFadeColor, 1));
            showSequence.Join(popup.transform.DOScale(1, 1));
            showSequence.OnComplete(() => OnCompleteShow?.Invoke());
            showSequence.Play();
        }

        protected virtual void Hide()
        {
            audio.Play(Sounds.BtnClose);

            bgImg.color = new Color(0, 0, 0, 0);
            bgImg.transform.DOScale(0, 0);
            popup.transform.DOScale(0, 0);
        }
    }
}