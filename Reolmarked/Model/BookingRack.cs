using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class BookingRack : INotifyPropertyChanged
    {
        private int _rackNumber;
        private bool _rackBooked;
        private bool _rackClicked;
        private string _rackColor;

        public BookingRack(int rackNumber, bool rackBooked)
        {
            RackNumber = rackNumber;
            RackColor = "#00a67d";
            RackBooked = rackBooked;
            RackClicked = false;
        }

        public int RackNumber
        {
            get => _rackNumber;
            set
            {
                _rackNumber = value;
                OnPropertyChanged();
            }
        }

        public bool RackBooked
        {
            get => _rackBooked;
            set
            {
                _rackBooked = value;
                if(RackBooked)
                    RackColor = "#cb2d6f";
                OnPropertyChanged();
            }
        }

        public bool RackClicked
        {
            get => _rackClicked;
            set
            {
                _rackClicked = value;
                if (RackClicked && !RackBooked)
                    RackColor = "#0600d1";
                if (!RackClicked && !RackBooked)
                    RackColor = "#00a67d";
                OnPropertyChanged();
            }
        }

        public string RackColor
        {
            get => _rackColor;
            set
            {
                _rackColor = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
