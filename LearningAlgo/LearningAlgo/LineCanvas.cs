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
    public class LineCanvas : AbsoluteLayout
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
        public EventHandler<DrugEventArgs> Drug;

        /// <summary>
        /// しっぽから頭を繋ぐ線を描画するフロント。
        /// </summary>
        /// <returns>
        /// 生成したLineCanvas.Lineインスタンス。
        /// </returns>
        public LineCanvas.Line Tail(params View[] views)
        {
            var array = new BoxView[5];

            var list = new List<LineCanvas.LineBounds>();

            for (var i = 0; i < 5; i++)
            {
                var box = new BoxView
                {
                    BackgroundColor = Color.Black,
                };
                array[i] = box;

                var bound = new LineBounds
                {
                    LineX = 0,
                    LineY = 0,
                };
                list.Add(bound);

                this.Children.Add(box);

                /*
                this.Children.Add(box,
                    Constraint.RelativeToParent((p) => {
                        return bound.LineX;
                   s }),
                    Constraint.RelativeToParent((p) => 
                    {
                        return bound.LineY;
                    }),
                    Constraint.RelativeToParent((p) => 
                    {
                        return 0;
                    }),
                    Constraint.RelativeToParent((p) =>
                    {
                        return 0;
                    })
                );    
                */
            }

            var line = new LineCanvas.Line(array)
            {
                PreviousView = views[0],
                BehideView = views[1],
                BoundsList = list,
            };

            Lines.Add(line);

            return line;
        }

        /// <summary>
        /// 右端または左端から頭をつなぐ線を描画するフロント。
        /// </summary>
        /// <param name="a">線をつなぐインスタンス</param>
        /// <param name="b">線をつなぐインスタンス</param>
        /// <returns>
        /// 生成したLineCanvas.Lineインスタンス。
        /// </returns>
        public LineCanvas.Line Side()
        {
            var array = new BoxView[4];

            for (var i = 0; i < 4; i++)
            {
                var box = new BoxView
                {
                    BackgroundColor = Color.Black,
                    WidthRequest = 0,
                    HeightRequest = 0,
                };
                array[i] = box;

                this.Children.Add(box);
            }

            var line = new LineCanvas.Line(array)
            {
                //UpView = a,
                //DownView = b,
            };

            Lines.Add(line);

            return line;
        }

        /// <summary>
        /// 引数で指定したViewインスタンスにつながっているLineCanvas.Lineインスタンスをすべて返す。
        /// </summary>
        /// <returns>
        /// 引数で指定したViewインスタンスにつながっているLineCanvas.Lineインスタンスを列挙した配列。
        /// </returns>
        /// <param name="view">
        /// 
        /// </param>
        public LineCanvas.Line[] SearchLines(View view)
        {
            return Lines.Where(l => view == l.PreviousView || view == l.BehideView)
                        .Select(l => l)
                        .ToArray();
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
            private const int LINEPIXEL = 2;

            /// <summary>
            /// インスタンスのマージン。
            /// </summary>
            private const int VIEWMARGIN = 20;

            /// <summary>
            /// <seealso cref="DrawTail" />
            /// で描画したBoxViewを管理する配列。
            /// </summary>
            public BoxView[] TailLines = new BoxView[5];

            /// <summary>
            /// <seealso cref="DrawSide"/>
            /// で描画したBoxViewを管理する配列。
            /// </summary>
            public BoxView[] SideLines = new BoxView[4];

            /// <summary>
            /// <para><seealso cref="DrawTail" />か</para>
            /// <para><seealso cref="DrawSide"/></para>
            /// のどちらかで描画したかを判定する。
            /// </summary>
            public bool WhichDraw;

            public View PreviousView;

            public View BehideView;

            /// <summary>
            /// 線のX,Y座標を格納した構造体を列挙したリスト
            /// </summary>
            public List<LineCanvas.LineBounds> BoundsList;

            /// <summary>
            /// 方向フラグ。<seealso cref="DrawSide" />で使用する。
            /// </summary>
            public bool Direct { get; set; }

            public Line(BoxView[] args)
            {
                if (args.Length == 5)
                {
                    TailLines = args;
                    WhichDraw = true;
                }
                else if (args.Length == 4)
                {
                    SideLines = args;
                }
            }

            public void Draw()
            {
                Draw(PreviousView, BehideView);    
            }

            /// <summary>
            /// <para><seealso cref="DrawTail" /></para>
            /// <para><seealso cref="DrawSide"/></para>
            /// のどちらかで線を描画するためのフロント。
            /// </summary>
            /// <param name="a">
            /// 線でつなぐViewインスタンス。
            /// </param>
            /// <param name="b">
            /// 線でつなぐViewインスタンス。
            /// </param>
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
            /// <param name="a">
            /// しっぽから線を描画するViewインスタンス。
            /// </param>
            /// <param name="b">
            /// 頭に線をつなぐViewインスタンス。
            /// </param>
            public void DrawTail(View a, View b)
            {
                /* AインスタンスのしっぽのY座標を取得 */
                var aty = a.TranslationY + a.Height;

                /* Bインスタンスの頭のY座標を取得 */
                var bhy = b.TranslationY;

                /* 両インスタンスの中心のX座標を取得 */
                var acx = a.TranslationX + a.Width / 2;
                var bcx = b.TranslationX + b.Width / 2;

                /* 位置関係を判定 */
                if (bhy - aty > VIEWMARGIN)
                {
                    /* ======== パターンa,bの描画 ======== */

                    /* 両インスタンスの間隔を計算 */
                    var both = (bhy - aty) / 2;

                    var same = acx - bcx == 0;

                    /* 線の描画 */
                    var box1 = TailLines[0];
                    box1.TranslationX = acx;
                    box1.TranslationY = aty;
                    box1.WidthRequest = LINEPIXEL;
                    box1.HeightRequest = same ? bhy - aty : both;

                    box1.IsVisible = true;
                    // box1.LayoutTo(new Rectangle(acx, aty, LINEPIXEL, same ? bhy - aty : both), 0);

                    var box2 = TailLines[1];
                    box2.IsVisible = false;

                    var box3 = TailLines[2];

                    var box4 = TailLines[3];
                    box4.IsVisible = false;

                    var box5 = TailLines[4];

                    if (same)
                    {
                        box3.IsVisible = false;

                        box5.IsVisible = false;
                    }
                    else
                    {
                        box3.TranslationX = Math.Min(acx, bcx);
                        box3.TranslationY = aty + both;
                        box3.WidthRequest = Math.Abs(bcx - acx);
                        box3.HeightRequest = LINEPIXEL;

                        box3.IsVisible = true;
                        // box3.LayoutTo(new Rectangle(Math.Min(acx, bcx), aty + both, Math.Abs(bcx - acx), LINEPIXEL), 0);

                        box5.TranslationX = bcx;
                        box5.TranslationY = bhy - both;
                        box5.WidthRequest = LINEPIXEL;
                        box5.HeightRequest = both;

                        box5.IsVisible = true;
                        // box5.LayoutTo(new Rectangle(bcx, bhy - both, LINEPIXEL, both), 0);
                    }
                }
                else
                {
                    /* ======== c,dパターンの描画 ======== */

                    /* Bインスタンスの頭のX座標取得 */
                    var bhx = b.TranslationX;

                    /* 両インスタンスの間のX座標 */
                    var bothx = Math.Min(bcx, acx) + Math.Abs(bcx - acx) / 2;

                    /* 両インスタンスのしっぽの位置関係を計算 */
                    var diy = bhy + b.Height - aty;

                    var box1 = TailLines[0];
                    box1.TranslationX = acx;
                    box1.TranslationY = aty;
                    box1.WidthRequest = LINEPIXEL;
                    box1.HeightRequest = VIEWMARGIN;
                    box1.IsVisible = true;

                    /* 両インスタンスの領域の差分の絶対値を計算 */
                    var abs = Math.Abs(bcx - acx);

                    /* box2の幅を計算 */
                    var box2w = abs - a.Width / 2 - b.Width / 2 > VIEWMARGIN * 2 ? abs / 2 : abs + b.Width / 2 + VIEWMARGIN;

                    var box2 = TailLines[1];
                    box2.TranslationX = acx - (bcx - acx > 0 ? 0 : box2w);
                    box2.TranslationY = aty + box1.HeightRequest;
                    box2.WidthRequest = box2w;
                    box2.HeightRequest = LINEPIXEL;
                    box2.IsVisible = true;

                    /* box3のX座標を計算 */
                    var box3x = acx + (bcx - acx > 0 ? box2w : -box2w);

                    var box3 = TailLines[2];
                    box3.TranslationX = box3x;
                    box3.TranslationY = bhy - VIEWMARGIN;
                    box3.WidthRequest = LINEPIXEL;
                    box3.HeightRequest = aty - bhy + VIEWMARGIN * 2;
                    box3.IsVisible = true;

                    var work = bcx - acx;

                    /* 両インスタンスの領域の差分を計算 */
                    var region = abs - a.Width / 2 - b.Width / 2;

                    var box4 = TailLines[3];
                    box4.TranslationX = (region > VIEWMARGIN * 2 && work <= 0) || (region <= VIEWMARGIN * 2 && work > 0) ? bcx : box3x;
                    box4.TranslationY = bhy - VIEWMARGIN;
                    box4.WidthRequest = Math.Abs(bcx - box3x);
                    box4.HeightRequest = LINEPIXEL;
                    box4.IsVisible = true;

                    var box5 = TailLines[4];
                    box5.TranslationX = bcx;
                    box5.TranslationY = bhy - VIEWMARGIN + LINEPIXEL;
                    box5.WidthRequest = LINEPIXEL;
                    box5.HeightRequest = VIEWMARGIN;
                    box5.IsVisible = true;
                }
            }

            /// <summary>
            /// 右端または左端から頭を繋ぐ線を描画する。
            /// </summary>
            /// <param name="a">
            /// 右端またはに左端から線を描画するViewインスタンス。
            /// </param>
            /// <param name="b">
            /// 頭に線をつなぐViewインスタンス。
            /// </param>
            /// <param name="dir">
            /// 方向フラグ。
            /// </param>
            public void DrawSide(View a, View b, bool dir = true)
            {
                /* Aインスタンスの右端または、左端のX, Y座標取得 */
                var aex = a.TranslationX + (dir ? a.Width : 0);
                var aey = a.TranslationY + a.Height / 2;

                /* Bインスタンスの中心のX座標取得 */
                var bcx = b.TranslationX + b.Width / 2;

                /* Bインスタンスの頭のY座標を取得 */
                var bhy = b.TranslationY;

                if (bhy - aey > VIEWMARGIN)
                {
                    /* ======== e,fパターンの描画 ======== */

                    /* Aインスタンスの中心のX座標を計算 */
                    var acx = a.TranslationX + a.Width / 2;

                    var work = aex - bcx;

                    /* Aインスタンスの端とBインスタンスの中心のX座標の差分を計算 */
                    var border = aex + (dir ? 1 : -1) * VIEWMARGIN - bcx;

                    /* 両インスタンスの領域の差分を計算 */
                    var region = Math.Abs(bcx - aex) + (bcx - aex > 0 ? -1 : 1) * (b.Width / 2);

                    var box1 = SideLines[0];
                    box1.TranslationY = aey;
                    box1.HeightRequest = LINEPIXEL;
                    box1.IsVisible = true;

                    var box2 = SideLines[1];

                    var box3 = SideLines[2];

                    var box4 = SideLines[3];
                    box4.TranslationX = bcx;
                    box4.WidthRequest = LINEPIXEL;

                    if ((dir && border > 0) || (!dir && border <= 0))
                    {
                        box1.TranslationX = aex - (dir ? 0 : VIEWMARGIN);
                        box1.WidthRequest = VIEWMARGIN;

                        var both = (bhy - aey + a.Height / 2) / 2;

                        box2.TranslationX = aex + (dir ? 1 : -1) * VIEWMARGIN;
                        box2.TranslationY = aey;
                        box2.WidthRequest = LINEPIXEL;
                        box2.HeightRequest = both;

                        var box3w = Math.Abs(bcx - aex) + VIEWMARGIN;

                        box3.TranslationX = bcx - (work > 0 ? 0 : box3w);
                        box3.TranslationY = aey + both;
                        box3.WidthRequest = Math.Abs(bcx - aex) + VIEWMARGIN;
                        box3.HeightRequest = LINEPIXEL;

                        box4.TranslationY = aey + both;
                        box4.HeightRequest = bhy - (aey + both);
                    }
                    else
                    {
                        box1.TranslationX = aex - (dir ? 0 : work);
                        box1.WidthRequest = Math.Abs(work);

                        box2.IsVisible = false;

                        box3.IsVisible = false;

                        box4.TranslationY = aey;
                        box4.HeightRequest = bhy - aey;
                    }
                }
                else
                {
                    /* Aインスタンスの中心のX座標を取計算 */
                    var acx = a.TranslationX + a.Width / 2;

                    /* 両インスタンスの領域の差分の絶対値を計算 */
                    var abs = Math.Abs(bcx - acx);

                    /* box1の幅 */
                    var box1w = 0.0;

                    /* box1wの計算 */
                    if ((dir && bcx - acx <= 0) || (!dir && bcx - acx > 0))
                    {
                        box1w = VIEWMARGIN;
                    }
                    else
                    {
                        if (abs - a.Width / 2 - b.Width / 2 > VIEWMARGIN * 2)
                        {
                            box1w = abs / 2;
                        }
                        else
                        {
                            box1w = abs + b.Width / 2 + VIEWMARGIN;
                        }

                        box1w -= a.Width / 2;
                    }

                    var box1 = SideLines[0];
                    box1.TranslationX = aex - (dir ? 0 : box1w);
                    box1.TranslationY = aey;
                    box1.WidthRequest = box1w;
                    box1.HeightRequest = LINEPIXEL;

                    /* box2のX座標を計算 */
                    var box2x = aex + (dir ? box1w : -box1w);

                    var box2 = SideLines[1];
                    box2.TranslationX = box2x;
                    box2.TranslationY = bhy - VIEWMARGIN;
                    box2.WidthRequest = LINEPIXEL;
                    box2.HeightRequest = aey - bhy + VIEWMARGIN;
                    box2.IsVisible = true;

                    var work = bcx - acx;

                    /* 両インスタンスの領域の差分を計算 */
                    var region = Math.Abs(bcx - aex) + (bcx - aex > 0 ? -1 : 1) * (b.Width / 2);

                    var box3 = SideLines[2];
                    box3.TranslationX = (region > VIEWMARGIN * 2) ? bcx : box2x;
                    box3.TranslationY = bhy - VIEWMARGIN;
                    box3.WidthRequest = Math.Abs(bcx - box2x);
                    box3.HeightRequest = LINEPIXEL;
                    box3.IsVisible = true;

                    var box4 = SideLines[3];
                    box4.TranslationX = bcx;
                    box4.TranslationY = bhy - VIEWMARGIN;
                    box4.WidthRequest = LINEPIXEL;
                    box4.HeightRequest = VIEWMARGIN;
                }
            }
        }

        /// <summary>
        /// 描画した線のX,Y座標を格納する構造体
        /// </summary>
        public class LineBounds
        {
            public double LineX;
            public double LineY;
        }
    }
}