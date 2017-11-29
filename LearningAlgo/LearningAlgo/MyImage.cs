using System;
using Xamarin.Forms;

namespace LearningAlgo
{
    public class MyImage : Image
    {
        // インスタンスがドラッグされたときのイベント
        public void OnImageDrug(object sender, ImageDrugEventArgs args)
        {
            var rc = this.Bounds;

            // コールバックから座標取得
            rc.X += args.X;
            rc.Y += args.Y;

            // 取得した座標に移動
            this.Layout(rc);
        }
    }
}
