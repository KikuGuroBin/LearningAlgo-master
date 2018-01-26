using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;

namespace LearningAlgo
{
    /// <summary>
    /// 線を描画するためのレイアウト。
    /// 
    /// 線の描画は内部クラスで行っている。
    /// </summary>
    public class LineCanvas : RelativeLayout
    {
        /// <summary>
        /// 生成した線を管理するリスト。
        /// </summary>
        public List<LineCanvas.Line> Lines = new List<LineCanvas.Line>();

        /// <summary>
        /// The drug flag.
        /// </summary>
        public bool DrugFlag;

        /// <summary>
        /// The drug.
        /// </summary>
        public EventHandler<DrugEventArgs> Drug = (sender, e) => {};

        /// <summary>
        /// しっぽから頭を繋ぐ線を描画するフロント。
        /// </summary>
        /// <returns>
        /// 生成したLineCanvas.Lineインスタンス。
        /// </returns>
        public LineCanvas.Line Tail(params View[] views)
        {
            /* 各BoxViewのX,Y座標格納する構造体を列挙するリスト */
            var list = new List<BoxView>();

            for (var i = 0; i < 5; i++)
            {
                /* 線の役割を持ったBoxView生成 */
                var box = new BoxView
                {
                    BackgroundColor = Color.Black,
                };

                list.Add(box);

                /* 制約付き子View登録 */
                this.AppendView(box);
            }

            /* 
             * 引数によって描画の方式を変えるので、
             * LineCanvas.Lineクラスのコンストラクタへの引数を変える。
             */
            var line = views.Length == 2 ?
                new LineCanvas.Line(views[0], views[1], list, true) :
                new LineCanvas.Line(list, true);

            Lines.Add(line);

            return line;
        }

        /// <summary>
        /// 右端または左端から頭をつなぐ線を描画するフロント。
        /// </summary>
        /// <param name="views">線でつなぎたいViewインスタンス</param>
        /// <returns>生成したLineCanvas.Lineインスタンス</returns>
        public LineCanvas.Line Side(bool dir, params View[] views)
        {
            /* 各BoxViewのX,Y座標格納する構造体を列挙するリスト */
            var list = new List<BoxView>();

            for (var i = 0; i < 5; i++)
            {
                /* 線の役割を持ったBoxView生成 */
                var box = new BoxView
                {
                    BackgroundColor = Color.Black,
                    IsVisible = i < 4,
                };

                list.Add(box);

                /* 制約付きで子View登録 */
                this.AppendView(box);
            }

            var line = views.Length == 2 ?
                new LineCanvas.Line(views[0], views[1], list, false) :
                new LineCanvas.Line(list, false);

            line.Direct = dir;

            Lines.Add(line);

            return line;
        }

        public void AppendView(View view)
        {
            this.Children.Add(view,
                Constraint.RelativeToParent((p) =>
                {
                    return view.X;
                }),
                Constraint.RelativeToParent((p) =>
                {
                    return view.Y;
                }),
                Constraint.RelativeToParent((p) =>
                {
                    return view.Width;
                }),
                Constraint.RelativeToParent((p) =>
                {
                    return view.Height;
                })
            );
        }

        /// <summary>
        /// 引数で指定したViewインスタンスとつながっているLineCanvas.Lineインスタンスを返す。
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public LineCanvas.Line[] SearchLines(View view)
        {
            return Lines.Where(l => view == l.PreviousView || view == l.BehideView)
                        .Select(l => l)
                        .ToArray();
        }

        /// <summary>
        /// 指定されたViewインスタンスにつながっているLineCanvas.Lineインスタンスを削除する。
        /// </summary>
        /// <param name="view">削除するViewインスタンス。</param>
        public void DeleteLine(View view)
        {
            var lines = SearchLines(view);

            DeleteLine(lines);
        }

        /// <summary>
        /// 引数で渡されたLineCanvas.Lineインスタンスと一致するインスタンスを
        /// フィールドLinesから削除し、
        /// 線の描画用に使用していたBoxViewインスタンスをレイアウトから消す
        /// </summary>
        /// <param name="lines">削除するLineCanvas.Lineインスタンス。</param>
        public void DeleteLine(params LineCanvas.Line[] lines)
        {
            foreach (var line in lines)
            {
                var list = line.Lines;

                foreach (var box in list)
                {
                    this.Children.Remove(box);
                }

                Lines.Remove(line);
            }
        }

