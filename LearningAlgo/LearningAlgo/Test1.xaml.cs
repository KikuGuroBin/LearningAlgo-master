using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearningAlgo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Test1 : ContentPage
	{

        // DBにする配列達
        string[] FlowTable = new string[3];
        string[] PrintTable = new string[4];
        string[] KindTable = new string[3];


        public Test1 ()
		{
			InitializeComponent ();

            // プログラムでのレイアウト追加方法
            Label TestNo1Label = new Label
            {
                Text = "1→i"
            };
            Label TestNo2Label = new Label
            {
                Text = "i＋1→i"
            };
            Label TestNo3Label = new Label
            {
                Text = "i出力"
            };

            MainLayout.Children.Add(TestNo1Label);
            MainLayout.Children.Add(TestNo2Label);
            MainLayout.Children.Add(TestNo3Label);

            // 必要な工程   形種類の判定


            // 判定後”□”の場合
            char[] StackText = TestNo1Label.Text.ToCharArray();
            bool Calculat = false;
            bool Tenmade = false;
            ArrayDeque NumberStack = new ArrayDeque();
            ArrayDeque SymbolStack = new ArrayDeque();

            for (int length = 0 ; length < StackText.Length ; length--)
            {
                if (Calculat==true)
                {

                    Calculat = false;
                    string Symbol = SymbolStack.Pop().ToString();
                    if (Symbol.Equals("×"))
                    {
                        NumberStack.Push(int.Parse(NumberStack.Pop().ToString()) * int.Parse(StackText[length].ToString()));
                    }
                    else if (Symbol.Equals("÷"))
                    {
                        NumberStack.Push(int.Parse(NumberStack.Pop().ToString()) / int.Parse(StackText[length].ToString()));

                    }


                    int a = int.Parse(NumberStack.Pop().ToString());

                }
                else if (StackText[length].Equals("→"))
                {

                    break;
                }

                else
                {
                    if (StackText[length].Equals("×") || StackText[length].Equals("÷") ||
                        StackText[length].Equals("＋") || StackText[length].Equals("－"))
                    {
                        SymbolStack.Push(StackText[length]);
                        Tenmade = false;
                        Calculat = true;

                    }
                    else
                    {
                        if (Tenmade == true)
                        {
                            NumberStack.Push(int.Parse(NumberStack.Pop().ToString()) * 10 + StackText[length]);
                        }
                        else
                        {
                            NumberStack.Push(StackText[length]);
                        }
                        Tenmade = true;

                    }





                }
                




                if(StackText[length].Equals("×") || StackText[length].Equals("÷")){
                    Calculat = true;

                }
                



            }


        }
	}
}