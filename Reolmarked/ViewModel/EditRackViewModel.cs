using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using Reolmarked.Command;
using Reolmarked.Data;
using Reolmarked.Model;
using Reolmarked.View;

namespace Reolmarked.ViewModel
{
    public class EditRackViewModel
    {
        public Rack RackToEdit { get; set; }
        public ICommand EditRackCommand { get; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");
        public event EventHandler RequestClose;
        public EditRackViewModel(Rack rack)
        {
            RackToEdit = rack;
            EditRackCommand = new RelayCommand(EditRack);
        }
        
        private void EditRack()
        {
            using var context = new AppDbContext(connectionString);
            var rackToEditDb = context.Rack.FirstOrDefault(r => r.RackNumber == RackToEdit.RackNumber);
            rackToEditDb.RackPrice = RackToEdit.RackPrice;
            context.SaveChanges();
            OnRequestClose();
        }

        private void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }


    }
}
