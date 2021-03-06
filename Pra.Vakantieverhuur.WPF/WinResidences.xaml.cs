﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Pra.Vakantieverhuur.CORE.Entities;
using Pra.Vakantieverhuur.CORE.Services;

namespace Pra.Vakantieverhuur.WPF
{
    /// <summary>
    /// Interaction logic for WinResidences.xaml
    /// </summary>
    public partial class WinResidences : Window
    {
        public WinResidences()
        {
            InitializeComponent();
        }

        public string status = "";
        public Residence selectedResidence = null;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (status == "view")
            {
                grpData.IsEnabled = false;
                lblTitle.Content = "Verblijf bekijken";
                btnSave.Visibility = Visibility.Hidden;
                btnCancel.Content = "Sluiten";
            }
            else
            {
                grpData.IsEnabled = true;
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Content = "Annuleren";
                if (status == "new")
                {
                    lblTitle.Content = "Verblijf toevoegen";
                }
                else
                {
                    lblTitle.Content = "Verblijf wijzigen";
                }
            }
            if (selectedResidence == null)
            {
                cmbKindOfResidence.SelectedIndex = 0;
                txtHouseName.Text = "";
                txtStreetAndNumber.Text = "";
                txtPostalCode.Text = "";
                txtTown.Text = "";
                txtBasePrice.Text = "";
                txtReducedPrice.Text = "";
                txtDaysForReduction.Text = "";
                txtDeposit.Text = "";
                txtMaxPersons.Text = "";
                chkMicrowave.IsChecked = false;
                chkTV.IsChecked = false;
                chkDishwasher.IsChecked = false;
                chkWashingMachine.IsChecked = false;
                chkWoodStove.IsChecked = false;
                chkPrivateSanitaryBlock.IsChecked = false;
                chkPrivateSanitaryBlock.Visibility = Visibility.Hidden;
                chkRentable.IsChecked = false;
                cmbKindOfResidence.IsEnabled = true;
            }
            else
            {
                if (selectedResidence is HolidayHome)
                {
                    cmbKindOfResidence.SelectedIndex = 0;
                    chkPrivateSanitaryBlock.Visibility = Visibility.Hidden;
                    chkDishwasher.Visibility = Visibility.Visible;
                    chkWashingMachine.Visibility = Visibility.Visible;
                    chkWoodStove.Visibility = Visibility.Visible;

                    chkDishwasher.IsChecked = ((HolidayHome)selectedResidence).DishWasher;
                    chkWashingMachine.IsChecked = ((HolidayHome)selectedResidence).WashingMachine;
                    chkWoodStove.IsChecked = ((HolidayHome)selectedResidence).WoodStove;
                    chkPrivateSanitaryBlock.IsChecked = false;

                }
                else
                {
                    cmbKindOfResidence.SelectedIndex = 1;
                    chkPrivateSanitaryBlock.Visibility = Visibility.Visible;
                    chkDishwasher.Visibility = Visibility.Hidden;
                    chkWashingMachine.Visibility = Visibility.Hidden;
                    chkWoodStove.Visibility = Visibility.Hidden;

                    chkDishwasher.IsChecked = false;
                    chkWashingMachine.IsChecked = false;
                    chkWoodStove.IsChecked = false;
                    chkPrivateSanitaryBlock.IsChecked = ((Caravan)selectedResidence).PrivateSanitaryBlock;

                }

                txtHouseName.Text = selectedResidence.ResidenceName;
                txtStreetAndNumber.Text = selectedResidence.StreetAndNumber;
                txtPostalCode.Text = selectedResidence.PostalCode.ToString();
                txtTown.Text = selectedResidence.Town;
                txtBasePrice.Text = selectedResidence.BasePrice.ToString();
                txtReducedPrice.Text = selectedResidence.ReducedPrice.ToString();
                txtDaysForReduction.Text = selectedResidence.DaysForReduction.ToString();
                txtDeposit.Text = selectedResidence.Deposit.ToString();
                txtMaxPersons.Text = selectedResidence.MaxPersons.ToString();
                chkMicrowave.IsChecked = selectedResidence.Microwave;
                chkTV.IsChecked = selectedResidence.TV;
                chkRentable.IsChecked = selectedResidence.IsRentable;

                cmbKindOfResidence.IsEnabled = false;
            }
        }

