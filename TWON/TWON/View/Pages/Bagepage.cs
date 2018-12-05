using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SkiaSharpFormsDemos
{
	public class HomeBasePage : ContentPage
	{
		public HomeBasePage()
		{
			NavigateCommand = new Command<Type>(async (Type pageType) =>
			{
				Page page = (Page)Activator.CreateInstance(pageType);
				await Navigation.PushAsync(page);
			});

			BindingContext = this;
		}

		public ICommand NavigateCommand { private set; get; }

		// This is the I command stuff that should help add new commands to the namespace and these commands can be called as if they already belongsed to the name space long before they were evern inventedd. 
	}
}
