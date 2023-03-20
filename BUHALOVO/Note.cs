using System;

namespace BUHALOVO
{
    class Note
    {
        private string name;
        private string type;
        private double amountOfMoney;
        private bool isReceipt;
        private DateOnly date;

        public Note(string name, string type, double amountOfMoney, bool isReceipt, DateOnly date)
        {
            this.Name = name;
            this.Type = type;
            this.AmountOfMoney = amountOfMoney;
            this.IsReceipt = isReceipt;
            this.Date = date;
        }

        public string Name 
        {
            get { return name; }
            set { name = value; } 
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public double AmountOfMoney
        {
            get { return amountOfMoney; }
            set { amountOfMoney = value; }
        }

        public bool IsReceipt 
        { 
            get { return isReceipt; }
            set
            {
                if (AmountOfMoney > 0) isReceipt = true;
                else isReceipt = false;
            } 
        }
        public DateOnly Date 
        { 
            get { return date; }
            set { date = value; } 
        }
    }
}
