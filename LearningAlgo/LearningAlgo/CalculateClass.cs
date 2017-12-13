using System;
using System.Collections.Generic;
using System.Text;

namespace LearningAlgo
{
    class CalculateClass
    {
        public CalculateClass() { }

        Stack<int> NumberStack;
        Stack<string> SymbolStack;
        string RightArithExpression;
        string LeftArithExpression;
        string JudgeSymbol;

        /*ひし形の計算*/
        public (string, int, int) DiamondCalculat(Dictionary<string, int> VarManagement, string ArithExpression)
        {
            NumberStack = new Stack<int>();
            SymbolStack = new Stack<string>();

            char[] stackText = ArithExpression.ToCharArray();

            for (int lengthcount = 0, rorl = 0; lengthcount < stackText.Length; lengthcount++)
            {
                if (rorl == 0)
                {

                    if (stackText[lengthcount].ToString() == "：" ||
                        stackText[lengthcount].ToString() == "＞" ||
                        stackText[lengthcount].ToString() == "＜" ||
                        stackText[lengthcount].ToString() == "≧" ||
                        stackText[lengthcount].ToString() == "≦" ||
                        stackText[lengthcount].ToString() == "＝" ||
                        stackText[lengthcount].ToString() == "≠")
                    {
                        JudgeSymbol = stackText[lengthcount].ToString();
                        rorl = 1;
                    }
                    else
                    {
                        LeftArithExpression = LeftArithExpression + stackText[lengthcount].ToString();
                        System.Diagnostics.Debug.WriteLine("左の中身:       " + LeftArithExpression);
                    }
                }
                else
                {
                    RightArithExpression = RightArithExpression + stackText[lengthcount].ToString();
                    System.Diagnostics.Debug.WriteLine("右の中身:       " + RightArithExpression);
                }
            }
            System.Diagnostics.Debug.WriteLine("前半: " + LeftArithExpression);
            System.Diagnostics.Debug.WriteLine("後半：" + RightArithExpression);

            string before = DiamondSeparateCalculate(VarManagement, LeftArithExpression + "→");
            string after = DiamondSeparateCalculate(VarManagement, RightArithExpression + "→");
            System.Diagnostics.Debug.WriteLine("前半: " + before);
            System.Diagnostics.Debug.WriteLine("後半：" + after);




            if (JudgeSymbol == "：")
            {
                return (JudgeSymbol, int.Parse(before), int.Parse(after));
            }
            else
            {
                return (JudgeTorF(int.Parse(before), int.Parse(after), JudgeSymbol));
            }
            return ("", 0, 0);
        }

        /*ひし形の出力先の選択*/
        private (string, int, int) JudgeTorF(int before, int after, string JudgeSymbol)
        {
            if (JudgeSymbol == "＞")
            {
                if (before > after)
                {
                    return ("0", before, after);
                }
            }
            else if (JudgeSymbol == "＜")
            {
                if (before < after)
                {

                    return ("0", before, after);
                }

            }
            else if (JudgeSymbol == "≧")
            {
                if (before >= after)
                {
                    return ("0", before, after);
                }
            }
            else if (JudgeSymbol == "≦")
            {
                if (before <= after)
                {
                    return ("0", before, after);
                }
            }
            else if (JudgeSymbol == "＝")
            {
                if (before == after)
                {
                    return ("0", before, after);
                }
            }
            else if (JudgeSymbol == "≠")
            {
                if (before != after)
                {
                    return ("0", before, after);
                }
            }
            else
            {
                return ("-1", before, after);
            }


            return ("-1", before, after);

        }

