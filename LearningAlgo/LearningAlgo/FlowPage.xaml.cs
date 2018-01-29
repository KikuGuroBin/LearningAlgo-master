using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using LearningAlgo;

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
        private const int DIALOGSIZE = 500;

        /// <summary>
        /// フローチャートの部品のサイズ
        /// </summary>
        private const int IMAGESIZE = 100;

        /// <summary>
        /// 線を描画するレイアウト
        /// </summary>
        public LineCanvas Canvas;

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

        /// <summary>
        /// 配置された画像インスタンスを格納する (identification_id
        /// </summary>
        private Dictionary<int, Label> ImageList = new Dictionary<int, Label>();

        /// <summary>
        /// トレース時配置された画像の前後関係の把握用
        /// </summary>
        private int PriviousId = 0;

        /// <summary>
        /// 移動可能インスタンス新規作成時衝突を起こさないためのImageListの最大キー
        /// </summary>
        private int MaxIdentificationId = 0;

        /// <summary>
        /// プリセットテーブル　プリID、各テーブルの中身(日にちとかプリIDとか)
        /// </summary>
        Dictionary<string, Flowtable> DbInsertListTb1 = new Dictionary<string, Flowtable>();

        /// <summary>
        /// 各プリセットの中身用テーブルパーツID,各テーブルの中身(パーツIDとか式とか)
        /// </summary>
        Dictionary<string, FlowPartstable> DbInsertListTb2 = new Dictionary<string, FlowPartstable>();

        /// <summary>
        /// 各パーツの中身用テーブルパーツID,各テーブルの中身(パーツIDとか出力先)
        /// </summary>
        Dictionary<string, Outputtable> DbInsertListTb3 = new Dictionary<string, Outputtable>();

        /// <summary>
        /// iとかjとか
        /// </summary>
        Dictionary<string, int> VarManegement = new Dictionary<string, int>();

        /// <summary>
        /// 配置されているMyLayoutインスタンスのリスト
        /// </summary>
        List<MyLayout> MyLayoutList = new List<MyLayout>();

        /// <summary>
        /// Lineを繋ぐOutputtable型のList
        /// </summary>
        List<Outputtable> OutputTableList = new List<Outputtable>();

        /// <summary>
        /// 2つのオブジェクトをつなぐ際のドラッグとの差別用フラグ
        /// </summary>
        bool LineConnectionflug;

        Stack<View> LineConnectionStack = new Stack<View>();

        string SelectFloeId;

        public FlowPage()
        {
            InitializeComponent();

            new DBConnection().DBRead();

            Dialog.CommitButton.Clicked += ComitButtonClicked;

            /* プリセット一覧をロードするメソッド */
            PresetLoad();

            InitializeCanvas();


        }

        public FlowPage(string s){
            InitializeComponent();

            new DBConnection().DBRead();

            Dialog.CommitButton.Clicked += ComitButtonClicked;

            /*プリセット一覧をロードするメソッド*/
            PresetLoad();

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

            SidePane.Opacity = 0;

            /* 初回限定の処理 */
            if (First)
            {
                /* ダイアログ関係のインスタンスを隠す */
                Shadow.TranslateTo(0, height, 0);
                Shadow.WidthRequest = width;
                Shadow.HeightRequest = height;
                Shadow.Opacity = 0;
                Shadow.IsVisible = false;

                Dialog.TranslateTo(width / 2, height, 0);
                Dialog.WidthRequest = DIALOGSIZE;
                Dialog.HeightRequest = DIALOGSIZE;
                Dialog.Opacity = 0;

                /* フローチャートのレイアウト部分を配置 
                FlowScroller.TranslateTo(0, 0, 0);
                FlowScroller.WidthRequest = width;
                FlowScroller.HeightRequest = height / 10 * 9;*/

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

        /* index.InitializeCanvas */
        /// <summary>
        /// フィールドCanvasの初期化
        /// </summary>
        private void InitializeCanvas()
        {
            Canvas = new LineCanvas()
            {
                BackgroundColor = Color.Lime,
            };

            /* 制約付きでフィールドMainにCanvas追加 */
            MainLayout.Children.Add(Canvas,
                Constraint.RelativeToParent((p) =>
                {
                    return Canvas.X;
                }),
                Constraint.RelativeToParent((p) =>
                {
                    return Canvas.Y;
                }),
                Constraint.RelativeToParent((p) =>
                {
                    return Canvas.Width;
                }),
                Constraint.RelativeToParent((p) =>
                {
                    return Canvas.Height;
                })
            );

            var rc = Canvas.Bounds;
            rc.X += 50;
            rc.Y += 100;
            rc.Width += 50;
            rc.Height += 50;
            Canvas.LayoutTo(rc, 0);
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
        /// サイドラベルタップイベント
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        private void SidePanel(object sender, EventArgs args)
        {
            SidePanelShow();
        }

        private void ShowDialogClicked(object sender, EventArgs args)
        {
            
            /* カスタムダイアログ表示 */
            ImitationDialog.ShowUp(200);
        }

        private void ShadowTapped(object sender, EventArgs args)
        {
            ImitationDialog.Hide();

        }

        private void ComitButtonClicked(object sender, EventArgs args)
        {
            Debug.WriteLine("Debug:Now" + Dialog.SendId);
            ImageList[Dialog.SendId].Text = Dialog.SendStr;
            DbInsertListTb2[Dialog.SendId.ToString()].data = Dialog.SendStr;
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

            /* 新規キーの更新 */
            MaxIdentificationId++;

            /* ドラッグ可能なレイアウト生成 */
            var myLayout = new MyLayout
            {
                BackgroundColor =Color.Azure,
                //TranslationX = 0,
                //TranslationY = ImagePosition,
                WidthRequest = IMAGESIZE,
                HeightRequest = IMAGESIZE,
                DrugFlag = true
            };

            /* フローチャートのパネルに画像を追加 */
            var rc = myLayout.Bounds;

            CanvasAppend(myLayout, 0, ImagePosition, rc.Width, rc.Height);
            myLayout.LayoutTo(new Rectangle(0, ImagePosition, rc.Width, rc.Height), 0);

            /*レイアウト内画像*/
            var myImage = new Image
            {
                BackgroundColor=Color.Brown,
                Source=source,
                WidthRequest = IMAGESIZE,
                HeightRequest = IMAGESIZE ,
            };

            /* 処理内容を記述するスクロール可能なラベル生成 */
            var myLabel = new MyLabel
            {
                Text="",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                LabelId = MaxIdentificationId,

            };
            var scroll = new ScrollView()
            {
                BackgroundColor=Color.BlueViolet,
                Content = myLabel,
                TranslationX = IMAGESIZE / 4,
                TranslationY = IMAGESIZE * 3 / 8,
                WidthRequest = IMAGESIZE / 2,
                HeightRequest = IMAGESIZE / 4,
                Orientation = ScrollOrientation.Horizontal,
            };


            var tgr = new TapGestureRecognizer();
            tgr.Tapped += (s, e) =>
            {
                var labelid = (s as MyLabel).LabelId;
                Dialog.SendStr = ImageList[labelid].Text;
                Dialog.SendId = labelid;
                /* カスタムダイアログ表示 */
                Debug.WriteLine("Debug:    ItemTapped");
                ImitationDialog.ShowUp(200);
            };

            myLabel.GestureRecognizers.Add(tgr);
            myLayout.PartsId = MaxIdentificationId.ToString();
            MyLayoutList.Add(myLayout);
            myLayout.LayoutDrug += LayoutDrug;
            myLayout.Children.Add(myImage);
            myLayout.Children.Add(scroll);

            /* 管理用リストに追加 */
            ImageList.Add(MaxIdentificationId, myLabel);
            /* 第二管理Dictionaryに追加 */
            DbInsertListTb2[MaxIdentificationId.ToString()] = new FlowPartstable();
            DbInsertListTb2[MaxIdentificationId.ToString()].flow_id = SelectFloeId;
            DbInsertListTb2[MaxIdentificationId.ToString()].identification_id = MaxIdentificationId.ToString();
            DbInsertListTb2[MaxIdentificationId.ToString()].type_id = source.ToString();
            DbInsertListTb2[MaxIdentificationId.ToString()].startFlag = "0";
            /* レイアウトでの最後尾の座標を更新 */
            ImagePosition += IMAGESIZE;

        }

        /* index.CanvasAppend */
        /// <summary>
        /// フィールドCanvasへのViewの追加。
        /// </summary>
        /// <param name="view">Canvasに追加するViewインスタンス。</param>
        /// <param name="x">ViewインスタンスのX座標。</param>
        /// <param name="y">ViewインスタンスのY座標。</param>
        /// <param name="width">Viewインスタンスの幅。</param>
        /// <param name="height">Viewインスタンスの高さ。</param>
        private async void CanvasAppend(View view, double x = 0, double y = 0, double width = 0, double height = 0)
        {
            /* 制約付きでViewを追加 */
            Canvas.AppendView(view);

            /* 引数で指定した座標、サイズにする */
            var rc = view.Bounds;
            rc.X = x;
            rc.Y = y;
            rc.Width = width;
            rc.Height = height;
            await view.LayoutTo(rc, 0);

            rc = Canvas.Bounds;

            /* Canvasの拡張 */
            if (rc.Width < x + width)
            {
                rc.Width = x + width;
            }

            if (rc.Height < y + height)
            {
                rc.Height = y + height;
            }

            await Canvas.LayoutTo(rc, 0);
        }

        /* index.MoveCanvas */
        /// <summary>
        /// フィールドCanvasを非同期で動かす。
        /// </summary>
        /// <param name="x">Canvasの新しいX座標。</param>
        /// <param name="y">Canvasの新しいY座標。</param>
        private async void MoveCanvas(double x, double y)
        {
            var rc = Canvas.Bounds;
            rc.X += x;
            rc.Y += y;
            await Canvas.LayoutTo(rc, 0);
        }

        private async void MoveView(View view, double x = 0, double y = 0){
            var rc = view.Bounds;
            rc.X += x;
            rc.Y += y;
            await view.LayoutTo(rc, 0);
        }

        private void LayoutDrug(object sender, DrugEventArgs args)
        {
            var layout = sender as MyLayout;

            if(!LineConnectionflug)
            {
                /* Viewの移動 */
                MoveView(layout, args.X, args.Y);

                /* 動かしたViewがつながっているLineCanvas.Lineインスタンスの一覧を取得 */
                var lines = Canvas.SearchLines(layout);

                foreach (var l in lines)
                {
                    l.Draw();
                }

                var rc = Canvas.Bounds;

                /* Canvasの拡張 */
                if (rc.Width < layout.X + layout.Width)
                {
                    rc.Width = layout.X + layout.Width;
                }

                if (rc.Height < layout.Y + layout.Height)
                {
                    rc.Height = layout.Y + layout.Height;
                }

                Canvas.LayoutTo(rc, 0);

                DbInsertListTb2[layout.PartsId].position_x = "";
                DbInsertListTb2[layout.PartsId].position_y = "";
            }
            else
            {
                /*線を引くモード*/
                if (LineConnectionStack.Contains(layout))
                {
                    //layout.BackgroundColor = Color.Black;
                    //LineConnectionStack.Pop();
                    //Debug.WriteLine("同じなのでだしました");
                }
                else
                {
                    if (LineConnectionStack.Count == 0)
                    {
                        layout.BackgroundColor = Color.Red;
                        LineConnectionStack.Push(layout);
                        Debug.WriteLine("一つ目を追加");
                    }
                    else
                    {
                        View layout2 = LineConnectionStack.Peek();
                        layout2.BackgroundColor = Color.Black;
                        LineConnectionStack.Pop();
                        LineConnectionStack.Clear();
                        Debug.WriteLine("2つ目を追加および線");
                        LineConnectionflug = false;

                        var line = Canvas.Tail(layout2, layout);

                        line.Draw();

                    }

                }
            }

            //if (!layout.DrugFlag)
            //{
            //    return;
            //}

            //if ((layout.TranslationX + args.X) < 0)
            //{
            //    return;
            //}

            //if ((layout.TranslationX + args.X + layout.Width) > Canvas.Width)
            //{
            //    return;
            //}

            //layout.TranslationX += args.X;

            //if ((layout.TranslationY + args.Y) < 0)
            //{
            //    return;
            //}

            //if ((layout.TranslationY + args.Y + layout.Height) > Canvas.Height)
            //{
            //    return;
            //}

            //layout.TranslationY += args.Y;


        }

        /// <summary>
        /// LineCanvasインスタンスのドラッグ用イベント。
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        private void CanvasDrug(object sender, DrugEventArgs args)
        {
            
        }

        /* ------------- でバッグ用 --------------- */
        int s;

        void Clicked3(object sa, EventArgs args)
        {
            //Canvas.Children.Add(new Label { Text = "aaa" }, new Rectangle(0, s += 100, 50, 50));
        }
        /* ------------- --------------- */


        /// <summary>
        /// フローチャートの読み込みを行う
        /// </summary>
        /// <param name="array">フローチャートのパーツを格納した配列</param>
        private void FlowLoad(FlowPartstable[] array)
        {
            /* 
             * 
             * flow_id
             * identification_id
             * type_id
             * data
             * position_x
             * position_y
             * startFlag
             */

            /* 別のプリセット読み込んだ時初期化用 */
            //FlowPanel.Children.Clear();
            ImageList.Clear();
            MaxIdentificationId = 0;

            var w = 10;

            foreach (var flow in array){
                var x = double.Parse(flow.position_x) + 100;
                var y = double.Parse(flow.position_y);

                /* ドラッグ可能なレイアウト生成 */
                var myLayout = new MyLayout
                {
                    BackgroundColor = Color.Azure,
                    WidthRequest = IMAGESIZE,
                    HeightRequest = IMAGESIZE,
                    DrugFlag = true
                };

                var work = y + (w += 100);

                var rc = new Rectangle(x, work, IMAGESIZE, IMAGESIZE);

                /* フローチャートのパネルに画像を追加 */
                CanvasAppend(myLayout, x, work, IMAGESIZE, IMAGESIZE);

                /*レイアウト内画像*/
                var myImage = new Image
                {
                    BackgroundColor = Color.Brown,
                    WidthRequest = IMAGESIZE,
                    HeightRequest = IMAGESIZE,
                    Source = flow.type_id,
                };

                /* 処理内容を記述するスクロール可能なラベル生成 */
                var myLabel = new MyLabel
                {
                    Text = flow.data,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    LabelId = int.Parse(flow.identification_id),

                };
                var scroll = new ScrollView()
                {
                    Content = myLabel,
                    TranslationX = IMAGESIZE / 4,
                    TranslationY = IMAGESIZE * 3 / 8,
                    WidthRequest = IMAGESIZE / 2,
                    HeightRequest = IMAGESIZE / 4,
                    Orientation = ScrollOrientation.Horizontal,
                };

                if(MaxIdentificationId < int.Parse(flow.identification_id))
                {
                    MaxIdentificationId = int.Parse(flow.identification_id);
                }

                /* 管理用リストに追加 */
                ImageList.Add(int.Parse(flow.identification_id), myLabel);

                var tgr = new TapGestureRecognizer();
                tgr.Tapped += (s, e) =>
                {
                    var labelid = (s as MyLabel).LabelId;
                    Dialog.SendStr = ImageList[labelid].Text;
                    Dialog.SendId = labelid;

                    /* カスタムダイアログ表示 */
                    Debug.WriteLine("Debug:    FlowLoad.MyLabel.Tap");
                    Dialog.SetStr();
                    ImitationDialog.ShowUp(200);
                };
                myLabel.GestureRecognizers.Add(tgr);
                myLayout.PartsId = flow.identification_id;
                MyLayoutList.Add(myLayout);
                myLayout.LayoutDrug += LayoutDrug;
                myLayout.Children.Add(myImage);
                myLayout.Children.Add(scroll);
            }

        }

        public async void PresetLoad()
        {

            //PrisetScroll.HeightRequest;

            /* 起動時にどうせ使う第1テーブルを読み込む用コネクション */
            var preClass = new PresetLoadClass();
            DbInsertListTb1 = await preClass.OnAppearing();

            var p = (from x in DbInsertListTb1
                     select x.Key).ToList<string>();

            for (int count = 0; count < p.Count; count++)
            {
                Button PriIdLabel = new Button
                {
                    Text = p[count]
                };

                PriIdLabel.Clicked += (s, e) =>
                {
                    var l = s as Button;
                    var text = l.Text;
                    SelectFloeId = text;
                    PresetSet(text);
                };

                Priset.Children.Add(PriIdLabel);
            }
        }

        public async void PresetSet(string Tb1Id)
        {
            /* 選択後第2テーブルを読み込んで配置する */
            var parClass = new PartsLoadClass();

            (DbInsertListTb2, DbInsertListTb3) = await parClass.OnAppearing(Tb1Id);

            /* 配置 */
            FlowLoad((from o in DbInsertListTb2.Values select o).ToArray());

            /* 線の描画が完了したViewのIDリスト */
            var work = new List<string>();

            /* 線の描画 */
            foreach(var myLayout in MyLayoutList)
            {
                /* myLayoutと関連を持った別のViewの検索 */
                var outputs = from view in MyLayoutList
                              from view2 in (from x in DbInsertListTb3
                                             where x.Value.identification_id == myLayout.PartsId
                                             select x.Value.output_identification_id)
                              where view.PartsId == view2
                              select view;

                /* 検索したViewとmyLayoutをつなぐ線を引く */
                foreach(var v in outputs)
                {
                    /* 線を引く */
                    var line = Canvas.Tail(myLayout, v);
                    line.Draw();
                }

                /* ViewのIDをリストに格納 */
                work.Add(myLayout.PartsId);
            }
        }

        public async void TracePreviewer()
        {
            /*スタートフラグを探す*/
            var StartPossition = from x in DbInsertListTb2
                                 where x.Value.startFlag == "1"
                                 select x.Value.identification_id;
            
            string NextId = await TypeCalculate(DbInsertListTb2[String.Concat(StartPossition)]);

            /* 
             * トレース開始
             * 出力先がなくなるまでループ
             */
            for (;;)
            {
                /* 計算はほかのメソッドに任せて出力先を返してくる */
                NextId = await TypeCalculate(DbInsertListTb2[NextId]);
                /*もし出力先が-1だったらbreak*/
                if(NextId == "-1")
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 計算を行う
        /// 第二テーブルを受け取ってアウトプット番号を返す
        /// </summary>
        /// <returns>The calculate.</returns>
        /// <param name="partsTb">Parts tb.</param>
        public async Task<string> TypeCalculate(FlowPartstable partsTb)
        {
            /* 形状四角 */
            if (partsTb.type_id.Equals("SideSikaku.png"))
            {
                VarManegement = new CalculateClass().SquareCalculate(VarManegement, partsTb.data);
                var startPossition2 = from x in DbInsertListTb3
                                      where x.Value.identification_id == partsTb.identification_id
                                      select x.Key;

                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        Debug.WriteLine("四角で受け取るiの値:" + VarManegement["i"].ToString());
                        ILabel.Text = "i:   " + VarManegement["i"].ToString();
                    }catch(Exception){ }
                    try{
                        Debug.WriteLine("四角で受け取るjの値:" + VarManegement["j"].ToString());
                        JLabel.Text = "j:   " + VarManegement["j"].ToString();
                    }catch (Exception) { }
                });

                /* 表示用 */
                await LabelInserter(partsTb);

                Debug.WriteLine("Debug:    color");

                return DbInsertListTb3[startPossition2.First()].output_identification_id;
            }
            /* 形状ひし形 */
            else if (partsTb.type_id.Equals("SideHisigata.png"))
            {
                (string Symbol, int b, int c) JudgAnsower = new CalculateClass().DiamondCalculat(VarManegement, partsTb.data);
                string Symbol = JudgAnsower.Symbol;
                int before = JudgAnsower.b;
                int after = JudgAnsower.c;

                Debug.WriteLine("hisigata返還アイテム： " + Symbol + before + after);

                if (Symbol == ":")
                {
                    if (before == after)
                    {
                        Symbol = "-1";
                    }
                    else if (before > after)
                    {
                        Symbol = "1";
                    }
                    else
                    {
                        Symbol = "2";
                    }

                }
                if (Symbol == "-1")
                {

                    /* 表示用 */
                    await LabelInserter(partsTb);

                    var NextIdFinder = from x in DbInsertListTb3
                                       where x.Value.identification_id == partsTb.identification_id
                                          && x.Value.blanch_flag == "-1"
                                       select x.Value.output_identification_id;

                    await Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            TraceLabel.Text = TraceLabel.Text + "No" + "\n";
                        });

                        Thread.Sleep(1000);
                    });


                    Debug.WriteLine("Noだった場合(-1)の時のリターン値：　" + NextIdFinder.First());

                    return String.Concat(NextIdFinder.First());
                }
                else if (Symbol.Equals("0"))
                {
                    /* 表示用 */
                    await LabelInserter(partsTb);

                    var NextIdFinder = from x in DbInsertListTb3
                                       where x.Value.identification_id == partsTb.identification_id
                                          && x.Value.blanch_flag == "0"
                                       select x.Value.output_identification_id;

                    await Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            TraceLabel.Text = TraceLabel.Text + "Yes" + "\n";
                        });

                        Thread.Sleep(1000);
                    });

                    Debug.WriteLine("Yesだった場合(0)の時のリターン値：　" + NextIdFinder.First());

                    return String.Concat(NextIdFinder.First());
                }
                else if (Symbol.Equals("1"))
                {
                    /* 表示用 */
                    await LabelInserter(partsTb);

                    var NextIdFinder = from x in DbInsertListTb3
                                       where x.Value.identification_id == partsTb.identification_id
                                          && x.Value.blanch_flag == "1"
                                       select x.Value.output_identification_id;

                    await Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            TraceLabel.Text = TraceLabel.Text + ">" + "\n";
                        });

                        Thread.Sleep(1000);
                    });

                    Debug.WriteLine(": >場合(1)の時のリターン値：　" + NextIdFinder.First());
                    return String.Concat(NextIdFinder.First());
                }
                else if (Symbol.Equals("2"))
                {
                    /* 表示用 */
                    await LabelInserter(partsTb);

                    var nextIdFinder = from x in DbInsertListTb3
                                       where x.Value.identification_id == partsTb.identification_id
                                          && x.Value.blanch_flag == "2"
                                       select x.Value.output_identification_id;

                    await Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            TraceLabel.Text = TraceLabel.Text + "<" + "\n";
                        });

                        Thread.Sleep(1000);
                    });

                    Debug.WriteLine(": <場合(1)の時のリターン値：　" + nextIdFinder.First());

                    return String.Concat(nextIdFinder.First());
                }
            }
            /* 形状台形 */
            else if (partsTb.type_id.Equals("SideDaikeiUe.png") || partsTb.type_id.Equals("SideDaikeiSita.png"))
            {
                if (!partsTb.data.Equals(""))
                {
                    (string Symbol, int b, int c) judgAnsower = new CalculateClass().DiamondCalculat(VarManegement, partsTb.data);
                    string symbol = judgAnsower.Symbol;
                    int before = judgAnsower.b;
                    int after = judgAnsower.c;

                    if (symbol == "-1")
                    {
                        /* 表示用 */
                        await LabelInserter(partsTb);
                        var nextIdFinder = from x in DbInsertListTb3
                                           where x.Value.identification_id == partsTb.identification_id
                                              && x.Value.blanch_flag == "-1"
                                           select x.Value.output_identification_id;
                        
                        await Task.Run(() =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                TraceLabel.Text += "継続\n";
                            });

                            Thread.Sleep(1000);
                        });

                        Debug.WriteLine("Noだった場合(-1)の時のリターン値：　{0}", nextIdFinder.First());

                        return String.Concat(nextIdFinder.First());
                    }
                    else if (symbol.Equals("0"))
                    {
                        /* 表示用 */
                        await LabelInserter(partsTb);
                        var nextIdFinder = from x in DbInsertListTb3
                                           where x.Value.identification_id == partsTb.identification_id
                                              && x.Value.blanch_flag == "0"
                                           select x.Value.output_identification_id;
                        
                        await Task.Run(() =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                TraceLabel.Text += "終了\n";
                            });

                            Thread.Sleep(1000);
                        });

                        Debug.WriteLine("Yesだった場合(0)の時のリターン値：　{0}", nextIdFinder.First());

                        return String.Concat(nextIdFinder.First());
                    }
                }
                else
                {
                    var nextIdFinder = from x in DbInsertListTb3
                                       where x.Value.identification_id == partsTb.identification_id
                                          && x.Value.blanch_flag == "0"
                                       select x.Value.output_identification_id;
                    /*
                    await Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            TraceLabel.Text = TraceLabel.Text + "スルーパス" + "\n";
                        });

                        Thread.Sleep(1000);
                    });
                    Debug.WriteLine("Yesだった場合(0)の時のリターン値：　" + NextIdFinder.First());
                    */

                    return String.Concat(nextIdFinder.First());
                }

            }
            /* 形状平行四辺形 */
            else if (partsTb.type_id == "SideHeikou.png")
            {
                var nextIdFinder = from x in DbInsertListTb3
                                   where x.Value.identification_id == partsTb.identification_id
                                   select x.Value.output_identification_id;

                var output = from x in DbInsertListTb2
                             where x.Value.identification_id == partsTb.identification_id
                             select x.Value.data;

                /* 表示用 */
                await LabelInserter(partsTb);

                Debug.WriteLine("最終結果ではなく:  " + VarManegement[new CalculateClass().ParallelogramOutput(String.Concat(output))]);

                MassageLabel.Text = "終了" + String.Concat(output) + "の解は" + VarManegement[new CalculateClass().ParallelogramOutput(String.Concat(output))];

                return "-1";
            }

            return "-1";
        }

        /// <summary>
        /// トレースの現在位置を可視化する為にラベルの背景色を変える
        /// </summary>
        /// <returns></returns>
        /// <param name="flowTable">フローチャートの部品を格納した配列</param>
        private async Task<string> LabelInserter(FlowPartstable flowTable)
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                TraceLabel.Text = TraceLabel.Text + flowTable.data.ToString() + "\n";
                int id = int.Parse(flowTable.identification_id);

                    try
                    {
                        ImageList[PriviousId].BackgroundColor = Color.Default;
                        ImageList[id].BackgroundColor = Color.Violet;
                        PriviousId = id;
                    }catch(Exception)
                    {
                        ImageList[id].BackgroundColor = Color.Violet;
                        PriviousId = id;
                    }
                });

                Thread.Sleep(1000);
            });

            return "";
        }

        private void TraceDoCliked(object sender, EventArgs e)
        {
            /* 本来はトレースボタンが押された際のメソッド呼び出し */
            ILabel.Text = "i:   0";
            JLabel.Text = "j:   0";
            VarManegement.Clear();
            TraceLabel.Text = "";
            MassageLabel.Text = "開始";
            TracePreviewer();
        }
        private void LineConectionClicked(object sender, EventArgs e)
        {
            if(!LineConnectionflug)
            {
                LineConnectionflug = true;
            }
            else
            {
                LineConnectionflug = false;
            }
        }
        private void SaveClicked(object sender,EventArgs e)
        {
            /*
            DbInsertListTb2でーたべーすに
            */
        }
    }
}
