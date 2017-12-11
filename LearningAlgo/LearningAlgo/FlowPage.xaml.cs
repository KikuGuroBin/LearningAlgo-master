using System;
using System.Linq;
using System.Collections.Generic;
using LearningAlgo;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

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

        public FlowPage()
        {
            InitializeComponent();

            new DBConnection().DBRead();

            /*プリセット一覧をロードするメソッド*/
            PresetLoad();
        }

        public FlowPage(string s){
            InitializeComponent();

            new DBConnection().DBRead();

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

            myImage.Drug += Drug;

            /* フローチャートのパネルに画像を追加 */
            FlowPanel.Children.Add(myImage);

            /* レイアウトでの最後尾の座標を更新 */
            ImagePosition += 50;
        }

        private void Drug(object sender, DrugEventArgs args)
        {
            var image = sender as MyImage;

            if((image.TranslationX + args.X) < 0){
                return;
            }

            if((image.TranslationX + args.X + image.Width) > FlowPanel.Width){
                return;
            }

            image.TranslationX += args.X;

            if((image.TranslationY + args.Y) < 0){
                return;
            }

            if((image.TranslationY + args.Y + image.Height) > FlowPanel.Height){
                return;
            }

            image.TranslationY += args.Y;
        }

        private async void FlowLoad()
        {
            /*  */
            Opacity = 0;

            for (;;){
                /* ドラッグ可能なImageインスタンス生成 
                var myImage = new MyImage
                {
                    TranslationX = ,
                    TranslationY = ,
                    WidthRequest = 50,
                    HeightRequest = 50,
                    Source = source,
                };

                /* フローチャートのパネルに画像を追加 
                FlowPanel.Children.Add(myImage);*/
            }

            Opacity = 1;
        }

        public async void PresetLoad()
        {

            //PrisetScroll.HeightRequest;

            /*起動時にどうせ使う第壱テーブルを読み込む用コネクション*/
            PresetLoadClass PreClass = new PresetLoadClass();
            DbInsertListTb1 = await PreClass.OnAppearing();

            var a = from x in DbInsertListTb1
                    select x.Key;
            List<string> p = a.ToList<string>();

            for (int count = 0; count < p.Count; count++)
            {
                Button PriIdLabel = new Button
                {
                    Text = p[count]
                };
                /*sender eve as*/
                PriIdLabel.Clicked += (s, e) =>
                {
                    var l = s as Button;
                    var text = l.Text;
                    PresetSet(text);
                };

                Priset.Children.Add(PriIdLabel);
            }
        }

        public async void PresetSet(string Tb1Id)
        {
            /*選択後第弐テーブルを読み込んで配置する*/
            PartsLoadClass ParClass = new PartsLoadClass();
            (DbInsertListTb2, DbInsertListTb3) = await ParClass.OnAppearing(Tb1Id);


            Debug.WriteLine("プリセット読み込みメソッド：");
        }



        public async void TracePreviewer()
        {
            /*スタートフラグを探す*/
            var StartPossition = from x in DbInsertListTb2
                                 where x.Value.startFlag == "1"
                                 select x.Value.identification_id;
            string NextId = await TypeCalculate(DbInsertListTb2[String.Concat(StartPossition)]);
            /*トレース始めます*/
            for (;;)
            {
                /*計算はほかのメソッドに任せて出力先を返してくる*/
                NextId = await TypeCalculate(DbInsertListTb2[NextId]);
                /*もし出力先ばなければbreak*/
                if (NextId == "-1")
                {
                    break;
                }
            }
        }


        /*計算本体クラス
         第二テーブルを受け取って
         アウトプット番号を返す
         */
        public async Task<string> TypeCalculate(FlowPartstable PartsTb)
        {

            /*形状四角のばあい*/
            if (PartsTb.type_id == "SideSikaku.png")
            {
                VarManegement = new CalculateClass().SquareCalculate(VarManegement, PartsTb.data);
                var StartPossition2 = from x in DbInsertListTb3
                                      where x.Value.identification_id == PartsTb.identification_id
                                      select x.Key;

                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        Debug.WriteLine("四角で受け取るiの値:" + VarManegement["i"].ToString());
                        label1.Text = "i:   " + VarManegement["i"].ToString();
                    }
                    catch (Exception e) { }
                    finally { }

                    try
                    {
                        Debug.WriteLine("四角で受け取るiの値:" + VarManegement["j"].ToString());
                        label2.Text = "j:   " + VarManegement["j"].ToString();
                    }
                    catch (Exception e) { }
                    finally { }
                });
                /*表示用*/
                await LabelInserter(PartsTb.data);
                return DbInsertListTb3[StartPossition2.First()].output_identification_id;
            }

            /*形状ひし形のばあい*/
            else if (PartsTb.type_id == "SideHisigata.png")
            {
                (string Symbol, int b, int c) JudgAnsower = new CalculateClass().DiamondCalculat(VarManegement, PartsTb.data);
                string Symbol = JudgAnsower.Symbol;
                int before = JudgAnsower.b;
                int after = JudgAnsower.c;
                Debug.WriteLine("台形返還アイテム： " + Symbol + before + after);
                if (Symbol == ":")
                {

                }
                else if (Symbol == "-1")
                {
                    var NextIdFinder = from x in DbInsertListTb3
                                       where x.Value.identification_id == PartsTb.identification_id
                                       && x.Value.blanch_flag == "-1"
                                       select x.Value.output_identification_id;

                    await Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            label4.Text = label4.Text + "No" + "\n";
                        });
                        Thread.Sleep(1000);
                    });


                    Debug.WriteLine("Noだった場合(-1)の時のリターン値：　" + NextIdFinder.First());

                    /*表示用*/
                    await LabelInserter(PartsTb.data);
                    return String.Concat(NextIdFinder.First());
                }
                else if (Symbol == "0")
                {
                    var NextIdFinder = from x in DbInsertListTb3
                                       where x.Value.identification_id == PartsTb.identification_id
                                         && x.Value.blanch_flag == "0"
                                       select x.Value.output_identification_id;
                    await Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            label4.Text = label4.Text + "Yes" + "\n";
                        });
                        Thread.Sleep(1000);
                    });

                    Debug.WriteLine("Yesだった場合(0)の時のリターン値：　" + NextIdFinder.First());

                    /*表示用*/
                    await LabelInserter(PartsTb.data);
                    return String.Concat(NextIdFinder.First());

                }

            }
            else if (PartsTb.type_id == "SideDaikeiUe.png")
            {
            }
            else if (PartsTb.type_id == "SideDaikeiSita.png")
            {
            }
            else if (PartsTb.type_id == "SideHeikou.png")
            {
                var NextIdFinder = from x in DbInsertListTb3
                                   where x.Value.identification_id == PartsTb.identification_id
                                   select x.Value.output_identification_id;
                var Output = from x in DbInsertListTb2
                             where x.Value.identification_id == PartsTb.identification_id
                             select x.Value.data;

                /*表示用*/
                await LabelInserter(PartsTb.data);
                Debug.WriteLine("最終結果ではなく:  " + VarManegement[new CalculateClass().ParallelogramOutput(String.Concat(Output))]);
                label3.Text = "終了" + String.Concat(Output) + "の解は" + VarManegement[new CalculateClass().ParallelogramOutput(String.Concat(Output))];
                return "-1";

            }
            return "-1";

        }

        private async Task<string> LabelInserter(string data)
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    label4.Text = label4.Text + data.ToString() + "\n";
                });

                Thread.Sleep(1000);
            });
            return "";
        }

        private void TraceDoCliked(object sender, EventArgs e)
        {
            /*本来はトレースボタンが押された際のメソッド呼び出し
             * トレース実際にやってみた*/
            label1.Text = "i:   0";
            label2.Text = "j:   0";
            VarManegement.Clear();
            label4.Text = "";
            label3.Text = "開始";
            TracePreviewer();
        }
    }
}
