using Reolmarked.Command;
using Reolmarked.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Reolmarked.Helper;
using System.Windows;

namespace Reolmarked.ViewModel
{
    public class MonthlySettlementViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SettlementSummary> SettlementSummaries { get; set; }
        public ICommand MarkAsPaidCommand { get; }

        public MonthlySettlementViewModel()
        {
            SettlementSummaries = new ObservableCollection<SettlementSummary>();
            MarkAsPaidCommand = new RelayCommand(MarkAsPaid);
            LoadSettlements();
        }

        private void LoadSettlements()
        {
            SettlementSummaries.Clear();

            try
            {
                using var context = DbContextFactory.CreateContext();

                var selectedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                var renters = context.Renter.ToList();
                var rentalAgreements = context.RentalAgreement.ToList();
                var agreementLines = context.AgreementLine.ToList();
                var products = context.Product.ToList();
                var transactionLines = context.TransactionLine.ToList();
                var transactions = context.Transaction.Where(t => t.TransactionDateTime.Year == selectedMonth.Year && t.TransactionDateTime.Month == selectedMonth.Month).ToList();

                foreach (var renter in renters)
                {
                    var renterAgreements = rentalAgreements.Where(ra => ra.RenterId == renter.RenterId).ToList();

                    if (!renterAgreements.Any()) continue;

                    decimal totalRent = renterAgreements.Sum(ra => ra.RentalAgreementTotalPrice);

                    var renterShelfNumbers = renterAgreements
                        .SelectMany(ra => agreementLines.Where(al => al.RentalAgreementId == ra.RentalAgreementId))
                        .Select(al => al.ShelfNumber)
                        .ToList();

                    var soldProducts = products
                        .Where(p => p.ProductSold && renterShelfNumbers.Contains(p.ShelfNumber))
                        .ToList();

                    var soldProductSerialNumbers = soldProducts.Select(p => p.ProductSerialNumber).ToList();

                    var relevantTransactionIds = transactionLines
                        .Where(tl => soldProductSerialNumbers.Contains(tl.ProductSerialNumber))
                        .Select(tl => tl.TransactionId)
                        .Distinct()
                        .ToList();

                    var relevantTransactions = transactions
                        .Where(t => relevantTransactionIds.Contains(t.TransactionId))
                        .ToList();

                    decimal totalSales = 0;
                    foreach (var transactionId in relevantTransactionIds)
                    {
                        var transaction = relevantTransactions.FirstOrDefault(t => t.TransactionId == transactionId);
                        if (transaction != null)
                        {
                            var productsSoldInTransaction = transactionLines
                                .Where(tl => tl.TransactionId == transactionId && soldProductSerialNumbers.Contains(tl.ProductSerialNumber))
                                .ToList();

                            foreach (var tl in productsSoldInTransaction)
                            {
                                var product = soldProducts.FirstOrDefault(p => p.ProductSerialNumber == tl.ProductSerialNumber);
                                if (product != null)
                                {
                                    totalSales += product.ProductPrice;
                                }
                            }
                        }
                    }

                    decimal commission = totalSales * 0.10m;
                    decimal netAmount = totalSales - commission - totalRent;

                    if (netAmount != 0)
                    {
                        var summary = new SettlementSummary(
                            renter.RenterId,
                            renter.RenterName,
                            renter.RenterAccountNumber,
                            totalRent,
                            totalSales,
                            commission,
                            false
                        );

                        SettlementSummaries.Add(summary);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved generering af afregning: {ex.Message}", "Fejl",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MarkAsPaid(object parameter)
        {
            if (parameter is SettlementSummary summary)
            {
                try
                {
                    using var context = DbContextFactory.CreateContext();

                    var selectedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                    var existingSettlement = context.MonthlySettlement.FirstOrDefault(ms =>
                        ms.RenterId == summary.RenterId &&
                        ms.SettlementDate.Year == selectedMonth.Year &&
                        ms.SettlementDate.Month == selectedMonth.Month);

                    if (existingSettlement == null)
                    {
                        var newSettlement = new MonthlySettlement(
                            summary.RenterId,
                            selectedMonth,
                            summary.TotalRent,
                            summary.TotalSales,
                            summary.Commission
                        );
                        newSettlement.IsPaid = true;

                        context.MonthlySettlement.Add(newSettlement);
                        context.SaveChanges();
                    }
                    else
                    {
                        existingSettlement.IsPaid = true;
                        context.SaveChanges();
                    }

                    SettlementSummaries.Remove(summary);

                    MessageBox.Show($"Afregning for {summary.RenterName} markeret som betalt.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fejl ved markering som betalt: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
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