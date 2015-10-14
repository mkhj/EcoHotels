using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models.Security;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models;
using EcoHotels.Core.Domain.Models.Security;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class UserController : Controller
    {
        #region - Services -

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public IUserService UserService { get; set; }

        [Dependency]
        public IOrganizationService OrganizationService { get; set; }

        #endregion

        [HttpGet]
        public ActionResult Create()
        {
            return View(new UserModel(null));
        }

        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult Create(UserModel model)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();

            #region - Defensive programming -

            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("User is not valid."));
            }

            if (!UserService.IsEmailUnique(model.Email))
            {
                return Json(new JsonResultError("E-mail needs to be unique."));
            }

            #endregion

            var organization = OrganizationService.FindById(currentOrganizationId);

            var user = EcoHotels.Core.Domain.Models.Security.User.Create(organization, model.Email, model.Password, RolesEnum.Organization);
            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.IsActive = model.IsActive;
            //UserService.Save(user);

            organization.Users.Add(user);

            //new EmailService().SendAccountCreatedEmailToUser(user, model.Password, hotel);

            OrganizationService.Save(organization);

            return Json(new JsonResultSuccess("Created succesfully.", new { id = user.Id, name = user.GetFullname(), created = true }));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var organization = OrganizationService.FindById(currentOrganizationId);

            if (!organization.Users.Any(x => x.Id == id))
            {
                throw new ApplicationException("User does not exist.");
            }

            return View(new UserModel(id, organization.Users));
        }

        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult Edit(UserModel model)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();

            #region - Defensive programming -

            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("User is not valid."));
            }

            var user = UserService.FindById(model.Id);
            if (user.IsNull() || user.Organization.Id != currentOrganizationId)
            {
                return Json(new JsonResultError("User is not valid."));
            }

            var userChangedEmail = string.Compare(user.Email, model.Email, true) != 0;
            if (userChangedEmail && !UserService.IsEmailUnique(model.Email))
            {
                return Json(new JsonResultWarning("E-mail needs to be unique."));             
            }

            if (model.Password.IsNotNullOrEmpty() && model.Password.Length <= 5)
            {
                return Json(new JsonResultWarning("Password should be longer then 5 characters."));
            }

            #endregion


            user.Email = model.Email;
            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.IsActive = model.IsActive;

            var passwordHasChanged = false;
            if(model.Password.IsNotNullOrEmpty() && model.Password.Length > 5)
            {
                passwordHasChanged = user.SetNewPassword(model.Password);    
            }

            UserService.Save(user);

            if (passwordHasChanged)
            {
                //var hotel = HotelService.FindById(currentTenant.HotelId);
                //new EmailService().SendPasswordChangedEmailToUser(user, model.Password, hotel);
            }

            return Json(new JsonResultSuccess("Updated succesfully."));
        }

        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult Delete(int id)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var user = UserService.FindById(currentOrganizationId, id);

            if (user.IsNull())
            {
                throw new ApplicationException("User not found.");
            }

            UserService.Delete(user);

            return Json(new JsonResultSuccess("Deleted succesfully."));
        }

    }
}
