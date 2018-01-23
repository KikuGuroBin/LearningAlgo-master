using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearningAlgo
{

    /// <summary>
    /// LineCanvasのデモ用
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LineCanvasDemo : ContentPage
    {
        /// <summary>
        /// 描画した線の管理用。
        /// リストが望ましい。
        /// </summary>
        List<LineCanvas.Line> Lines = new List<LineCanvas.Line>();

        bool First;

        public LineCanvasDemo()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            Canvas.WidthRequest = width;
            Canvas.HeightRequest = height;

            Main.WidthRequest = width;
            Main.HeightRequest = height;

            if (First)
            {
                return;
            }

            First = true;

            /* ======================== LineCanvasについて ================================ */

            /* 以下の2つのメソッドどちらかで線の引き方を選ぶ */

            var line = Canvas.Side();

            /* var line = Canvas.Tail(); */

            /* 管理用リストに格納 */
            Lines.Add(line);

            /* ======================== ここまで ================================ */
        }

        private void OnClick(object sender, EventArgs args)
        {
            /* ======================== LineCanvasについて ================================ */

            var line = Lines[0];

            /* 実際に線を引くときは以下のメソッドを使用するだけでいい */
            line.Draw(Box1, Box2);

            /* ======================== ここまで ================================ */
        }
    }
}