using Microsoft.Extensions.Configuration;
using Reolmarked.Command;
using Reolmarked.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Reolmarked.Helper;

namespace Reolmarked.ViewModel
{
    public class MonthlySettlementViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SettlementSummary> SettlementSummaries { get; set; }
        public ObservableCollection<MonthlySettlement> SettlementHistory { get; set; }
        private DateTime _selectedMonth;
        private SettlementSummary _selectedRenter;
        private bool _isHistoryVisible;
        public ICommand GenerateSettlementCommand { get; }
        public ICommand ViewHistoryCommand { get; }
        public ICommand CloseHistoryCommand { get; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");

        public DateTime SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                OnPropertyChanged();
                LoadSettlements();
            }
        }

        public SettlementSummary SelectedRenter
        {
            get => _selectedRenter;
            set
            {
                _selectedRenter = value;
                OnPropertyChanged();
            }
        }

        public bool IsHistoryVisible
        {
            get => _isHistoryVisible;
            set
            {
                _isHistoryVisible = value;
                OnPropertyChanged();
            }
        }

        public MonthlySettlementViewModel()
        {
            SettlementSummaries = new ObservableCollection<SettlementSummary>();
            SettlementHistory = new ObservableCollection<MonthlySettlement>();
            SelectedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            GenerateSettlementCommand = new RelayCommand(GenerateSettlement);
            ViewHistoryCommand = new RelayCommand(ViewHistory);
            CloseHistoryCommand = new RelayCommand(CloseHistory);
            LoadSettlements();
        }

        private void LoadSettlements()
        {
            SettlementSummaries.Clear();

            using var context = DbContextFactory.CreateContext();
            var renters = context.Renter.ToList();
            var rentedShelfs = context.RentedShelf.ToList();

            foreach (var renter in renters)
            {
                var renterShelfs = rentedShelfs.Where(rr => rr.RenterId == renter.RenterId).ToList();
                if (!renterShelfs.Any()) continue;

                decimal totalRent = renterShelfs.Sum(rr => rr.RentedShelfAgreedPrice);

                var soldProducts = context.Product
                    .Where(p => p.ProductSold &&
                               renterShelfs.Select(rr => rr.ShelfNumber).Contains(p.ShelfNumber))
                    .Join(context.TransactionLine, p => p.ProductSerialNumber, tl => tl.ProductSerialNumber, (p, tl) => new { Product = p, TransactionLine = tl })
                    .Join(context.Transaction, ptl => ptl.TransactionLine.TransactionId, t => t.TransactionId, (ptl, t) => new { ptl.Product, Transaction = t })
                    .Where(result => result.Transaction.TransactionDateTime.Year == SelectedMonth.Year &&
                                   result.Transaction.TransactionDateTime.Month == SelectedMonth.Month)
                    .ToList();

                decimal totalSales = soldProducts.Sum(sp => sp.Product.ProductPrice);
                decimal commission = totalSales * 0.10m;

                var summary = new SettlementSummary(renter.RenterId, renter.RenterName, renter.RenterAccountNumber, totalRent, totalSales, commission);
                SettlementSummaries.Add(summary);
            }
        }

        private void GenerateSettlement()
        {
            using var context = new AppDbContext(connectionString);

            foreach (var summary in SettlementSummaries)
            {
                var existingSettlement = context.MonthlySettlement.FirstOrDefault(ms =>
                    ms.RenterId == summary.RenterId &&
                    ms.SettlementDate.Year == SelectedMonth.Year &&
                    ms.SettlementDate.Month == SelectedMonth.Month);

                if (existingSettlement == null)
                {
                    var newSettlement = new MonthlySettlement(
                        summary.RenterId,
                        SelectedMonth,
                        summary.TotalRent,
                        summary.TotalSales,
                        summary.Commission
                    );

                    context.MonthlySettlement.Add(newSettlement);
                }
            }

            context.SaveChanges();
        }

        private void ViewHistory(object parameter)
        {
            if (parameter is SettlementSummary summary)
            {
                SelectedRenter = summary;
                LoadRenterHistory(summary.RenterId);
                IsHistoryVisible = true;
            }
        }

        private void LoadRenterHistory(int renterId)
        {
            SettlementHistory.Clear();

            using var context = new AppDbContext(connectionString);
            var history = context.MonthlySettlement
                .Where(ms => ms.RenterId == renterId)
                .OrderByDescending(ms => ms.SettlementDate)
                .ToList();

            foreach (var settlement in history)
            {
                SettlementHistory.Add(settlement);
            }
        }

        private void CloseHistory()
        {
            IsHistoryVisible = false;
            SelectedRenter = null;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}