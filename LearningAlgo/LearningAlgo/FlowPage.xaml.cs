using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LearningAlgo
{
    /// <summary>
    /// フローチャートの生成、実行画面
    /// </summary>
    public partial class FlowPage : ContentPage
    {
        /// <summary>
        /// カスタムダイアログサイズの定数
        /// </summary>
        private const int DIALOGSIZE = 300;

        /// <summary>
        /// カスタムダイアログの表示アニメーションを取り扱う
        /// </summary>
        private ImitationDialog ImitationDialog;

        /// <summary>
        /// フローチャート内の最後尾の図形の座標
        /// </summary>
        private double ImagePosition;

        /// <summary>
        /// OnSizeAllocated制御用
        /// </summary>
        private bool FirstOrder = true;

        private double SidePanelHeight;

        /// <summary>
        /// 図形選択メニューの表示、非表示フラグ
        /// </summary>
        private bool SidePaneShowing;

        public FlowPage()
        {
            InitializeComponent();

            /* iOSだけ、上部に余白をとる */
            /* Padding = new Thickness(0, Device.RuntimePlatform == Device.iOS ? 20 : 0, 0, 0);
            アイテムをImagesourceFormPageで参照できるようにメンバ変数に格納 */

            // DBにする配列達
            string[] FlowTable = new string[3];
            string[] PrintTable = new string[4];
            string[] KindTable = new string[3];

            // 変数を格納したDBからDictionaryにぶち込む
            Dictionary<string, int> VarManegement = new Dictionary<string, int>();
            VarManegement["i"] = 3;
            VarManegement["j"] = 5;
            string Shiki;

            /*
             * □
             Shiki = "1＋i＋3×j＋i→i";
            SquareCalculatClass squareCalculat =new SquareCalculatClass();
            VarManegement = squareCalculat.SquareCalculate(VarManegement,Shiki);
            System.Diagnostics.Debug.WriteLine("ここ一番で決める:"+VarManegement["i"].ToString());
            */


            /*
             * ♢
            Shiki = "1＋i＋3×j＋i≧3＋4";
            //Symbolは0がNo、1がYes、：が判定
            DiamondCalculatClass diamondCalculat = new DiamondCalculatClass();
            (string Symbol,int b,int c) Kekka = diamondCalculat.DiamondCalculat(VarManegement, Shiki);
            */

            /*
             * ♢
            Shiki = "1＋i＋3×j＋i≧3＋4";
            //Symbolは0がNo、1がYes、：が判定
            DiamondCalculatClass diamondCalculat = new DiamondCalculatClass();
            bool Kekka = diamondCalculat.DiamondCalculat(VarManegement, Shiki);
            */


        }

        /// <summary>
        /// 画面サイズが変更されたときのイベント
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            /* 初回だけ処理を行う */
            if (FirstOrder)
            {
                SidePanelHeight = SidePane.Height / 3 * 2;

                SidePane.LayoutTo(new Rectangle(-SidePane.Width, 0, SidePane.Width / 3 * 2, SidePanelHeight));

                /* フローチャートのレイアウト部分のサイズ、座標 */
                FlowScroller.LayoutTo(new Rectangle(0, 0, width, height / 10 * 9), 0);
                FlowPanel.LayoutTo(new Rectangle(0, 0, width, height / 10 * 9), 0);

                UnderMenu.LayoutTo(new Rectangle(0, height / 10 * 9, width, height / 10), 0);

                /* ダイアログ関係のインスタンスを隠す */
                Shadow.LayoutTo(new Rectangle(0, height, width, height), 0);
                Dialog.LayoutTo(new Rectangle(200, height, width / 2 - 100, 100), 0);
                ImitationDialog = new ImitationDialog
                {
                    Shadow = this.Shadow,
                    Dialog = this.Dialog,
                };



                FlowPanel.Children.Add(new Label{Text = "label"}, () => new Rectangle(0, 300, 50, 50));


                /* 次回以降呼ばれないようにする */
                FirstOrder = false;
            }
        }

        /// <summary>
        /// 図形選択メニューの表示、非表示する
        /// </summary>
        private async void SideAnimatePanel()
        {
            /* 表示フラグ反転 */
            SidePaneShowing = !SidePaneShowing;

            /* フラグによって表示、非表示にする */
            if (SidePaneShowing)
            {
                /* 表示 */
                await SidePane.LayoutTo(new Rectangle(0, 0, SidePane.Width, SidePanelHeight), 100, Easing.CubicIn);
            }
            else
            {
                /* 非表示 */
                await SidePane.LayoutTo(new Rectangle(0 - SidePane.Width, 0, SidePane.Width, SidePanelHeight), 50, Easing.CubicOut);
            }
        }

        /* サイドラベルタップ用のイベント */
        private void SidePanel(object sender, EventArgs args)
        {
            SideAnimatePanel();
        }

        private void ShowDialogClicked(object sender, EventArgs args)
        {
            /* カスタムダイアログ表示 */
            ImitationDialog.ShowUp(300);
        }

        private void ShadowTapped(object sender, EventArgs args)
        {
            ImitationDialog.Hide();
        }

        /// <summary>
        /// 図形選択メニューで図形が選択された時のイベント
        /// </summary>
        /// <param name="sender">
        /// 発火したインスタンス。
        /// 基本的にはImageが来る
        /// </param>
        /// <param name="args">
        /// Arguments.
        /// </param>
        private void ItemTapped(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("deg : FlowPage.ItemTapped");

            /* 選択された画像を取得 */
            var source = (sender as Image).Source;

            /* ドラッグ可能なImageインスタンス生成 */
            var myImage = new MyImage
            {
                Source = source,
            };

            /* フローチャートのパネルに画像を追加 */
            FlowPanel.Children.Add(myImage, () => new Rectangle(0, 0, 10, 10));

            /* レイアウトでの最後尾の座標を更新 */
            ImagePosition += myImage.HeightRequest;
        }
    }
}
