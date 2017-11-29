using System;
namespace LearningAlgo
{
    /// <summary>
    /// 画像ドラッグ時にレンダラーからのコールバックに用いる
    /// </summary>
    public class ImageDrugEventArgs
    {
        /// <summary>
        /// MyImageインスタンスのX座標
        /// </summary>
        public double X;

        /// <summary>
        /// MyImageインスタンスのY座標
        /// </summary>
        public double Y;

        public ImageDrugEventArgs(object sender, double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
