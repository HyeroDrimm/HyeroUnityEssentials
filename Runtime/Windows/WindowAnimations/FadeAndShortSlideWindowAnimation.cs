using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HyeroUnityEssentials.WindowSystem
{
    public class FadeAndShortSlideWindowAnimation : WindowAnimation
    {
        private Vector2 showPos;
        private Vector2 hidePos;
        private Vector2 distance;

        public FadeAndShortSlideWindowAnimation(CanvasGroup canvasGroup, RectTransform rectTransform, Vector2 distance)
            : base(canvasGroup, rectTransform)
        {
            this.distance = distance;
            showPos = rectTransform.anchoredPosition;
            hidePos = rectTransform.anchoredPosition + distance;
        }

        public override void SetupShow()
        {
            rectTransform.anchoredPosition = hidePos;

            canvasGroup.alpha = 0f;
        }

        public override void SetupHide()
        {
            rectTransform.anchoredPosition = showPos;

            canvasGroup.alpha = 1f;
        }

        public override void DoShow(float t)
        {
            var transformedT = tTransformFunc(t);
            canvasGroup.alpha = transformedT;
            rectTransform.anchoredPosition = Vector2.Lerp(hidePos, showPos, transformedT);
        }

        public override void DoHide(float t)
        {
            var transformedT = tTransformFunc(t);
            canvasGroup.alpha = 1 - transformedT;
            rectTransform.anchoredPosition = Vector2.Lerp(showPos, hidePos, transformedT);
        }

        public override void EndShow() { }

        public override void EndHide() { }
    }
}
