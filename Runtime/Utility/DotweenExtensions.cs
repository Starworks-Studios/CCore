using UnityEngine;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening
{
    public static class DotweenExtensions
    {
        public static Tween To(DOGetter<Vector3> getter, DOSetter<Vector3> setter, DOGetter<Vector3> endValue, float duration)
        {
            return DOTween.To(() => endValue() - getter(), (v) => setter(endValue() - v), Vector3.zero, duration);
        }
        public static Tween ToTransformPosition(this Transform moveT, Transform targetT, float duration)
        {
            return DotweenExtensions.To(() => moveT ? moveT.position : Vector3.zero, 
                (v) => {if (moveT) moveT.position = v; }, 
                () => targetT ? targetT.position : (moveT ? moveT.position : Vector3.zero), 
                duration);
        }
        //public static TweenerCore<T, T, NoOptions> To<T>(DOGetter<T> getter, DOSetter<T> setter, DOGetter<T> endValue, float duration)
        //{
        //    return DOTween.To(()=>endValue()-getter(), )
        //}

    }
}