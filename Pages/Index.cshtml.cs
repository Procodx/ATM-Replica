using ATM_Replica.Data_folder;
using ATM_Replica.Data_folder.model_folder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ATM_Replica.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public User userInfo { get; set; }
         

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
       
        public void OnGet()
        {
            
        }
        public ActionResult OnPost()
        {

            var db = new Db();
           var userLogin =  db.GetUser(Email);
           if(userLogin != null)
            {
                if (userLogin.Email == Email && userLogin.Password == Password)
                {
                    userInfo = userLogin;
                    return Redirect($"UserTransactions/UserDashboard/{Email}");

                }
            }
            
            ModelState.AddModelError("", "Invalid password or email.");
            return Page();

        }


    }
}
    
