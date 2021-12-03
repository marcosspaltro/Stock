using Stock.Clases.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stock.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class vw_Stock : ContentPage
    {
        public vw_Stock()
        {
            InitializeComponent();
        }

        private void Entry_Completed(object sender, System.EventArgs e)
        {
            var id = (Entry)sender;
            Clases.Stock st = (Clases.Stock)id.BindingContext;
            st.Asignar_Tipo();
            MessagingCenter.Send(this, Literals.DatoModificado, st);
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                Entry MyEntry = (Entry)sender;
                MyEntry.CursorPosition = 0;
                MyEntry.SelectionLength = MyEntry.Text != null ? MyEntry.Text.Length : 0;
            });
        }


        async void Refresh_grd(object sender, EventArgs e)
        {
            string stockt = lbl1.Text.Substring(6);
            Clases.Stock st = new Clases.Stock();
            st.Asignar_Tipo(stockt);
            MessagingCenter.Send(this, Literals.Actualizar_grd, st);
            My_Refresh_grd.IsRefreshing = false;
        }
    }
}