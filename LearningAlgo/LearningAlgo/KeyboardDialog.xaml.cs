using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LearningAlgo
{
    public partial class KeyboardDialog : ContentView
    {
        /// <summary>
        /// Labelのデータバインディング用
        /// </summary>
        private ViewBounds LabelBounds;

        private string SendStr;



        bool Flag;

        public bool Showing { get; set; }

        public KeyboardDialog()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

        private void PositionButton(object sender, EventArgs e)
        {
            if (sender.Equals(nextbutton))
            {
                ///→押下
                ///ここにらべるのやつの処理を書く


            }
            else if (sender.Equals(backbutton))
            {
                ///←押下
                ///ここにらべるのやつの処理を書く
            }
        }
        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            ///消去ボタン押下
            SendStr = "";
            displaylabel.Text = "入力してください";
        }
        private void ComitButtonClicked(object sender, EventArgs e)
        {
            ///決定ボタン押下
            ///ここにコミットされた文字列を送信する処理をﾊﾞﾊﾞｰﾝと書く
            ///ImitationDialog.Hide();の処理 と同じ処理を書きたい

        }

        private void OnClicked(object sender, EventArgs e)
        {
            ///1,2,3,4,5,6,7,8,9,0
            ///+,-,×,÷,%
            ///=,≒,>,>=,<,<=
            ///i,j

            if (sender.Equals(button0))
            {
                ///DisplayStr += "0";
                System.Diagnostics.Debug.WriteLine("asdlkfjasdfhkasdbflhjjbxdznjsa");
                SendStr += "0";
            }
            else if (sender.Equals(button1))
            {
                ///DisplayStr += "1";
                SendStr += "1";
            }
            else if (sender.Equals(button2))
            {
                ///DisplayStr += "2";
                SendStr += "2";
            }
            else if (sender.Equals(button3))
            {
                ///DisplayStr += "3";
                SendStr += "3";
            }
            else if (sender.Equals(button4))
            {
                ///DisplayStr += "4";
                SendStr += "4";
            }
            else if (sender.Equals(button5))
            {
                ///DisplayStr += "5";
                SendStr += "5";
            }
            else if (sender.Equals(button6))
            {
                ///DisplayStr += "6";
                SendStr += "6";
            }
            else if (sender.Equals(button7))
            {
                ///DisplayStr += "7";
                SendStr += "7";
            }
            else if (sender.Equals(button8))
            {
                ///DisplayStr += "8";
                SendStr += "8";
            }
            else if (sender.Equals(button9))
            {
                ///DisplayStr += "9";
                SendStr += "9";
            }
            else if (sender.Equals(buttoni))
            {
                ///i
                SendStr += "i";

            }
            else if (sender.Equals(buttonj))
            {
                ///j
                SendStr += "j";
            }
            else if (sender.Equals(buttonplus))
            {
                ///+
                SendStr += "＋";
            }
            else if (sender.Equals(buttonminas))
            {
                ///-
                SendStr += "−";
            }
            else if (sender.Equals(buttonmulti))
            {
                ///×
                SendStr += "×";
            }
            else if (sender.Equals(buttondivid))
            {
                ///÷
                SendStr += "÷";
            }
            else if (sender.Equals(buttonremainder))
            {
                ///%
                SendStr += "％";
            }
            else if (sender.Equals(buttonclon))
            {
                ///:
                SendStr += "：";
            }
            else if (sender.Equals(buttoneqal))
            {
                ///=
                SendStr += "＝";
            }
            else if (sender.Equals(buttonnoteqal))
            {
                ///≒
                SendStr += "≒";
            }
            else if (sender.Equals(buttondainari))
            {
                ///>
                SendStr += "＞";
            }
            else if (sender.Equals(buttondainarieqal))
            {
                ///>=
                SendStr += "＞＝";
            }
            else if (sender.Equals(buttonsyounari))
            {
                ///<
                SendStr += "＜";
            }
            else if (sender.Equals(buttonsyounarieqal))
            {
                ///<=
                SendStr += "＞＝";
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                displaylabel.Text = SendStr;
            });

        }
    }
}
