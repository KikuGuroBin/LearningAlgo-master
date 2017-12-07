using System;
namespace LearningAlgo
{
    /// <summary>
    /// ドラッグ時にレンダラーからのコールバックに用いる
    /// </summary>
    public class DrugEventArgs
    {
        /// <summary>
        /// インスタンスのX座標
        /// </summary>
        public double X;

        /// <summary>
        /// インスタンスのY座標
        /// </summary>
        public double Y;

        public DrugEventArgs(object sender, double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
