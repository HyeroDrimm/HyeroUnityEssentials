using System;
using UnityEngine;

namespace HyeroUnityEssentials.WindowSystem
{
    public abstract class WindowAnimation
    {
        protected RectTransform rectTransform;
        protected CanvasGroup canvasGroup;
        protected Func<float, float> tTransformFunc;

        protected WindowAnimation(CanvasGroup canvasGroup, RectTransform rectTransform, Func<float,float> tTransformFunc)
        {
            this.tTransformFunc = tTransformFunc;
            this.rectTransform = rectTransform;
            this.canvasGroup = canvasGroup;
        }

        protected WindowAnimation(CanvasGroup canvasGroup, RectTransform rectTransform, TransformFunction transformFunction = TransformFunction.EaseInOut)
        {
            this.tTransformFunc = TransformFunctionsToFunc(transformFunction);
            this.rectTransform = rectTransform;
            this.canvasGroup = canvasGroup;
        }


        public abstract void SetupShow();
        public abstract void SetupHide();

        public abstract void DoShow(float t);
        public abstract void DoHide(float t);

        public abstract void EndShow();
        public abstract void EndHide();


        protected float EaseInOut(float t) => t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;

        protected float Linear(float t) => t;

        private Func<float, float> TransformFunctionsToFunc(TransformFunction transformFunction)
        {
            return transformFunction switch
            {
                TransformFunction.Linear => Linear,
                TransformFunction.EaseInOut => EaseInOut,
                _ => throw new ArgumentOutOfRangeException(nameof(transformFunction), transformFunction, null)
            };
        }

        public enum TransformFunction
        {
            Linear,
            EaseInOut,
        }
    }
}
