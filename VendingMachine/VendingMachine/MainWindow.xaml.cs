using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VendingMachine
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            ShowChangeBalance();
            ShowAssortment();
            ShowUserPurse();
        }

        void ShowChangeBalance()
        {
            using (VMDatabaseEntities context = new VMDatabaseEntities())
            {
                var query = from b in context.VendingMachineChangePurses
                            select b;

                foreach (var item in query)
                {
                    VMChangeListView.Items.Add(new VendingMachineChangePurse { NominalValue = item.NominalValue, NumberOfBills = item.NumberOfBills });
                }
            }
        }

        void ShowAssortment()
        {
            using (VMDatabaseEntities context = new VMDatabaseEntities())
            {
                var query = from b in context.VendingMachineAssortments
                            select b;

                foreach (var item in query)
                {
                    AssortmentListView.Items.Add(new VendingMachineAssortment { ProductName = item.ProductName, ProductAmount = item.ProductAmount, ProductCost = item.ProductCost });
                }
            }
        }

        void ShowUserPurse()
        {
            using (VMDatabaseEntities context = new VMDatabaseEntities())
            {
                var query = from b in context.UserPurses
                            select b;

                foreach (var item in query)
                {
                    UserPurseListView.Items.Add(new UserPurse { NominalValue = item.NominalValue, NumberOfBills = item.NumberOfBills });
                }
            }
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            var f = AssortmentListView.SelectedIndex + 1;

            using (var context = new VMDatabaseEntities())
            {
                var query = from st in context.VendingMachineAssortments where st.Id == f select st;

                var queryResult = query.FirstOrDefault<VendingMachineAssortment>();

                var balance = Convert.ToInt32(BalanceTextblock.Text);

                if (balance >= queryResult.ProductCost && queryResult.ProductAmount > 0)
                {
                    queryResult.ProductAmount -= 1;

                    context.SaveChanges();

                    AssortmentListView.Items.Clear();

                    ShowAssortment();

                    balance -= queryResult.ProductCost;

                    BalanceTextblock.Text = balance.ToString();

                    MessageBox.Show("Thanks for buying " + queryResult.ProductName);



                }
                else if (balance < queryResult.ProductCost)
                {
                    MessageBox.Show("Insufficient funds in account");
                    return;
                }
                else if (queryResult.ProductAmount == 0)
                {
                    MessageBox.Show("No " + queryResult.ProductName);
                }
            }

        }

        private void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            item.IsSelected = true;

        }

        private void AddMoneytoBalance_Click(object sender, RoutedEventArgs e)
        {

            var f = UserPurseListView.SelectedIndex + 1;

            using (var context = new VMDatabaseEntities())
            {
                var query = from st in context.UserPurses where st.Id == f select st;

                var queryResult = query.FirstOrDefault<UserPurse>();

                if (queryResult.NumberOfBills > 0)
                {
                    var balanceNow = Convert.ToInt32(BalanceTextblock.Text);

                    queryResult.NumberOfBills -= 1;
                    context.SaveChanges();

                    BalanceTextblock.Text = (balanceNow + queryResult.NominalValue).ToString();

                    UserPurseListView.Items.Clear();

                    ShowUserPurse();
                }

                else if (queryResult.NumberOfBills == 0)
                {
                    MessageBox.Show("Not enough bills");
                    return;
                }
            }
        }

        private void ChangeGiving_Click(object sender, RoutedEventArgs e)
        {
            var balance = Convert.ToInt32(BalanceTextblock.Text);

            using (var context = new VMDatabaseEntities())
            {
                if (balance == 0)
                {
                    MessageBox.Show("balance 0");
                    return;
                }
                else if (balance > 0)
                {

                    var queryVMChange = from st in context.VendingMachineChangePurses
                                        select st;

                    foreach (var item in queryVMChange)
                    {
                        while (balance / item.NominalValue >= 1 && balance != 0)
                        {
                            var query = from st in context.UserPurses where st.NominalValue == item.NominalValue select st;

                            var queryResult = query.FirstOrDefault<UserPurse>();

                            balance -= item.NominalValue;

                            item.NumberOfBills -= 1;

                            queryResult.NumberOfBills += 1;
                        }
                    }

                    BalanceTextblock.Text = balance.ToString();
                    context.SaveChanges();
                    VMChangeListView.Items.Clear();
                    ShowChangeBalance();
                    UserPurseListView.Items.Clear();
                    ShowUserPurse();
                    MessageBox.Show("Change given");
                }
            }
        }
    }
}
