using Microsoft.Extensions.Configuration;
using Reolmarked.Command;
using Reolmarked.Helper;
using Reolmarked.Model;
using Reolmarked.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Reolmarked.ViewModel
{
    public class ShelfViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ShelfWithTypeName> _shelfsWithTypeName;

        public ICommand EditShelfCommand { get; }

        public ObservableCollection<ShelfWithTypeName> ShelfsWithTypeName
        {
            get => _shelfsWithTypeName;
            set
            {
                _shelfsWithTypeName = value;
                OnPropertyChanged();
            }
        }

        public ShelfViewModel()
        {
            LoadShelfsAsync();
            EditShelfCommand = new RelayCommand(EditShelf);
        }

        private async void LoadShelfsAsync()
        {
            using var context = DbContextFactory.CreateContext();

            try
            {
                List<ShelfWithTypeName> shelfsWithTypeName = await Task.Run(() =>
                {
                    List<ShelfWithTypeName> shelfsWithTypeName = new List<ShelfWithTypeName>();
                    ShelfType shelfTypeToUse;
                    RentalAgreement rentedShelfToUse;
                    var shelfs = context.Shelf.ToList();
                    var rentedShelf = context.RentedShelf.ToList();
                    var shelfTypeName = context.ShelfType.ToList();
                    foreach (Shelf shelf in shelfs)
                    {
                        shelfTypeToUse = shelfTypeName.FirstOrDefault(sT => sT.ShelfTypeId == shelf.ShelfTypeId);
                        rentedShelfToUse = rentedShelf.FirstOrDefault(rS => rS.ShelfNumber == shelf.ShelfNumber);
                        shelfsWithTypeName.Add(new ShelfWithTypeName(shelf, shelfTypeToUse.ShelfTypeName, shelfTypeToUse.ShelfTypePrice, rentedShelfToUse?.RentedShelfAgreedPrice));
                    }
                        
                    return shelfsWithTypeName;
                });

                ShelfsWithTypeName = new ObservableCollection<ShelfWithTypeName>(shelfsWithTypeName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved forbindelse til database: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditShelf(object? parameter)
        {
            if (parameter is ShelfWithTypeName shelfWithTypeName)
            {
                var editShelfWindow = new EditShelfWindow(shelfWithTypeName.Shelf);
                editShelfWindow.ShowDialog();
                OnPropertyChanged();
                LoadShelfsAsync();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}