using System;

using Xamarin.Forms;

namespace LearningAlgo
{
    public class FlowParts : AbsoluteLayout
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:LearningAlgo.FlowParts"/> edit flag.
        /// </summary>
        /// <value><c>true</c> if edit flag; otherwise, <c>false</c>.</value>
        public bool EditFlag { get; set; }

        private ScrollView LabelScroller { get; set; }

        public FlowParts()
        {
            Children.Add(new Image{});

            LabelScroller = new ScrollView
            {
                Content = new Label
                {
                    Text = ""
                },
            };
            Children.Add(LabelScroller);
        }

        public void Drug(object sender, DrugEventArgs args)
        {
            this.TranslateTo(args.X, args.Y, 0);
        }
    }
}
