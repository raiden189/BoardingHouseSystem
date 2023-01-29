using BoardingHouseSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoardingHouseSystem.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StudentBoardingView : ContentPage
	{
		public StudentBoardingView ()
		{
			InitializeComponent ();
            this.BindingContext = new StudentBoardingViewModel();
        }
	}
}