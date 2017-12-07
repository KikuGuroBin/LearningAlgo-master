using System;
using System.Collections.Generic;
using LearningAlgo;
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
        /// 初回処理用フラグ
        /// </summary>
        private bool First = true;

        /// <summary>
        /// 図形選択メニューの表示、非表示フラグ
        /// </summary>
        private bool SidePaneShowing;

        public FlowPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 画面サイズが変更されたときのイベント
        /// </summary>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            Shadow.Opacity = 0;
            Dialog.Opacity = 0;
            SidePane.Opacity = 0;

            /* 初回限定の処理 */
            if (First)
            {
                /* ダイアログ関係のインスタンスを隠す */
                Shadow.TranslateTo(0, height, 0);
                Shadow.WidthRequest = width;
                Shadow.HeightRequest = height;

                Dialog.TranslateTo(width / 2, height, 0);
                Dialog.WidthRequest = DIALOGSIZE;
                Dialog.HeightRequest = DIALOGSIZE;

                /* フローチャートのレイアウト部分を配置 */
                FlowScroller.TranslateTo(0, 0, 0);
                FlowScroller.WidthRequest = width;
                FlowScroller.HeightRequest = height / 10 * 9;

                /* フッターメニューを配置 */
                Footer.TranslateTo(0, height / 10 * 9, 0);
                Footer.WidthRequest = width;
                Footer.HeightRequest = height / 10;

                /* 図形選択メニューを隠す */
                SidePane.TranslateTo(-(width / 4), 0, 0);
                SidePane.WidthRequest = width / 4;
                SidePane.HeightRequest = height - height / 10;

                /* カスタムダイアログ表示制御用インスタンスに格納 */
                ImitationDialog = new ImitationDialog
                {
                    Shadow = this.Shadow,
                    Dialog = this.Dialog,
                };

                First = false;
            }
            else
            {
                /* 図形選択メニューを隠す */
                //SidePane.LayoutTo(new Rectangle(-(width / 4), 0, width / 4, height), 200, Easing.CubicOut);

                //SidePaneShowing = !SidePaneShowing;
            }

            SidePane.Opacity = 1;
        }

        /// <summary>
        /// 図形選択メニューの表示、非表示する
        /// </summary>
        private async void SidePanelShow()
        {
            /* 表示フラグ反転 */
            SidePaneShowing = !SidePaneShowing;

            /* フラグによって表示、非表示にする */
            if (SidePaneShowing)
            {
                /* 表示 */
                await SidePane.TranslateTo(0, 0, 200, Easing.CubicIn);
            }
            else
            {
                /* 非表示 */
                await SidePane.TranslateTo(-(Width / 4), 0, 200, Easing.CubicOut);
            }
        }

        /// <summary>
        /// フローチャートのパネルをスライドさせる
        /// </summary>
        private async void FlowScrollerSlide()
        {
            /* フラグによってスライドの方向を変える */
            if (SidePaneShowing)
            {
                /* 右へスライド */
                await FlowScroller.TranslateTo(Width / 4, 0, 200, Easing.CubicIn);
                                              // Width - Width / 4, Height - Height / 10)
            }
            else
            {
                /* 左へスライド */
                await FlowScroller.TranslateTo(0, 0, 200, Easing.CubicOut);
                //Width, Height - Height / 10
            }
        }

        /* サイドラベルタップ用のイベント */
        private void SidePanel(object sender, EventArgs args)
        {
            SidePanelShow();
            FlowScrollerSlide();
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
            /* 選択された画像を取得 */
            var source = (sender as Image).Source;

            /* ドラッグ可能なImageインスタンス生成 */
            var myImage = new MyImage
            {
                TranslationX = 0,
                TranslationY = ImagePosition,
                WidthRequest = 50,
                HeightRequest = 50,
                Source = source,
            };

            /* フローチャートのパネルに画像を追加 */
            FlowPanel.Children.Add(myImage);

            /* レイアウトでの最後尾の座標を更新 */
            ImagePosition += 50;
        }
    }
}