        /// <summary>
        /// 線の描画を行う内部クラス。
        /// 
        /// 描画に関する詳細はdocument.htmlを参照。
        /// </summary>
        public class Line
        {
            /// <summary>
            /// 線の幅。
            /// </summary>
            private const int linePixcel = 2;

            /// <summary>
            /// インスタンスのマージン。
            /// </summary>
            private const int viewMargin = 20;

            /// <summary>
            /// 描画する線の出発点とつなげるViewインスタンス。
            /// </summary>
            public View PreviousView;

            /// <summary>
            /// 描画する線の末尾とつなげるViewインスタンス。
            /// </summary>
            public View BehideView;

            /// <summary>
            /// 描画用BoxViewを管理するリスト。
            /// </summary>
            public List<BoxView> Lines;

            /// <summary>
            /// <para><seealso cref="DrawTail" />か</para>
            /// <para><seealso cref="DrawSide"/></para>
            /// のどちらかで描画したかを判定する。
            /// <para>真ならTail方式。偽ならSide方式が適応される。</para>
            /// </summary>
            private bool WhichDraw;

            /// <summary>
            /// 方向フラグ。<seealso cref="DrawSide" />で使用する。
            /// </summary>
            public bool Direct;

            /// <summary>
            /// コンストラクタ。
            /// 描画用BoxViewを代入する。
            /// </summary>
            /// <param name="args">線を表現するBoxViewインスタンス</param>
            public Line(List<BoxView> args, bool which)
            {
                WhichDraw = true;

                Lines = args;
            }

            /// <summary>
            /// コンストラクタのオーバーロード。
            /// <para><seealso cref="PreviousView" />と</para>
            /// <para><seealso cref="BehideView" /></para>
            /// に引数を代入する。
            /// </summary>
            /// <param name="a">描画する線の出発点とつなげるViewインスタンス</param>
            /// <param name="b">描画する線の末尾とつなげるViewインスタンス</param>
            /// <param name="args">線を表現するBoxViewインスタンス</param>
            public Line(View a, View b, List<BoxView> args, bool which) : this(args, which)
            {
                PreviousView = a;
                BehideView = b;
            }

            /// <summary>
            /// <para><seealso cref="Draw(View, View)" />のオーバーロード。</para>
            /// フィールドを使用して線を描画する。
            /// </summary>
            public void Draw()
            {
                Draw(PreviousView, BehideView);
            }

            /// <summary>
            /// 引数で指定した2つViewインスタンスを
            /// <para><seealso cref="DrawTail" /></para>
            /// <para><seealso cref="DrawSide"/></para>
            /// のどちらかで線を描画するためのフロント。
            /// </summary>
            /// <param name="a">描画する線の出発点とつなげるViewインスタンス</param>
            /// <param name="b">描画する線の末尾とつなげるViewインスタンス</param>
            public void Draw(View a, View b)
            {
                if (WhichDraw)
                {
                    DrawTail(a, b);
                }
                else
                {
                    DrawSide(a, b, Direct);
                }
            }

