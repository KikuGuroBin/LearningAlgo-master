using System;

using Foundation;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using LearningAlgo;
using LearningAlgo.iOS;

[assembly: ExportRenderer(typeof(MyLayout), typeof(MyLayoutRenderer))]
namespace LearningAlgo.iOS
{
    /// <summary>
    /// MyImageインスタンスのレンダラー
    /// </summary>
    public class MyLayoutRenderer : ViewRenderer<MyLayout, UIView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<MyLayout> e)
        {
            base.OnElementChanged(e);
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            UITouch touch = touches.AnyObject as UITouch;
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            UITouch touch = touches.AnyObject as UITouch;

            /* MyImageインスタンスの現在の座標 */
            var newPoint = touch.LocationInView(this);

            /* MyImageインスタンスの前回の座標 */
            var previousPoint = touch.PreviousLocationInView(this);

            /* 差分を計算 */
            nfloat dx = newPoint.X - previousPoint.X;
            nfloat dy = newPoint.Y - previousPoint.Y;

            /* コールバック */
            var el = this.Element as MyLayout;
            el.LayoutDrug(el, new DrugEventArgs(el, dx, dy));
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
        }
    }
}
