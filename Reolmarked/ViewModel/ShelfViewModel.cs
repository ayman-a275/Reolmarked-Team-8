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
            try
            {
                List<ShelfWithTypeName> shelfsWithTypeName = await Task.Run(() =>
                {
                    using var context = DbContextFactory.CreateContext();
                    List<ShelfWithTypeName> shelfsWithTypeName = new List<ShelfWithTypeName>();

                    var shelfs = context.Shelf.ToList();
                    var rentalAgreements = context.RentalAgreement.ToList();
                    var agreementLines = context.AgreementLine.ToList();
                    var shelfTypes = context.ShelfType.ToList();

                    foreach (Shelf shelf in shelfs)
                    {
                        ShelfType shelfType = shelfTypes.FirstOrDefault(sT => sT.ShelfTypeId == shelf.ShelfTypeId);

                        if (shelfType == null)
                        {
                            continue;
                        }

                        decimal? agreedPrice = null;

                        if (shelf.ShelfRented)
                        {
                            AgreementLine agreementLine = agreementLines.FirstOrDefault(aL => aL.ShelfNumber == shelf.ShelfNumber);

                            if (agreementLine != null)
                            {
                                RentalAgreement rentalAgreement = rentalAgreements.FirstOrDefault(rA => rA.RentalAgreementId == agreementLine.RentalAgreementId);

                                if (rentalAgreement != null)
                                {
                                    int shelfCountInAgreement = agreementLines.Count(aL => aL.RentalAgreementId == rentalAgreement.RentalAgreementId);
                                    agreedPrice = rentalAgreement.RentalAgreementTotalPrice / shelfCountInAgreement;
                                }
                            }
                        }

                        shelfsWithTypeName.Add(new ShelfWithTypeName(
                            shelf,
                            shelfType.ShelfTypeName,
                            shelfType.ShelfTypePrice,
                            agreedPrice
                        ));
                    }

                    return shelfsWithTypeName;
                });

                ShelfsWithTypeName = new ObservableCollection<ShelfWithTypeName>(shelfsWithTypeName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved indlæsning af reoler: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditShelf(object? parameter)
        {
            if (parameter is ShelfWithTypeName shelfWithTypeName)
            {
                var editShelfWindow = new EditShelfWindow(shelfWithTypeName.Shelf);
                editShelfWindow.ShowDialog();
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