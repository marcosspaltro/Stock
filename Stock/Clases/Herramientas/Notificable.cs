using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Stock.Clases.Herramientas
{
    public abstract class Notificable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// CallerMemberName devuelve el nombre de la propiedad cambiada.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
