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
        /// カスタムダイアログの表示アニメーションを取り扱う
        /// </summary>
        private ImitationDialog ImitationDialog;

        /// <summary>
        /// 
        /// </summary>
        private double FlowPosition;

        /// <summary>
        /// OnSizeAllocated制御用
        /// </summary>
        private bool FirstOrder = true;

        private double SidePanelHeight;

        /// <summary>
        /// 図形選択メニューの表示、非表示フラグ
        /// </summary>
        private bool PanelShowing;

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

        /* 画面サイズが変更されたときのイベント */
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            /* 初回だけ処理を行う */
            if (FirstOrder)
            {
                SidePanelHeight = SidePane.Height / 3 * 2;

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
            PanelShowing = !PanelShowing;

            /* フラグによって表示、非表示にする */
            if (PanelShowing)
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
            ImitationDialog.ShowUp(300);
        }

        private void ItemTapped(object sender, EventArgs args)
        {
            var image = sender as Image;

            /* フローチャートのパネルに画像を追加 */
            FlowPanel.Children.Add(image, () => new Rectangle(FlowPosition, 0, image.WidthRequest, image.HeightRequest));

            /* レイアウトでの最後尾の座標を更新 */
            FlowPosition += image.HeightRequest;
        }
    }
}
