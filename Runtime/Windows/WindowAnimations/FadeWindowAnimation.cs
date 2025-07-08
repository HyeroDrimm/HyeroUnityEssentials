using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HyeroUnityEssentials.WindowSystem
{
    internal class FadeWindowAnimation :WindowAnimation
    {
        public FadeWindowAnimation(CanvasGroup canvasGroup) : base(canvasGroup, null) { }

        public override void SetupShow()
        {
            canvasGroup.alpha = 0f;
        }

        public override void SetupHide()
        {
            canvasGroup.alpha = 1f;
        }

        public override void DoShow(float t)
        {
            canvasGroup.alpha = tTransformFunc(t);
        }

        public override void DoHide(float t)
        {
            canvasGroup.alpha = 1 - tTransformFunc(t);
        }

        public override void EndShow() { }

        public override void EndHide() { }
    }
}