            /// <summary>
            /// しっぽから頭をつなぐ線を描画する。
            /// </summary>
            /// <param name="a">しっぽから線を描画するViewインスタンス。</param>
            /// <param name="b">頭に線をつなぐViewインスタンス。</param>
            public void DrawTail(View a, View b)
            {
                /* AインスタンスのしっぽのY座標を取得 */
                var aty = a.Y + a.Height;

                /* Bインスタンスの頭のY座標を取得 */
                var bhy = b.Y;

                /* 両インスタンスの中心のX座標を取得 */
                var acx = a.X + a.Width / 2;
                var bcx = b.X + b.Width / 2;

                /* 位置関係を判定 */
                if (bhy - aty > viewMargin)
                {
                    /* ================ パターンa,bの描画 ================ */

                    /* 両インスタンスの間隔を計算 */
                    var both = (bhy - aty) / 2;

                    var same = acx - bcx == 0;

                    /* 線の描画 */
                    var box1 = Lines[0];
                    box1.IsVisible = true;

                    var box2 = Lines[1];
                    box2.IsVisible = false;

                    var box3 = Lines[2];

                    var box4 = Lines[3];
                    box4.IsVisible = false;

                    var box5 = Lines[4];

                    if (same)
                    {
                        Move(box1,
                            acx - linePixcel / 2,
                            aty,
                            linePixcel,
                            bhy - aty - linePixcel / 2
                        );

                        box3.IsVisible = false;

                        box5.IsVisible = false;
                    }
                    else
                    {
                        Move(box1,
                            acx - linePixcel / 2,
                            aty,
                            linePixcel,
                            both - linePixcel / 2
                        );

                        box3.IsVisible = true;

                        Move(box3,
                            Math.Min(acx, bcx) - linePixcel / 2,
                            aty + both - linePixcel / 2,
                            Math.Abs(bcx - acx) + linePixcel,
                            linePixcel
                        );

                        box5.IsVisible = true;

                        Move(box5,
                            bcx - linePixcel / 2,
                            bhy - both + linePixcel / 2,
                            Math.Abs(bcx - acx) + linePixcel,
                            linePixcel
                        );
                    }
                }
                else
                {
                    /* ================ c,dパターンの描画 ================ */

                    /* Bインスタンスの頭のX座標取得 */
                    var bhx = b.X;

                    /* 両インスタンスの間のX座標 */
                    var bothx = Math.Min(bcx, acx) + Math.Abs(bcx - acx) / 2;

                    /* 両インスタンスのしっぽの位置関係を計算 */
                    var diy = bhy + b.Height - aty;

                    var box1 = Lines[0];
                    box1.IsVisible = true;

                    Move(box1,
                        acx - linePixcel / 2,
                        aty,
                        linePixcel,
                        viewMargin
                    );

                    /* 両インスタンスの領域の差分の絶対値を計算 */
                    var abs = Math.Abs(bcx - acx);

                    /* box2の幅を計算 */
                    var box2w = abs - a.Width / 2 - b.Width / 2 > viewMargin * 2 ? abs / 2 : abs + b.Width / 2 + viewMargin;

                    var box2 = Lines[1];
                    box2.IsVisible = true;

                    Move(box2,
                        acx - (bcx - acx > 0 ? 0 : box2w) - linePixcel / 2,
                        aty + box1.Height - linePixcel / 2,
                        box2w,
                        linePixcel
                    );

                    /* box3のX座標を計算 */
                    var box3x = acx + (bcx - acx > 0 ? box2w : -box2w) - linePixcel / 2;

                    var box3 = Lines[2];
                    box3.IsVisible = true;

                    Move(box3,
                        box3x,
                        bhy - viewMargin - linePixcel / 2,
                        linePixcel,
                        aty - bhy - viewMargin * 2
                    );

                    var work = bcx - acx;

                    /* 両インスタンスの領域の差分を計算 */
                    var region = abs - a.Width / 2 - b.Width / 2;

                    var box4 = Lines[3];
                    box4.IsVisible = true;

                    Move(box4,
                        (region > viewMargin * 2 && work <= 0) || (region <= viewMargin * 2 && work > 0) ? bcx - linePixcel / 2 : box3x,
                        bhy - viewMargin - linePixcel / 2,
                        Math.Abs(bcx - box3x),
                        linePixcel
                    );

                    var box5 = Lines[4];
                    box5.IsVisible = true;

                    Move(box5,
                        bcx,
                        bhy - viewMargin - linePixcel / 2,
                        linePixcel,
                        viewMargin
                    );
                }
            }

