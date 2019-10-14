using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;
using System.Net.Mail;

/* The Simple Bank Management System using C# Console Application
 * Bank account details, user data, banking transaction etx are to be stored in files
 * 
 */
namespace onlineBankingSystem
{
    class BankSystem
    {
        static void Main(string[] args)
        {

            //bank constuctor parameters for bank name, userdata file
            Bank bank = new Bank("login.txt"); 
            bank.menu();
            Console.ReadKey();
        }
    }

    class Bank
    {
        //attributes 
        private User user;
        private AccountFunc accountFunc;
        private bool isLoggedIn = false;

        //constructor 
        public Bank(String user)
        {
            this.user = new User(user);
            this.accountFunc = new AccountFunc();
        }
         
        //main menu options
        public void menu()
        {
            try
            {                
                if (!isLoggedIn)
                {
                    user.login();
                    isLoggedIn = true;
                }
                menuUI();
                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        m_create_account();
                        break;
                    case 2:
                        m_search_account();
                        break;
                    case 3:
                        m_deposit();
                        break;
                    case 4:
                        m_withdraw();
                        break;
                    case 5:
                        m_statement();
                        break;
                    case 6:
                        m_delete_account();
                        break;
                    case 7:
                        m_exit();
                        break;
                    default:
                        m_error();
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("wrong format");
                Console.ReadKey(true);
                menu();
            }
        }

        private void m_create_account()
        {
            
            accountFunc.create_account();
            menu();
        }
        private void m_search_account()
        {
                    
            accountFunc.search_account();
            menu();
        }
        private void m_deposit()
        {
            
            accountFunc.updateBalance(true);
            menu();
        }
        private void m_withdraw()
        {
            
            accountFunc.updateBalance(false);
            menu();
        }
        private void m_statement()
        {
            
            accountFunc.statement();
            menu();
        }
        private void m_delete_account()
        {
            
            accountFunc.delete_account();
            menu();
        }
        private void m_exit()
        {
            isLoggedIn = false;
            menu();
        }
        private void m_error()
        {
            Console.WriteLine("invalid input");
            menu();
        }
        
        private void menuUI()
        {
            Console.Clear();
            Bordering.Draw(Border.TOP);
            Bordering.Draw(Border.TEXT, "WELCOME TO SIMPLE BANKING SYSTEM", true);
            Bordering.Draw(Border.MID);
            Bordering.Draw(Border.TEXT, "1. Create a new accont");
            Bordering.Draw(Border.TEXT, "2. Search for an accont");
            Bordering.Draw(Border.TEXT, "3. Deposit");
            Bordering.Draw(Border.TEXT, "4. Withdraw");
            Bordering.Draw(Border.TEXT, "5. A/C statement");
            Bordering.Draw(Border.TEXT, "6. Delete account");
            Bordering.Draw(Border.TEXT, "7. Exit");
            Bordering.Draw(Border.MID);
            Bordering.Draw(Border.TEXT, "Enter your choice (1-7): ",false,0);
            Bordering.Draw(Border.BOT,"",false,9);
            Bordering.setPos(0);
        }
    }
    class User
    {
        private String user_name;
        private String password;
        private String[] login_f;

        public User(String file)
        {
            login_f = File.ReadAllLines(file);
        }

        public void login()
        {
            Console.Clear();
            Bordering.Draw(Border.TOP);
            Bordering.Draw(Border.TEXT, "WELCOME TO SIMPLE BANKING SYSTEM", true);
            Bordering.Draw(Border.MID);
            Bordering.Draw(Border.TEXT, "LOGIN TO START", true);
            Bordering.Draw(Border.TEXT, "  ", true);
            Bordering.Draw(Border.TEXT, "User Name: ",false,0); 
            Bordering.Draw(Border.TEXT, "Password: ",false,1); 
            Bordering.Draw(Border.BOT,"",false,9);

            Bordering.setPos(0); user_name = Console.ReadLine();
            Bordering.setPos(1); readPassword();

            Bordering.setPos(9);
            Console.WriteLine("");

            if (exists(user_name)){
                if (matches(user_name, password)){
                    
                    Console.WriteLine("Valid credentials!... Please enter");
                    Console.ReadKey(true);
                }
                else {
                    Console.WriteLine("Password does not match with username.");
                    Console.ReadKey(true);
                    login();
                }
            }
            else {
                Console.WriteLine("Username dose not exist.");
                Console.ReadKey(true);
                login();
            }
                
        }