        /*ひし*/
        public string DiamondSeparateCalculate(Dictionary<string, int> VarManagement, string ArithExpression)
        {
            NumberStack = new Stack<int>();
            SymbolStack = new Stack<string>();
            char[] StackText = ArithExpression.ToCharArray();

            System.Diagnostics.Debug.WriteLine(ArithExpression);

            System.Diagnostics.Debug.WriteLine(StackText.Length);
            //与えられた文字列を一文字ずつ分解したから初めから数字と記号でスタックしてく(×÷は計算する)　→　が来たらブレイク
            for (int Tenmade = 0, length = 0, Calculat = 0, Popcount = 0; length < StackText.Length; length++)
            {

                System.Diagnostics.Debug.WriteLine("何週目ですか:" + length);
                System.Diagnostics.Debug.WriteLine("見るのは:" + StackText[length]);

                if (StackText[length].ToString().Equals("→"))
                {

                    for (int a = NumberStack.Count - 1; a > 0; a--)
                    {
                        System.Diagnostics.Debug.WriteLine("残りはこれ:" + a);
                        CalculationMethod();
                    }
                    break;
                }
                else
                {
                    if (StackText[length].ToString().Equals("×") || StackText[length].ToString().Equals("÷"))
                    {
                        if (Calculat == 1)
                        {
                            Popcount++;
                        }

                        if (Calculat >= 2)
                        {
                            CalculationMethod();
                        }

                        SymbolStack.Push(StackText[length].ToString());
                        System.Diagnostics.Debug.WriteLine("中身：" + SymbolStack.ToString());
                        System.Diagnostics.Debug.WriteLine("数値中身：" + NumberStack.ToString());
                        Calculat = 2;
                        Tenmade = 0;
                    }
                    else if (StackText[length].ToString().Equals("＋") || StackText[length].ToString().Equals("－"))
                    {
                        if (Calculat >= 1)
                        {

                            for (; Popcount >= 0; Popcount--)
                            {
                                CalculationMethod();
                            }

                        }
                        Calculat = 1;
                        SymbolStack.Push(StackText[length].ToString());
                        System.Diagnostics.Debug.WriteLine("中身：" + SymbolStack.Peek().ToString());
                        System.Diagnostics.Debug.WriteLine("数値中身：" + NumberStack.Peek().ToString());
                        Tenmade = 0;
                    }
                    else if (StackText[length].ToString().Equals("i") || StackText[length].ToString().Equals("j"))
                    {
                        if (!VarManagement.ContainsKey(StackText[length].ToString()))
                        {
                            VarManagement.Add(StackText[length].ToString(), 0);
                        }
                        else
                        {
                            NumberStack.Push(VarManagement[(StackText[length].ToString())]);
                        }
                    }
                    else
                    {
                        if (Tenmade == 1)
                        {

                            NumberStack.Push(int.Parse(NumberStack.Pop().ToString()) * 10 + int.Parse(StackText[length].ToString()));
                        }
                        else
                        {
                            NumberStack.Push(int.Parse(StackText[length].ToString()));
                        }
                        Tenmade = 1;

                        try
                        {
                            System.Diagnostics.Debug.WriteLine("中身：" + SymbolStack.Peek().ToString());

                        }
                        catch (Exception e)
                        {

                            System.Diagnostics.Debug.WriteLine("数値中身：" + NumberStack.Peek().ToString());
                        }
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("最終結果：" + NumberStack.Peek().ToString());

            return NumberStack.Pop().ToString();

        }


        /*台形の計算*/
        public bool TrapezoidCalculat(Dictionary<string, int> VarManagement, string ArithExpression)
        {
            NumberStack = new Stack<int>();
            SymbolStack = new Stack<string>();
            char[] stackText = ArithExpression.ToCharArray();

            for (int lengthcount = 0, rorl = 0; lengthcount < stackText.Length; lengthcount++)
            {
                if (rorl == 0)
                {

                    if (stackText[lengthcount].ToString() == "＞" ||
                        stackText[lengthcount].ToString() == "＜" ||
                        stackText[lengthcount].ToString() == "≧" ||
                        stackText[lengthcount].ToString() == "≦" ||
                        stackText[lengthcount].ToString() == "＝" ||
                        stackText[lengthcount].ToString() == "≠")
                    {
                        JudgeSymbol = stackText[lengthcount].ToString();
                        rorl = 1;
                    }
                    else
                    {
                        LeftArithExpression = LeftArithExpression + stackText[lengthcount].ToString();
                        System.Diagnostics.Debug.WriteLine("左の中身:       " + LeftArithExpression);
                    }
                }
                else
                {
                    RightArithExpression = RightArithExpression + stackText[lengthcount].ToString();
                    System.Diagnostics.Debug.WriteLine("右の中身:       " + RightArithExpression);
                }
            }
            System.Diagnostics.Debug.WriteLine("前半: " + LeftArithExpression);
            System.Diagnostics.Debug.WriteLine("後半：" + RightArithExpression);

            string before = DiamondSeparateCalculate(VarManagement, LeftArithExpression + "→");
            string after = DiamondSeparateCalculate(VarManagement, RightArithExpression + "→");
            System.Diagnostics.Debug.WriteLine("前半: " + before);
            System.Diagnostics.Debug.WriteLine("後半：" + after);


            return (JudgeTorF2(int.Parse(before), int.Parse(after), JudgeSymbol));

        }

        /*ひし形の出力先の選択*/
        private bool JudgeTorF2(int before, int after, string JudgeSymbol)
        {
            if (JudgeSymbol == "＞")
            {
                if (before > after)
                {
                    return true;
                }
            }
            else if (JudgeSymbol == "＜")
            {
                if (before < after)
                {

                    return true;
                }

            }
            else if (JudgeSymbol == "≧")
            {
                if (before >= after)
                {
                    return true;
                }
            }
            else if (JudgeSymbol == "≦")
            {
                if (before <= after)
                {
                    return true;
                }
            }
            else if (JudgeSymbol == "＝")
            {
                if (before == after)
                {
                    return true;
                }
            }
            else if (JudgeSymbol == "≠")
            {
                if (before != after)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }


            return false;

        }

        /* ”□”の場合 */
        public Dictionary<string, int> SquareCalculate(Dictionary<string, int> VarManagement, string ArithExpression)
        {


            // 必要な工程   形種類の判定

            NumberStack = new Stack<int>();
            SymbolStack = new Stack<string>();

            char[] StackText = ArithExpression.ToCharArray();

            // 最終的な変数　→i　の i的な
            string FinalizdKey;
            FinalizdKey = (StackText[(StackText.Length - 1)]).ToString();

            System.Diagnostics.Debug.WriteLine(ArithExpression);



            System.Diagnostics.Debug.WriteLine(StackText.Length);
            //与えられた文字列を一文字ずつ分解したから初めから数字と記号でスタックしてく(×÷は計算する)　→　が来たらブレイク
            for (int Tenmade = 0, length = 0, Calculat = 0, Popcount = 0; length < StackText.Length; length++)
            {

                System.Diagnostics.Debug.WriteLine("何週目ですか:" + length);
                System.Diagnostics.Debug.WriteLine("見るのは:" + StackText[length]);

                if (StackText[length].ToString().Equals("→"))
                {

                    for (int a = NumberStack.Count - 1; a > 0; a--)
                    {
                        System.Diagnostics.Debug.WriteLine("残りはこれ:" + a);
                        CalculationMethod();
                    }
                    break;
                }
                else
                {
                    if (StackText[length].ToString().Equals("×") || StackText[length].ToString().Equals("÷"))
                    {
                        if (Calculat == 1)
                        {
                            Popcount++;
                        }

                        if (Calculat >= 2)
                        {
                            CalculationMethod();
                        }

                        SymbolStack.Push(StackText[length].ToString());
                        System.Diagnostics.Debug.WriteLine("中身：" + SymbolStack.ToString());
                        System.Diagnostics.Debug.WriteLine("数値中身：" + NumberStack.ToString());
                        Calculat = 2;
                        Tenmade = 0;
                    }
                    else if (StackText[length].ToString().Equals("＋") || StackText[length].ToString().Equals("－"))
                    {
                        if (Calculat >= 1)
                        {

                            for (; Popcount >= 0; Popcount--)
                            {
                                CalculationMethod();
                            }

                        }
                        Calculat = 1;
                        SymbolStack.Push(StackText[length].ToString());
                        System.Diagnostics.Debug.WriteLine("中身：" + SymbolStack.Peek().ToString());
                        System.Diagnostics.Debug.WriteLine("数値中身：" + NumberStack.Peek().ToString());
                        Tenmade = 0;
                    }
                    else if (StackText[length].ToString().Equals("i") || StackText[length].ToString().Equals("j"))
                    {
                        if (!VarManagement.ContainsKey(StackText[length].ToString()))
                        {
                            VarManagement.Add(StackText[length].ToString(), 0);
                        }
                        else
                        {
                            NumberStack.Push(VarManagement[(StackText[length].ToString())]);
                        }
                    }
                    else
                    {
                        if (Tenmade == 1)
                        {

                            NumberStack.Push(int.Parse(NumberStack.Pop().ToString()) * 10 + int.Parse(StackText[length].ToString()));
                        }
                        else
                        {
                            NumberStack.Push(int.Parse(StackText[length].ToString()));
                        }
                        Tenmade = 1;

                        try
                        {
                            System.Diagnostics.Debug.WriteLine("中身：" + SymbolStack.Peek().ToString());

                        }
                        catch (Exception e)
                        {

                            System.Diagnostics.Debug.WriteLine("数値中身：" + NumberStack.Peek().ToString());
                        }
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("最終結果：" + NumberStack.Peek().ToString());
            VarManagement[FinalizdKey.ToString()] = int.Parse(NumberStack.Pop().ToString());

            return VarManagement;

        }

        /*計算部分*/
        public void CalculationMethod()
        {

            int Before;
            int After;
            try
            {
                Before = int.Parse(NumberStack.Pop().ToString());


            }
            catch (Exception e)
            {
                Before = int.Parse(NumberStack.Pop().ToString());
            }

            try
            {
                After = int.Parse(NumberStack.Pop().ToString());


            }
            catch (Exception e)
            {

                After = int.Parse(NumberStack.Pop().ToString());
            }



            if (SymbolStack.Peek().ToString().Equals("×"))
            {
                SymbolStack.Pop();

                NumberStack.Push(After * Before);
            }
            else if (SymbolStack.Peek().ToString().Equals("÷"))
            {
                SymbolStack.Pop();
                NumberStack.Push(After / Before);

            }
            else if (SymbolStack.Peek().ToString().Equals("＋"))
            {
                SymbolStack.Pop();
                NumberStack.Push(After + Before);
            }
            else if (SymbolStack.Peek().ToString().Equals("－"))
            {
                SymbolStack.Pop();
                NumberStack.Push(After - Before);

            }
        }

        /*平行四辺形の判定*/
        public string ParallelogramOutput(string OutData)
        {
            return OutData.Substring(0, 1);
        }
    }
}