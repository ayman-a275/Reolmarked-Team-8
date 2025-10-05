using Reolmarked.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Reolmarked.Model;
using Reolmarked.Helper;
using System.Windows;

namespace Reolmarked.ViewModel
{
    public class RentShelfViewModel : INotifyPropertyChanged
    {
        private string _BtnShelfColor = "#00a67d";
        private bool _BtnShelfBool;
        private decimal _rentedAgreedPrice;
        private Renter? _selectedRenter;
        private ObservableCollection<RentalAgreementWithDetails> _rentalAgreements;
        public ObservableCollection<Renter> Renters { get; set; }
        public ObservableCollection<AgreementLine> AgreementLine { get; set; }
        public ObservableCollection<Shelf> Shelfs { get; set; }
        public ObservableCollection<BookingShelf> BookingShelfs { get; set; }
        public ICommand BtnShelfClickedCommand { get; }
        public ICommand BtnBookShelfClickedCommand { get; }
        public ICommand DeleteRentalAgreementCommand { get; }

        public string BtnShelfColor
        {
            get => _BtnShelfColor;
            set
            {
                _BtnShelfColor = value;
                OnPropertyChanged();
            }
        }

        public bool BtnShelfBool
        {
            get => _BtnShelfBool;
            set
            {
                _BtnShelfBool = value;
                OnPropertyChanged();
            }
        }

        public decimal RentedAgreedPrice
        {
            get => _rentedAgreedPrice;
            set
            {
                _rentedAgreedPrice = value;
                OnPropertyChanged();
            }
        }