        private void readPassword()
        {

            ConsoleKeyInfo currentKeyInfo = new ConsoleKeyInfo();
            string value = "";
            int[] currentPos = Bordering.getPos(1);
            while (currentKeyInfo.Key != ConsoleKey.Enter)
            {
                currentKeyInfo = Console.ReadKey(true);

                if (currentKeyInfo.Key == ConsoleKey.Backspace)
                {
                    if (Console.CursorLeft > currentPos[0])
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        value = value.Remove(value.Length-1);
                    }
                }
                else if (currentKeyInfo.Key != ConsoleKey.Enter)
                {
                    Console.Write("*");
                    value += currentKeyInfo.KeyChar;
                }

            }

            password = value;
        }

        //check if username is existing in the file
        private bool exists(String user_name)
        {
            foreach (string set in login_f)
            {
                string[] splits = set.Split('|');
                if(splits[0] == user_name)
                return true;
            }
            return false;
        }

        //check if username matches with password
        private bool matches(String user_name, String password)
        {
            foreach (string set in login_f)
            {
                string[] splits = set.Split('|');
                if (splits[0] == user_name && splits[1] == password)
                    return true;        
            }
            return false;
        }
    }

    class AccountFunc
    {

        //public static AccountFunc accountFunc;
        //private Dictionary<int, Account> accountDictionary;
        private BinaryFormatter formatter;
        private Account currentAccount;

        public AccountFunc()
        {
            //this.accountDictionary = new Dictionary<int, Account>();
            this.formatter = new BinaryFormatter();
            this.currentAccount = new Account();
        }

        public void create_account()
        {
            try
            {
                title("CREATE A NEW ACCOUNT");
                Bordering.Draw(Border.TEXT, "First Name: ", false, 0);
                Bordering.Draw(Border.TEXT, "Last Name: ", false, 1);
                Bordering.Draw(Border.TEXT, "Address: ", false, 2);
                Bordering.Draw(Border.TEXT, "Phone: ", false, 3);
                Bordering.Draw(Border.TEXT, "Email: ", false, 4);
                Bordering.Draw(Border.BOT, "", false, 9);
                currentAccount = new Account(getAccountNumber(), readString(0), readString(1),
                    readString(2), readInt(3), validEmail(readString(4)), 0.0);
                //change parameter to the cursor location.
                SaveFile(currentAccount);
                Console.WriteLine("\n\nIs the Information correct (y/n)?");
                Console.WriteLine("\n\nAccount Created! details will be provided via email.");
                Console.WriteLine("Account number is:" + currentAccount.AccountNum);
                Console.ReadKey(true);
                //need validation
            }
            catch (Exception e)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nError :{0}",e.Message);
                Console.ReadKey(true);
            }
        }


        public void search_account()
        {
            try { 
            title("SEARCH AN ACCOUNT");
            Bordering.Draw(Border.TEXT, "Account Number: ",false,0);
            Bordering.Draw(Border.BOT, "", false, 9);
            showAccountDetails(find_account(readInt(0)));
            Console.WriteLine("\n Check another account (y/n)?");
            if (Console.ReadLine() == "y")
                search_account();
        }
            catch (Exception e)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nError :{0}",e.Message);
                Console.ReadKey(true);
            }
}

        public void updateBalance(bool isDeposit)
        {
            try { 
            if (isDeposit)
                title("DEPOSIT");
            else title("WITHDRAW");
            Bordering.Draw(Border.TEXT, "Account Number: ", false, 0);
            Bordering.Draw(Border.TEXT, "Amount: ", false, 1);
            Bordering.Draw(Border.BOT, "", false, 9);
            int accountNumber = readInt(0);
            
            currentAccount = find_account(accountNumber);
                Bordering.setPos(9);

                if (currentAccount == null)
                {
                    Console.WriteLine("\nRetry (y/n)?");
                    if (Console.ReadLine() == "y")
                    {
                        updateBalance(isDeposit);
                    }
                }
                else {
                    Console.WriteLine("Enter the amount...");Bordering.savePos(9);
                }
            int amount = readInt(1);
                string msg = "";
            if (isDeposit)
            {
                currentAccount.updateBalance(amount);
                    msg = "Deposit";
            }
            else {
                currentAccount.updateBalance(-amount);
                    msg = "Withdraw";
            }              

            SaveFile(currentAccount);
            Bordering.setPos(9);
                Console.WriteLine("{0} Successful!",msg);
                Console.ReadKey(true);

            }
            catch (Exception e)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nError :{0}",e.Message);
                Console.ReadKey(true);
            }
}


        public void statement()
        {
            try {
            title("STATEMENT");
            Bordering.Draw(Border.TEXT, "Account Number: ", false, 0);
            Bordering.Draw(Border.BOT,"",false,9);
            int accountNumber = readInt(0);
            currentAccount = find_account(accountNumber);
            if (currentAccount != null)
            {
                showAccountDetails(currentAccount, true);
                Console.WriteLine("\nEmail Statement (y/n)?");
                mail(currentAccount);
                Console.WriteLine("Email sent successfully!...");
                Console.ReadKey(true);
            }
            }
            catch (Exception e)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nError :{0}", e.Message);
                Console.ReadKey(true);
            }
        }

        public void delete_account()
        {
            try {
            title("DELETE AN ACCOUNT");
            Bordering.Draw(Border.TEXT, "Account Number: ", false, 0);
            Bordering.Draw(Border.BOT);
            int accountNumber = readInt(0);
            showAccountDetails(find_account(accountNumber), true);
            Console.WriteLine("Delete (y/n)?");
            if(Console.ReadLine() == "y")
            File.Delete(currentAccount.AccountNum+".txt");
            Console.WriteLine("Account Deleted!...");
            Console.ReadKey(true);
            }
            catch (Exception e)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nError :{0}", e.Message);
                Console.ReadKey(true);
            }
        }

        private int getAccountNumber()
        {
            try { 
            int existings = Directory.GetFiles("./", "*.txt").Count();
            int accountNumber = 100000 + existings;
            return accountNumber;
            }
            catch (Exception e)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nError :{0}", e.Message);
                Console.ReadKey(true);
                return -1;
            }
        }

        public void SaveFile(Account currentAccount)
        {
            try { 
                FileStream writeFileStream = new FileStream(currentAccount.AccountNum + ".txt", FileMode.Create, FileAccess.Write);
                // write object
                this.formatter.Serialize(writeFileStream, currentAccount);
                // close file
                writeFileStream.Close();
            }
            catch (Exception e)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nError :{0}", e.Message);
                Console.ReadKey(true);
            }
        }

        private Account find_account(int accountNumber)
        {
            String fileName = accountNumber.ToString() + ".txt";

            try
            {
                FileStream readFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                currentAccount = (Account)this.formatter.Deserialize(readFileStream);
                readFileStream.Close();
                return currentAccount;
                
            }
            catch (FileNotFoundException)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nAccount not found!                              ");Bordering.savePos(9);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected Error: {0}", e.Message);
                return null;
            }
        }

        private void showAccountDetails(Account account,bool isStatement = false) {
            if (account != null)
            {
                Bordering.Draw(Border.BOT);
                
                
                if (isStatement)
                {
                    Console.WriteLine("\n\nAccount found! The statement is displayed below...");
                    Bordering.Draw(Border.TOP);
                    Bordering.Draw(Border.TEXT, "SIMPLE BANKING SYSTEM", true);
                    Bordering.Draw(Border.MID);
                    Bordering.Draw(Border.TEXT, "Account Statement");
                }
                else
                {
                    Console.WriteLine("\n\nAccount found!");
                    Bordering.Draw(Border.TOP);
                    Bordering.Draw(Border.TEXT, "ACCOUNT DETAILS", true);
                    Bordering.Draw(Border.MID);
                }
                Bordering.Draw(Border.TEXT, "  ");
                Bordering.Draw(Border.TEXT, "Account No: " + account.AccountNum);
                Bordering.Draw(Border.TEXT, "Account Balance: $" + account.Balance);
                Bordering.Draw(Border.TEXT, "First Name: " + account.FirstName);
                Bordering.Draw(Border.TEXT, "Last Name: " + account.LastName);
                Bordering.Draw(Border.TEXT, "Address: " + account.Address);
                Bordering.Draw(Border.TEXT, "Phone: " + account.Phone);
                Bordering.Draw(Border.TEXT, "Email: " + account.Email);
                Bordering.Draw(Border.BOT,"",false,9);
            }
        }

        private void mail(Account currenAccount)
        {
            string subject = "";
            string text = currentAccount.toString();

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("dotnet1227@gmail.com","anthy@123");
            smtpClient.Timeout = 10000;

            MailMessage message = new MailMessage("dotnet1227@gmail.com", currentAccount.Email, subject, text);

            try {
                smtpClient.Send(message);
                Console.WriteLine("sent");
                Console.ReadKey(true);
            }
            catch (Exception e)
            {
                Bordering.setPos(9);
                Console.WriteLine("Error: Failed to send Mail, {0}", e);
                Console.ReadKey(true);
            }
        }

        private string validEmail(string email)
        {
            string[] set = email.Split('@');
            if (set.Length == 2)
                return email;
            else {
                Bordering.setPos(9);
                Console.WriteLine("\nInvalid Email.. Please type charcters");
                Bordering.setPos(4);
                Console.WriteLine("                                 ");
                return readString(4);
            }
        }

        private string readString(int pos)
        {
            try
            {
                Bordering.setPos(pos);
                return Console.ReadLine();
            }

            catch (FormatException)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nWrong datatype.. Please type charcters");
                Bordering.setPos(pos);
                Console.WriteLine("                                 ");
                return readString(pos);
            }
        }

        private double readDouble(int pos)
        {
            try
            {
                Bordering.setPos(pos);
                return Convert.ToDouble(Console.ReadLine());
            }

            catch (FormatException)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nWrong datatype.. Please type numbers");
                Bordering.setPos(pos);
                Console.WriteLine("                                 ");
                return readDouble(pos);
            }
        }

        private int readInt(int pos)
        {
            try
            {
                Bordering.setPos(pos);
                return Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nWrong datatype.. Please type numbers");
                Bordering.setPos(pos);
                Console.WriteLine("                                 ");
                return readInt(pos);
            }
            catch (Exception e)
            {
                Bordering.setPos(9);
                Console.WriteLine("\nUnexpected Error: {0]",e.Message);
                Bordering.setPos(pos);
                Console.WriteLine("                                 ");
                return readInt(pos);
            }
        }
        private void title(string option)
        {
            Console.Clear();
            Bordering.Draw(Border.TOP);
            Bordering.Draw(Border.TEXT, option, true);
            Bordering.Draw(Border.MID);
            Bordering.Draw(Border.TEXT, "ENTER THE DETAILS", true);
            Bordering.Draw(Border.TEXT, "  ", true);
        }
    }

    [Serializable]
    class Account
    {
        //declare attributes of account (private)
        private string first_name, last_name, address, email;
        private int account_num, phone;
        private double balance;

        //Property for variables
        //only kept get to prevent from being modified by mistake
        public string FirstName
        {
            get
            { return first_name; }
        }
        public string LastName
        {
            get
            { return last_name; }
        }
        public string Address
        {
            get
            { return address; }
        }
        public string Email
        {
            get
            { return email; }
        }
        public int AccountNum
        {
            get
            { return account_num; }
        }

        public int Phone
        {
            get
            { return phone; }
        }

        public double Balance
        {
            get
            { return balance; }

        }
        public Account()
        { }
        //constructor
        public Account(int account_num, string first_name, string last_name, string address, int phone, string email, double balance)
        {
            this.account_num = account_num;
            this.first_name = first_name;
            this.last_name = last_name;
            this.address = address;
            this.phone = phone;
            this.email = email;
            this.balance = balance;
        }

        //being called in deposit and withdraw functions
        public void updateBalance(double update)
        {
            balance += update;
        }

        public string toString()
        {
            string accInf = "Account Number: " + AccountNum + "\n"
            + "First Name: " + FirstName + "\n"
            + "Last Name: " + LastName + "\n"
            + "Address: " + Address + "\n"
            + "Phone Number: " + Phone + "\n"
            + "Email: " + Email + "\n"
            + "Balance: " + Balance + "\n";

            return accInf;
        }
    }
}

