using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

public enum VendingState { INACTIVE, WAITING, DISPENSING }

namespace VendingMachine
{
    public class StateMachine
    {
        private InputReader Reader;

        private int purchasedItem;
        public int PurchasedItem
        {
            get { return purchasedItem; }
            private set
            {
                purchasedItem = value;
                VendingState = value > 0 ? VendingState.DISPENSING : VendingState.INACTIVE;
            }
        }

        private int numCoins;
        public int NumCoins
        {
            get { return numCoins; }
            private set
            {
                numCoins = value;
                VendingState = value > 0 ? VendingState.WAITING : VendingState.INACTIVE;
            }
        }

        private VendingState vendingState = VendingState.INACTIVE;
        public VendingState VendingState
        {
            get { return vendingState; }
            private set
            {
                vendingState = value;
                SetText();
            }
        }

        public StateMachine(int StartingCoins = 0)
        {
            NumCoins = StartingCoins;
            SetText();

            Reader = new InputReader();
            Reader.KeyPressed += Reader_KeyPressed;
            Reader.Activate();
        }

        private void Reader_KeyPressed(object? sender, EventArgs e)
        {
            switch (vendingState)
            {
                case VendingState.INACTIVE:
                    Vending_Inactive();
                    break;
                case VendingState.WAITING:
                    Vending_Waiting();
                    break;
                case VendingState.DISPENSING:
                    Vending_Dispensing();
                    break;
            }

            SetText();
        }

        private void Vending_Inactive(bool checkInput = true)
        {
            switch (Reader.PressedKey)
            {
                case '1':
                    NumCoins++;
                    return;
                case '2':
                    Leave();
                    break;
                default:
                    InvalidCommand();
                    break;
            }
        }

        private void Vending_Waiting()
        {
            switch (Reader.PressedKey)
            {
                case '1':
                    NumCoins++;
                    break;
                case '2':
                    TryBuy(1, 2);
                    break;
                case '3':
                    TryBuy(2, 3);
                    break;
                case '4':
                    ReturnCoins();
                    return;
                case '5':
                    Leave();
                    break;
                default:
                    InvalidCommand();
                    break;
            }
        }

        private void Vending_Dispensing()
        {
            switch (purchasedItem)
            {
                case 1:
                    DispenseItem("bubble gum", 2);
                    break;
                case 2:
                    DispenseItem("granola", 3);
                    break;
            }

            ReadKey();
            PurchasedItem = 0;
            NumCoins = 0;
        }

        private void SetText()
        {
            Clear();

            switch (vendingState)
            {
                case VendingState.INACTIVE:
                    WriteLine("The vending machine has two options: bubble gum ($0.50) and granola ($0.75).\nThe machine only takes quarters. What would you like to do?\n\n1. Insert a Quarter\n2. Leave");
                    return;
                case VendingState.WAITING:
                    WriteLine($"You have inserted ${(numCoins * 0.25).ToString("F2")}. What would you like to do?\n\n1. Insert Another Quarter\n2. Purchase Bubble Gum ($0.50)\n3. Purchase Granola ($0.75)\n4. Return Quarters\n5. Leave");
                    return;
                case VendingState.DISPENSING:
                    Vending_Dispensing();
                    break;
            }
        }

        private void Leave()
        {
            Clear();
            WriteLine("You left the vending machine.");
            Environment.Exit(0);
        }

        private void InvalidCommand()
        {
            Clear();
            WriteLine("Invalid command, please try again.");
            ReadKey();
        }

        public void ReturnCoins()
        {
            Clear();
            WriteLine("The machine returned all of your coins.");
            ReadKey();
            NumCoins = 0;
        }

        public void TryBuy(int item, int price)
        {
            if (numCoins >= price)
            {
                PurchasedItem = item;
            }
            else
            {
                Clear();
                WriteLine("You cannot afford that, please try again.");
                ReadKey();
            }
        }

        public void DispenseItem(string itemName, int price)
        {
            WriteLine($"You purchased the {itemName} for ${(price * 0.25).ToString("F2")}.");
            if (NumCoins > price)
            {
                WriteLine($"You were given back {NumCoins - price} quarter(s) as change.");
            }
        }
    }
}