        private void cmbKindOfResidence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsLoaded) return;

            if (cmbKindOfResidence.SelectedIndex == 0)
            {
                chkPrivateSanitaryBlock.Visibility = Visibility.Hidden;
                chkDishwasher.Visibility = Visibility.Visible;
                chkWashingMachine.Visibility = Visibility.Visible;
                chkWoodStove.Visibility = Visibility.Visible;
            }
            else
            {
                chkPrivateSanitaryBlock.Visibility = Visibility.Visible;
                chkDishwasher.Visibility = Visibility.Hidden;
                chkWashingMachine.Visibility = Visibility.Hidden;
                chkWoodStove.Visibility = Visibility.Hidden;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ColorError(Control ctrl)
        {
            ctrl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFBEDED"));
            ctrl.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF0000"));
            ctrl.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF0000"));
        }
        private void ColorOK(Control ctrl)
        {
            ctrl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            ctrl.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            ctrl.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ColorOK(txtHouseName);
            ColorOK(txtStreetAndNumber);
            ColorOK(txtPostalCode);
            ColorOK(txtTown);
            ColorOK(txtBasePrice);
            ColorOK(txtReducedPrice);
            ColorOK(txtDaysForReduction);
            ColorOK(txtDeposit);
            ColorOK(txtMaxPersons);

            string houseName = txtHouseName.Text.Trim();
            string streetAndNumber = txtStreetAndNumber.Text.Trim();
            int.TryParse(txtPostalCode.Text, out int postalCode);
            string town = txtTown.Text.Trim();
            decimal.TryParse(txtBasePrice.Text, out decimal basePrice);
            decimal.TryParse(txtReducedPrice.Text, out decimal reducedPrice);
            byte.TryParse(txtDaysForReduction.Text, out byte daysForReduction);
            decimal.TryParse(txtDeposit.Text, out decimal deposit);
            int.TryParse(txtMaxPersons.Text, out int maxPersons);
            bool rentable = (bool)chkRentable.IsChecked;
            bool? microwave = chkMicrowave.IsChecked;
            bool? tv = chkTV.IsChecked;
            bool? privateSanitaryBlock = chkPrivateSanitaryBlock.IsChecked;
            bool? dishWasher = chkDishwasher.IsChecked;
            bool? washingMachine = chkWashingMachine.IsChecked;
            bool? woodStove = chkWoodStove.IsChecked;

            bool errors = false;
            if (houseName.Length == 0)
            {
                errors = true;
                ColorError(txtHouseName);
            }
            if (streetAndNumber.Length == 0)
            {
                errors = true;
                ColorError(txtStreetAndNumber);
            }
            if (postalCode == 0)
            {
                errors = true;
                ColorError(txtPostalCode);
            }
            if (town.Length == 0)
            {
                errors = true;
                ColorError(txtTown);
            }
            if (basePrice == 0)
            {
                errors = true;
                ColorError(txtBasePrice);
            }
            if (reducedPrice == 0)
            {
                errors = true;
                ColorError(txtReducedPrice);
            }
            if (daysForReduction == 0)
            {
                errors = true;
                ColorError(txtDaysForReduction);
            }
            if (deposit == 0)
            {
                errors = true;
                ColorError(txtDeposit);
            }
            if (maxPersons == 0)
            {
                errors = true;
                ColorError(txtMaxPersons);
            }

            if (errors)
                return;

            if (status == "new")
            {
                if (cmbKindOfResidence.SelectedIndex == 0)
                    selectedResidence = new HolidayHome();
                else
                    selectedResidence = new Caravan();
            }
            selectedResidence.ResidenceName = houseName;
            selectedResidence.StreetAndNumber = streetAndNumber;
            selectedResidence.PostalCode = postalCode;
            selectedResidence.Town = town;
            selectedResidence.BasePrice = basePrice;
            selectedResidence.ReducedPrice = reducedPrice;
            selectedResidence.DaysForReduction = daysForReduction;
            selectedResidence.Deposit = deposit;
            selectedResidence.MaxPersons = maxPersons;
            selectedResidence.IsRentable = rentable;
            selectedResidence.Microwave = microwave;
            selectedResidence.TV = tv;
            if (selectedResidence is HolidayHome)
            {
                ((HolidayHome)selectedResidence).DishWasher = dishWasher;
                ((HolidayHome)selectedResidence).WashingMachine = washingMachine;
                ((HolidayHome)selectedResidence).WoodStove = woodStove;
            }
            else
            {
                ((Caravan)selectedResidence).PrivateSanitaryBlock = privateSanitaryBlock;
            }
            this.Close();
        }
    }
}
