using Reolmarked.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Reolmarked.Model;
using Microsoft.Extensions.Configuration;
using Reolmarked.Data;

namespace Reolmarked.ViewModel
{
    public class RentRackViewModel : INotifyPropertyChanged
    {
        private string _BtnRackColor = "#00a67d";
        private bool _BtnRackBool;
        public ObservableCollection<Renter> Renters { get; set; }
        public ObservableCollection<RentedRack> RentedRacks { get; set; }
        public ObservableCollection<Rack> Racks { get; set; }
        public ObservableCollection<BookingRack> BookingRacks { get; set; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");

        public string BtnRackColor { 
            get => _BtnRackColor; 
            set {
                _BtnRackColor = value;
                OnPropertyChanged();
            } 
        }

        public bool BtnRackBool
        {
            get => _BtnRackBool;
            set
            {
                _BtnRackBool = value;
                OnPropertyChanged();
            }
        }

        public ICommand BtnRackClickedCommand { get; }
        public ICommand BtnBookRackClickedCommand { get; }

        public RentRackViewModel()
        {
            using var context = new AppDbContext(connectionString);
            Renters = new ObservableCollection<Renter>(context.Renter.ToList());
            RentedRacks = new ObservableCollection<RentedRack>(context.RentedRack.ToList());
            if(context.Rack != null && context.RentedRack != null) 
                Racks = new ObservableCollection<Rack>(context.Rack.Where(r => !context.RentedRack.Any(rr => rr.RackNumber == r.RackNumber)).ToList());
            BtnRackClickedCommand = new RelayCommand(BtnRackClicked, canExecuteBtnRackClicked);
            BtnBookRackClickedCommand = new RelayCommand(BtnBookRackClicked, canExecuteBtnBookRackClicked);
            BookingRacks = new ObservableCollection<BookingRack>();

            foreach (Rack rack in context.Rack.ToList())
            {
                if (context.RentedRack.Any(rr => rr.RackNumber == rack.RackNumber))
                    BookingRacks.Add(new BookingRack(rack.RackNumber, true));
                else
                    BookingRacks.Add(new BookingRack(rack.RackNumber, false));
            }
        }

        public void BtnRackClicked(object? index)
        {
            int indexNum = Convert.ToInt32(index);
            var rack = BookingRacks[indexNum];
            rack.RackClicked = !rack.RackClicked;
        }

        public bool canExecuteBtnRackClicked(object? index)
        {
            return true;
        }

        public void BtnBookRackClicked()
        {
            using var context = new AppDbContext(connectionString);
            foreach (BookingRack bookingRack in BookingRacks)
            {
                if (bookingRack.RackClicked)
                {
                    context.RentedRack.Add(new RentedRack(bookingRack.RackNumber, 3, 0));
                    context.SaveChanges();
                }

            }
        }

        public bool canExecuteBtnBookRackClicked()
        {
            return true;
        }



        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
