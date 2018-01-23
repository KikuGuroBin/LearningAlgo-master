using System;
using Xamarin.Forms;

namespace LearningAlgo
{
    public class MyLayout : AbsoluteLayout
    {
        public EventHandler<DrugEventArgs> LayoutDrug = (s, e) => { };

        public bool DrugFlag { get; set; }

        public String PartsId { get; set; }

    }
}
