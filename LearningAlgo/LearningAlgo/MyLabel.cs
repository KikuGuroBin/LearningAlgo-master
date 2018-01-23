using System;
using Xamarin.Forms;

namespace LearningAlgo
{
    public class MyLabel : Label
    {
        public EventHandler<DrugEventArgs> Drug = (s, e) => { };

        public int LabelId { get; set; }
    }
}
