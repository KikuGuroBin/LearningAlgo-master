using System;
using Xamarin.Forms;

namespace LearningAlgo
{
    /// <summary>
    /// カスタムダイアログの表示アニメーション関係を扱うクラス
    /// </summary>
    public class ImitationDialog
    {
        /// <summary>
        /// ダイアログ表示時の影の部分。
        /// ダイアログを呼び出す側で定義して、初期化式等でセットする必要がある
        /// </summary>
        public BoxView Shadow { get; set; }

        /// <summary>
        /// ダイアログ本体。
        /// ダイアログを呼び出す側で定義して、初期化式等でセットする必要がある
        /// </summary>
        public RelativeLayout Dialog { get; set; }

        /// <summary>
        /// ダイアログ表示アニメーションの座標軸
        /// </summary>
        private int AxisFlag;

        /// <summary>
        /// ダイアログ表示アニメーションの方向
        /// </summary>
        private int DirectionFlag;

        /// <summary>
        /// アニメーション実行フラグ
        /// </summary>
        private bool Moving;

        /// <summary>
        /// ダイアログをY軸方向で動かすアニメーション
        /// </summary>
        /// <param name="y">
        /// ダイアログを表示させるY座標
        /// </param>
        /// <param name="up">
        /// ダイアログ、上から出るか下から出るか
        /// </param>
        public async void ShowUp(double y, bool up = true)
        {
            /* ほかのアニメーション中だったら何もしない */
            if (Moving)
            {
                return;
            }

            /* アニメーションフラグを立てる */
            Moving = true;

            /* ダイアログが表示されていたら隠すアニメーションを実行 */
            if (AxisFlag != 0)
            {
                Hide();
            }

            /* 表示軸フラグと方向フラグを立てる */
            AxisFlag = 1;
            DirectionFlag = 1;

            /* ダイアログを指定座標まで移動 */
            var rc = Dialog.Bounds;
            rc.Y = y - 50;
            await Dialog.LayoutTo(rc, 0);

            /* 影表示 */
            rc = Shadow.Bounds;
            rc.Y = 0;
            await Shadow.LayoutTo(rc, 0);

            /* アニメーション */
            for (int cnt = 0, i = up ? 1 : -1; cnt < 5; cnt += i)
            {
                Dialog.Opacity += 0.2;
                Shadow.Opacity += 0.1;

                rc = Dialog.Bounds;
                rc.Y -= 10;
                await Dialog.LayoutTo(rc, 100);
            }

            /* アニメーションフラグをおろす */
            Moving = false;
        }

        /// <summary>
        /// ダイアログをX軸方向で動かすアニメーション
        /// </summary>
        /// <param name="x">
        /// ダイアログを表示させるX座標
        /// </param>
        /// <param name="slide">
        /// ダイアログ、右から出るか←から出るか
        /// </param>
        public async void ShowSlide(double x, bool slide = true)
        {
            /* ほかのアニメーション中だったら何もしない */
            if (Moving)
            {
                return;
            }

            /* アニメーションフラグを立てる */
            Moving = true;

            /* ダイアログが表示されていたら隠すアニメーションを実行 */
            if (AxisFlag != 0)
            {
                Hide();
            }

            /* 表示軸フラグと方向フラグを立てる */
            AxisFlag = 2;
            DirectionFlag = 2;

            /* ダイアログを指定座標まで移動 */
            var rc = Dialog.Bounds;
            rc.X = x;
            await Dialog.LayoutTo(rc, 0);

            /* 影表示 */
            rc = Shadow.Bounds;
            rc.Y = 0;
            await Shadow.LayoutTo(rc, 0);

            /* アニメーション */
            for (int cnt = 0, i = slide ? 1 : -1; cnt < 5; cnt += i)
            {
                Dialog.Opacity += 0.2;

                Shadow.Opacity += 0.1;

                rc = Dialog.Bounds;
                rc.X -= 10;
                await Dialog.LayoutTo(rc, 100);
            }

            /* アニメーションフラグをおろす */
            Moving = false;
        }

        /// <summary>
        /// ダイアログを隠すアニメーション
        /// </summary>
        public async void Hide()
        {
            /* ほかのアニメーション中だったら何もしない */
            if (Moving)
            {
                return;
            }

            /* アニメーションフラグを立てる */
            Moving = true;

            /* フラグ初期化 */
            AxisFlag = 0;
            DirectionFlag = 0;

            var rc = Dialog.Bounds;

            /* 隠すアニメーション */
            for (var cnt = 0; cnt < 5; cnt++)
            {
                Dialog.Opacity -= 0.2;

                Shadow.Opacity -= 0.1;

                if (AxisFlag == 1)
                {
                    rc.Y += 10;
                }
                else if (AxisFlag == 2)
                {
                    rc.X += 10;
                }

                await Dialog.LayoutTo(rc, 100);
            }

            /* 影をなくす */
            rc = Shadow.Bounds;
            rc.Y = rc.Height;
            await Shadow.LayoutTo(rc, 0);

            /* アニメーションフラグをおろす */
            Moving = false;
        }
    }
}
