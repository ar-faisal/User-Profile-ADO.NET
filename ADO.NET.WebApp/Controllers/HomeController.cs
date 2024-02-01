using ADO.NET.Application.Common.Interfaces;
using ADO.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADO.NET.WebApp.Controllers
{


    
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }


        [AuthorizeUser("admin", "user")]
        //here both Roles, admin and user can access this
        public ActionResult Index()
        {

            
            var userId = (int)Session["UserId"];

            User usr = _unitOfWork.User.GetUser(userId);
            
            

            return View(usr);
        }



        public ActionResult Edit(int id)
        {
            // Retrieve the user with the given ID from the database
            User usr = _unitOfWork.User.GetUser(id);

            if (usr == null)
            {
                // Handle the case where the user is not found, for example, return a 404 Not Found view.
                return HttpNotFound();
            }

            // Pass the user to the Edit view
            return View(usr);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User obj)
        {
            try
            {
                ModelState.Remove("Email");
                ModelState.Remove("Confirmpwd");
                ModelState.Remove("ImageFile");
                ModelState.Remove("ImageUrl");
                ModelState.Remove("Password");
                ModelState.Remove("ResumeFile");
                ModelState.Remove("ResumeUrl");
                ModelState.Remove("RoleId");
                

                if (ModelState.IsValid)
                {

                    if (obj.ImageFile != null && obj.ImageFile.ContentLength > 0) // Check if a new image is uploaded
                    {
                        // Get the file extension
                        string imageFileExtension = Path.GetExtension(obj.ImageFile.FileName).ToLower();

                        // Check if the image file has a valid extension
                        if (imageFileExtension != ".jpg" && imageFileExtension != ".jpeg" && imageFileExtension != ".png")
                        {
                            ModelState.AddModelError("ImageFile", "Only JPG, JPEG, and PNG files are allowed.");
                            return View(obj);
                        }

                        // Save the new image file
                        string imageFileName = Guid.NewGuid().ToString() + imageFileExtension;
                        string imagePath = Path.Combine(Server.MapPath("/wwwroot/Images/ProfilePic"), imageFileName);  //Here for ASP.NET core, we start location with " ~/wwwroot/.... "

                        if (!string.IsNullOrEmpty(obj.ImageUrl))
                        {
                            var oldImagePath = Server.MapPath(obj.ImageUrl);

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        obj.ImageFile.SaveAs(imagePath);
                        obj.ImageUrl = "/wwwroot/Images/ProfilePic/" + imageFileName;
                    }

                    if (obj.ResumeFile != null && obj.ResumeFile.ContentLength > 0) // Check if a new resume file is uploaded
                    {
                        // Get the file extension
                        string resumeFileExtension = Path.GetExtension(obj.ResumeFile.FileName).ToLower();

                        // Check if the resume file has a valid extension (adjust the allowed extensions as needed)
                        if (resumeFileExtension != ".pdf" && resumeFileExtension != ".doc" && resumeFileExtension != ".docx")
                        {
                            ModelState.AddModelError("ResumeFile", "Only PDF, DOC, and DOCX files are allowed for resumes.");
                            return View(obj);
                        }

                        // Save the new resume file
                        string resumeFileName = Guid.NewGuid().ToString() + resumeFileExtension;
                        string resumePath = Path.Combine(Server.MapPath("/wwwroot/Resumes"), resumeFileName);

                        if (!string.IsNullOrEmpty(obj.ResumeUrl))
                        {
                            var oldResumePath = Server.MapPath(obj.ResumeUrl);

                            if (System.IO.File.Exists(oldResumePath))
                            {
                                System.IO.File.Delete(oldResumePath);
                            }
                        }

                        obj.ResumeFile.SaveAs(resumePath);
                        obj.ResumeUrl = "/wwwroot/Resumes/" + resumeFileName;
                    }

                    bool updated = _unitOfWork.User.UpdateUser(obj);

                    if (updated)
                    {
                        TempData["ToastrMessage"] = "The user has been updated successfully!";
                        TempData["ToastrType"] = "success";
                        return RedirectToAction("Index", "Home");
                    }
                }

                TempData["ToastrMessage"] = "The User Could Not Be Updated!";
                TempData["ToastrType"] = "error";
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["ToastrMessage"] = "An Error Occured While Updating The User!";
                TempData["ToastrType"] = "error";
                // Optionally redirect to an error page or display a user-friendly message
                return RedirectToAction("Error", "Home");
            }
        }


        public ActionResult Delete(int id)
        {
            
            User usr = _unitOfWork.User.GetUser(id);

            if (usr == null)
            {
                // Handle the case where the user is not found, for example, return a 404 Not Found view.
                return HttpNotFound();
            }

            // Pass the user to the Edit view
            return View(usr);

        }



        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // Retrieve the user by ID
                User userToDelete = _unitOfWork.User.GetUser(id);

                if (userToDelete == null)
                {
                    TempData["ToastrMessage"] = "User Not Found!";
                    TempData["ToastrType"] = "error";
                    return RedirectToAction("Index", "Home");
                }

                // Check and delete the image file
                if (!string.IsNullOrEmpty(userToDelete.ImageUrl))
                {
                    var imagePath = Server.MapPath(userToDelete.ImageUrl);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Check and delete the resume file
                if (!string.IsNullOrEmpty(userToDelete.ResumeUrl))
                {
                    var resumePath = Server.MapPath(userToDelete.ResumeUrl);

                    if (System.IO.File.Exists(resumePath))
                    {
                        System.IO.File.Delete(resumePath);
                    }
                }

                // Delete the user from the database
                bool deleted = _unitOfWork.User.DeleteUser(id);

                if (deleted)
                {
                    TempData["ToastrMessage"] = "The User has been deleted successfully!";
                    TempData["ToastrType"] = "success";
                }
                else
                {
                    TempData["ToastrMessage"] = "The User Could Not Be Deleted!";
                    TempData["ToastrType"] = "error"; ;
                }

                Session.Clear();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as appropriate for your application
                TempData["error"] = "An error occurred while deleting the user.";
                // Optionally redirect to an error page or display a user-friendly message
                return RedirectToAction("Error", "Home");
            }
        }

        //Here only admin can access it
        //[AuthorizeUser("admin")]
        //public ActionResult DeleteUsers()
        //{
        //    return();


        //}

    }
}