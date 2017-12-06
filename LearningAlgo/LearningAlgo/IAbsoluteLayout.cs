using System;
using Xamarin.Forms;

namespace LearningAlgo
{
    public class IAbsoluteLayout : AbsoluteLayout
    {
        public IAbsoluteLayout()
        {
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }
    }
}
