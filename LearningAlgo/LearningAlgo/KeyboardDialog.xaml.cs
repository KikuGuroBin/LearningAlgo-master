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

        public KeyboardDialog()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            LabelBounds = new ViewBounds
            {
                X = width / 2 - 30,
                Y = height / 2,
            };

            //Label1.BindingContext = LabelBounds;
        }
    }
}