            /// <summary>
            /// 右端または左端から頭を繋ぐ線を描画する。
            /// </summary>
            /// <param name="a">右端またはに左端から線を描画するViewインスタンス。</param>
            /// <param name="b">頭に線をつなぐViewインスタンス。</param>
            /// <param name="dir">方向フラグ。</param>
            public void DrawSide(View a, View b, bool dir = true)
            {
                /* Aインスタンスの右端または、左端のX, Y座標取得 */
                var aex = a.X + (dir ? a.Width : 0);
                var aey = a.Y + a.Height / 2;

                /* Bインスタンスの中心のX座標取得 */
                var bcx = b.X + b.Width / 2;

                /* Bインスタンスの頭のY座標を取得 */
                var bhy = b.Y;

                if (bhy - aey > viewMargin)
                {
                    /* ================ e,fパターンの描画 ================ */

                    /* Aインスタンスの中心のX座標を計算 */
                    var acx = a.X + a.Width / 2;

                    var work = aex - bcx;

                    /* Aインスタンスの端とBインスタンスの中心のX座標の差分を計算 */
                    var border = aex + (dir ? 1 : -1) * viewMargin - bcx;

                    /* 両インスタンスの領域の差分を計算 */
                    var region = Math.Abs(bcx - aex) + (bcx - aex > 0 ? -1 : 1) * (b.Width / 2);

                    var box1 = Lines[0];
                    box1.IsVisible = true;

                    var box2 = Lines[1];

                    var box3 = Lines[2];

                    var box4 = Lines[3];
                    box4.IsVisible = true;

                    if ((dir && border > 0) || (!dir && border <= 0))
                    {
                        Move(box1,
                            aex - (dir ? 0 : viewMargin),
                            aey - linePixcel / 2,
                            linePixcel,
                            viewMargin
                        );

                        var both = (bhy - aey + a.Height / 2) / 2;

                        box2.IsVisible = true;

                        Move(box2,
                            aex + (dir ? 1 : -1) * viewMargin - linePixcel / 2,
                            aey - linePixcel / 2,
                            linePixcel,
                            both
                        );

                        var box3w = Math.Abs(bcx - aex) + viewMargin;

                        box3.IsVisible = true;

                        Move(box3,
                            bcx - (work > 0 ? 0 : box3w) - linePixcel / 2,
                            aey + both - linePixcel / 2,
                            Math.Abs(bcx - aex) + viewMargin,
                            linePixcel
                        );

                        Move(box4,
                            bcx - linePixcel / 2,
                            aey + both - linePixcel / 2,
                            linePixcel,
                            bhy - (aey + both)
                        );
                    }
                    else
                    {
                        Move(box1,
                            aex - (dir ? 0 : work),
                            aey - linePixcel / 2,
                            Math.Abs(work),
                            viewMargin
                        );

                        box2.IsVisible = false;

                        box3.IsVisible = false;

                        Move(box4,
                            bcx - linePixcel / 2,
                            aey - linePixcel / 2,
                            linePixcel,
                            bhy - aey
                        );
                    }
                }
                else
                {
                    /* ================ g,hパターンの描画 ================ */

                    /* Aインスタンスの中心のX座標を取計算 */
                    var acx = a.X + a.Width / 2;

                    /* 両インスタンスの領域の差分の絶対値を計算 */
                    var abs = Math.Abs(bcx - acx);

                    /* box1の幅 */
                    var box1w = 0.0;

                    /* box1wの計算 */
                    if ((dir && bcx - acx <= 0) || (!dir && bcx - acx > 0))
                    {
                        box1w = viewMargin;
                    }
                    else
                    {
                        if (abs - a.Width / 2 - b.Width / 2 > viewMargin * 2)
                        {
                            box1w = abs / 2;
                        }
                        else
                        {
                            box1w = abs + b.Width / 2 + viewMargin;
                        }

                        box1w -= a.Width / 2;
                    }

                    var box1 = Lines[0];
                    box1.IsVisible = true;

                    Move(box1,
                        aex - (dir ? 0 : box1w),
                        aey - linePixcel / 2,
                        box1w,
                        linePixcel
                    );

                    /* box2のX座標を計算 */
                    var box2x = aex + (dir ? box1w : -box1w) - linePixcel / 2;

                    var box2 = Lines[1];
                    box2.IsVisible = true;

                    Move(box2,
                        box2x,
                        bhy - viewMargin - linePixcel / 2,
                        linePixcel,
                        aey - bhy + viewMargin
                    );

                    var work = bcx - acx;

                    /* 両インスタンスの領域の差分を計算 */
                    var region = Math.Abs(bcx - aex) + (bcx - aex > 0 ? -1 : 1) * (b.Width / 2);

                    var box3 = Lines[2];
                    box3.IsVisible = true;

                    Move(box3,
                        (region > viewMargin * 2) ? bcx - linePixcel / 2 : box2x,
                        bhy - viewMargin - linePixcel / 2,
                        Math.Abs(bcx - box2x),
                        linePixcel
                    );

                    var box4 = Lines[3];
                    box4.IsVisible = true;

                    Move(box3,
                        bcx - linePixcel / 2,
                        bhy - viewMargin - linePixcel / 2,
                        linePixcel,
                        viewMargin
                    );
                }
            }

            /// <summary>
            /// 線の種類を変更する。
            /// </summary>
            public void SwitchLine()
            {
                if (WhichDraw)
                {
                    Lines[4].IsVisible = false;
                }

                /* 種類フラグ反転 */
                WhichDraw = !WhichDraw;

                /* 描画メソッドを呼び出して線の更新 */
                Draw();
            }

            /// <summary>
            /// 非同期でViewインスタンスを動かす。
            /// </summary>
            /// <param name="view">指定した座標まで動かすViewインスタンス。</param>
            /// <param name="bounds">目的地の座標とViewインスタンスのサイズ。</param>
            private async void Move(View view, params double[] bounds)
            {
                var rc = view.Bounds;
                rc.X = bounds[0];
                rc.Y = bounds[1];
                rc.Width = bounds[2];
                rc.Height = bounds[3];

                await view.LayoutTo(rc, 0);
            }
        }
    }
}