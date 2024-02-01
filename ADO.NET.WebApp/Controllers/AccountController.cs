using ADO.NET.Application.Common.Interfaces;
using ADO.NET.Domain.Entities;
using ADO.NET.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;

namespace ADO.NET.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public ActionResult Index()
        {
            //Creating By Default admin
            bool adminCreated = _unitOfWork.User.RegisterAdmin();


            // Check if the session already exists
            if (Session["UserId"] != null && Session["RoleName"] != null)
            {
                // Session already exists, redirect to home page
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public ActionResult Index(User usr)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    bool check = _unitOfWork.User.RegisterUser(usr);
                    if (check)
                    {
                        TempData["ToastrMessage"] = "User has been created successfully!";
                        TempData["ToastrType"] = "success";
                        ModelState.Clear();
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        TempData["ToastrMessage"] = "Failed to create user!";
                        TempData["ToastrType"] = "error";
                        // Additional logic or redirect if needed
                        return RedirectToAction("SomeOtherAction");
                    }

                }
                return View();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627) // Unique constraint violation error numbers
                {
                    // Handle the unique constraint violation
                    ModelState.AddModelError("Email", "Email already exists. Please use a different email.");

                    TempData["ToastrMessage"] = "Email Already Exists!";
                    TempData["ToastrType"] = "error";

                    return View();
                }
                else
                {
                    // Handle other SQL Server exceptions
                    return View("Error"); // You might want to redirect to an error page or log the exception
                }
            }
            catch (Exception)
            {
                return View("Error"); // Handle other exceptions
            }

        }

        public ActionResult Login()
        {

            // Check if the session already exists
            if (Session["UserId"] != null && Session["RoleName"] != null)
            {
                // Session already exists, redirect to home page
                return RedirectToAction("Index", "Home");
            }
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM usr)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User authenticatedUser = _unitOfWork.User.AuthenticateUser(usr.Email, usr.Password);

                    if (authenticatedUser != null)
                    {
                        // Create a session for the logged-in user
                        Session["UserId"] = authenticatedUser.UserId;
                        Session["UserName"] = authenticatedUser.FirstName;

                        string roleName = _unitOfWork.User.GetRoleName(authenticatedUser.RoleId);
                        Session["RoleName"] = roleName;

                        // Add success Toastr notification
                        TempData["ToastrMessage"] = "Login successful!";
                        TempData["ToastrType"] = "success";

                        // Redirect to home page or any other desired page on successful login
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email or password");

                        // Add error Toastr notification
                        TempData["ToastrMessage"] = "Invalid email or password";
                        TempData["ToastrType"] = "error";

                        return View(usr);
                    }
                }

                return View(usr);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                ModelState.AddModelError("", "An error occurred during login. Please try again.");

                // Add error Toastr notification
                TempData["ToastrMessage"] = "An error occurred during login. Please try again.";
                TempData["ToastrType"] = "error";

                return View(usr);
            }
        }



        public ActionResult Logout()
        {
            try
            {

                // Add success Toastr notification
                TempData["ToastrMessage"] = "Logout successful!";
                TempData["ToastrType"] = "success";

                // Clear session variables
                Session.Clear();

                

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed

                // Add error Toastr notification
                TempData["ToastrMessage"] = "An error occurred during logout. Please try again.";
                TempData["ToastrType"] = "error";

                return RedirectToAction("Login", "Account");
            }
        }


    }
}