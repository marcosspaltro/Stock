﻿using Stock.Clases.Herramientas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stock.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Stock_CarneView : CarouselPage
    {


        public Stock_CarneView()
        {
            InitializeComponent();

        }

        private void Entry_Completed(object sender, System.EventArgs e)
        {
            var id = (Entry)sender;
            Clases.Stock st = (Clases.Stock)id.BindingContext;

            MessagingCenter.Send(this, Literals.DatoModificado, st);
        }
    }
}