        public Renter? SelectedRenter
        {
            get => _selectedRenter;
            set
            {
                _selectedRenter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RentalAgreementWithDetails> RentalAgreements
        {
            get => _rentalAgreements;
            set
            {
                _rentalAgreements = value;
                OnPropertyChanged();
            }
        }

        public RentShelfViewModel()
        {
            using var context = DbContextFactory.CreateContext();
            AgreementLine = new ObservableCollection<AgreementLine>(context.AgreementLine.ToList());
            RentalAgreements = new ObservableCollection<RentalAgreementWithDetails>();
            Renters = new ObservableCollection<Renter>(context.Renter.Where(r => !context.RentalAgreement.Any(rA => r.RenterId == rA.RenterId)).ToList());
            Shelfs = new ObservableCollection<Shelf>(context.Shelf.ToList());
            BtnShelfClickedCommand = new RelayCommand(BtnShelfClicked, canExecuteBtnShelfClicked);
            BtnBookShelfClickedCommand = new RelayCommand(BtnBookShelfClicked);
            DeleteRentalAgreementCommand = new RelayCommand(DeleteRentalAgreement);
            BookingShelfs = new ObservableCollection<BookingShelf>();

            foreach (Shelf shelf in Shelfs)
            {
                if (shelf.ShelfRented)
                    BookingShelfs.Add(new BookingShelf(shelf.ShelfNumber, true));
                else
                    BookingShelfs.Add(new BookingShelf(shelf.ShelfNumber, false));
            }

            LoadRentalAgreements();
        }

        private void LoadRentalAgreements()
        {
            try
            {
                using var context = DbContextFactory.CreateContext();
                var rentalAgreementsList = new ObservableCollection<RentalAgreementWithDetails>();

                var rentalAgreements = context.RentalAgreement.ToList();
                var renters = context.Renter.ToList();
                var agreementLines = context.AgreementLine.ToList();

                foreach (var agreement in rentalAgreements)
                {
                    var renter = renters.FirstOrDefault(r => r.RenterId == agreement.RenterId);
                    var shelfNumbers = agreementLines
                        .Where(al => al.RentalAgreementId == agreement.RentalAgreementId)
                        .Select(al => al.ShelfNumber)
                        .ToList();

                    var agreementWithDetails = new RentalAgreementWithDetails(
                        agreement.RentalAgreementId,
                        agreement.RenterId,
                        renter?.RenterName ?? "Ukendt lejer",
                        agreement.RentalAgreementDate,
                        agreement.RentalAgreementTotalPrice,
                        shelfNumbers
                    );
                    rentalAgreementsList.Add(agreementWithDetails);
                }

                RentalAgreements = rentalAgreementsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved indlæsning af lejeaftaler: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void BtnShelfClicked(object? index)
        {
            int indexNum = Convert.ToInt32(index);
            var shelf = BookingShelfs[indexNum];
            shelf.ShelfClicked = !shelf.ShelfClicked;
        }

        public bool canExecuteBtnShelfClicked(object? index)
        {
            using var context = DbContextFactory.CreateContext();
            int indexNum = Convert.ToInt32(index);
            var shelf = BookingShelfs[indexNum];
            Shelf shelfToCheck = context.Shelf.FirstOrDefault(r => r.ShelfNumber == shelf.ShelfNumber);
            if (shelfToCheck.ShelfRented)
                return false;
            return true;
        }

        public void BtnBookShelfClicked()
        {
            if (SelectedRenter == null)
            {
                MessageBox.Show("Du skal vælge en lejer.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!BookingShelfs.Any(b => b.ShelfClicked))
            {
                MessageBox.Show("Du skal have valgt mindst 1 reol.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (RentedAgreedPrice <= 0)
            {
                MessageBox.Show("Aftalt pris skal være større end 0.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using var context = DbContextFactory.CreateContext();

                var selectedShelfs = BookingShelfs.Where(b => b.ShelfClicked).ToList();

                RentalAgreement newRentalAgreement = new RentalAgreement(SelectedRenter.RenterId, DateTime.Now, RentedAgreedPrice);

                context.RentalAgreement.Add(newRentalAgreement);
                context.SaveChanges();

                foreach (BookingShelf bookingShelf in selectedShelfs)
                {
                    Shelf shelfToChange = context.Shelf.FirstOrDefault(r => r.ShelfNumber == bookingShelf.ShelfNumber);
                    if (shelfToChange != null)
                    {
                        shelfToChange.ShelfRented = true;

                        AgreementLine newAgreementLine = new AgreementLine(
                            bookingShelf.ShelfNumber,
                            newRentalAgreement.RentalAgreementId
                        );
                        context.AgreementLine.Add(newAgreementLine);
                    }
                }

                context.SaveChanges();

                MessageBox.Show($"Lejeaftale oprettet succesfuldt!\n\nLejer: {SelectedRenter.RenterName}\nAntal reoler: {selectedShelfs.Count}\nAftalt pris: {RentedAgreedPrice:C}", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                foreach (var bookingShelf in selectedShelfs)
                {
                    bookingShelf.ShelfClicked = false;
                    bookingShelf.ShelfBooked = true;
                }

                RentedAgreedPrice = 0;
                SelectedRenter = null;

                LoadRentalAgreements();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved oprettelse af lejeaftale: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteRentalAgreement(object? parameter)
        {
            if (parameter is RentalAgreementWithDetails agreementDetails)
            {
                var result = MessageBox.Show($"Er du sikker på at du vil slette lejeaftalen for {agreementDetails.RenterName}? Dette vil også frigøre alle tilknyttede reoler.", "Bekræft sletning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using var context = DbContextFactory.CreateContext();

                        var agreementToDelete = context.RentalAgreement.FirstOrDefault(ra => ra.RentalAgreementId == agreementDetails.RentalAgreementId);

                        if (agreementToDelete != null)
                        {
                            var agreementLines = context.AgreementLine.Where(al => al.RentalAgreementId == agreementDetails.RentalAgreementId).ToList();

                            foreach (var agreementLine in agreementLines)
                            {
                                var shelf = context.Shelf.FirstOrDefault(s => s.ShelfNumber == agreementLine.ShelfNumber);
                                if (shelf != null)
                                {
                                    shelf.ShelfRented = false;
                                }
                                context.AgreementLine.Remove(agreementLine);
                            }

                            context.RentalAgreement.Remove(agreementToDelete);
                            context.SaveChanges();

                            MessageBox.Show("Lejeaftale slettet succesfuldt!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                            BookingShelfs.Clear();
                            Shelfs = new ObservableCollection<Shelf>(context.Shelf.ToList());
                            foreach (Shelf shelf in Shelfs)
                            {
                                if (shelf.ShelfRented)
                                    BookingShelfs.Add(new BookingShelf(shelf.ShelfNumber, true));
                                else
                                    BookingShelfs.Add(new BookingShelf(shelf.ShelfNumber, false));
                            }

                            LoadRentalAgreements();
                        }
                        else
                        {
                            MessageBox.Show("Kunne ikke finde lejeaftale i databasen.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fejl ved sletning af lejeaftale: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}