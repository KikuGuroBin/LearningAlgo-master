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
        int s;

        public Test1 ()
		{
			InitializeComponent ();

        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            pane.Children.Add(new Label{Text = "aaa"}, new Rectangle(0, s += 100, 50, 50));
        }
	}
}