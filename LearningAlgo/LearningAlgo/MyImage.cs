using System;
using Xamarin.Forms;

namespace LearningAlgo
{
    public class MyImage : Image
    {
        public EventHandler<DrugEventArgs> Drug = (s, e) => {};

        public bool DrugFlag { get; set; }
    }

    public class MyLayout : AbsoluteLayout
    {
        public EventHandler<DrugEventArgs> LayoutDrug = (s, e) => {};

        public bool DrugFlag { get; set; }
    }
}
