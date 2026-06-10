using ATM_Replica.Data_folder.model_folder;
using ATM_Replica.Data_folder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;

namespace ATM_Replica.Pages.UserTransactions
{
    public class DepositModel : PageModel
    {
        [BindProperty]
        [Range(1, 100000000)]
        public int Amount { get; set; }

        [BindProperty]
        public Transaction? DepositAmount { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Email { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal Balance { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? LastName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? FirstName { get; set; }
        public int MyProperty { get; set; }

        public void OnGet()
        {
            try
            {
                //var userAccount = new Db();
                var db = new Db();
                var userDetails = db.GetUser(Email);

                if (userDetails.Email == Email)
                {
                    Balance = userDetails.Balance;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message", ex);
            }

        }
        public void OnPost()
        {
            try
            {
                decimal howMuch = Convert.ToDecimal(Amount);

                var db = new Db();
                var user = db.GetUser(Email);
                if (user != null)
                {
                    if (howMuch <= 0)
                    {
                        Balance = user.Balance;
                        ModelState.AddModelError("", "Your withdrawal amount must be positive");
                        throw new InvalidOperationException();
                    }
                    var updateBalance = user.Balance + howMuch;
                    user.Balance = updateBalance;
                    if (updateBalance > 0)
                    {
                        Balance = updateBalance;
                    }
                    FirstName = user.FirstName;
                    LastName = user.LastName;
                }
               

                var DepositAmount = new Transaction
                {
                    userId = user.Id,
                    amount = howMuch,
                    TransactionType = "Deposit",
                    TransactionDateTime = DateTime.Now,
                };
               
                db.CreateTransaction(DepositAmount);
                user.Transactions.Add(DepositAmount);
                db.UpdateUserInfo(user);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}
