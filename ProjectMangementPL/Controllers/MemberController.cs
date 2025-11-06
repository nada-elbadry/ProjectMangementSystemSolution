using GymMangementBLL.Services.Classes;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ProjectMangementPL.Controllers
{
    public class MemberController : Controller

    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        #region Get All Member

        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
        #endregion
        //public IActionResult Index(int id)
        //{
        //    return RedirectToRoute("Trainers", new {action ="GetTrainers"});
        //}

        //public ActionResult GetMembers()
        //{ 
        //return View();
        //}
        //public ActionResult CreateMember()
        //{
        //    return View();
        //}

        #region Get Member Data
        //BaseURL: /Member/MemberDetails-> id =0
        //BaseURL: /Member/MemberDetails/5 -> id =1
        public ActionResult MemberDetails(int id)
        { 
            if(id<=0)
                return RedirectToAction(nameof(Index));
       var member = _memberService.GetMemberDeails(id);
            if (member is null)
                return RedirectToAction(nameof(Index));
            return View(member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
                return RedirectToAction(nameof(Index));
            var healthRecord = _memberService.GetMemberHealthRecordDetails(id);
            if (healthRecord is null)
                return RedirectToAction(nameof(Index));
            return View(healthRecord);
        }
        #endregion
        #region Add Member
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreatMemberViewModel createMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Invalid data. Please correct the errors and try again.");
                return View(nameof(Create), createMember);
            }
            bool result = _memberService.CreateMember(createMember);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (_memberService.IsEmailExists(createMember.Email))
                    ModelState.AddModelError("DuplicateEmail", "This email is already registered.");
                if (_memberService.IsPhoneExists(createMember.Phone))
                    ModelState.AddModelError("DuplicatePhone", "This phone number is already registered.");
                return View(nameof(Create), createMember);
            }
        }
        #endregion
        #region Update Member
        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Numbers";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            bool emailTaken = _memberService.IsEmailExists(viewModel.Email, id);
            bool phoneTaken = _memberService.IsPhoneExists(viewModel.Phone, id);
            if (emailTaken)
                ModelState.AddModelError("Email", "This email is already registered by another member.");
            if (phoneTaken)
                ModelState.AddModelError("Phone", "This phone number is already registered by another member.");
            if (emailTaken || phoneTaken)
                return View(viewModel);
            var result = _memberService.UpdateMemberDetails(id, viewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "Member updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Member failed to update. An unexpected error occurred.";
                return View(viewModel);
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete Member


        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var Member = _memberService.GetMemberDeails(id);

            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            return View();
        }


        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            // --- FIX #2: Added pre-check for better error message ---
            if (id == 0)
            {
                // This catches if the fix wasn't applied correctly
                TempData["ErrorMessage"] = "Delete Failed: ID was 0. Binding error.";
                return RedirectToAction(nameof(Index));
            }

            if (_memberService.HasActiveSessions(id))
            {
                TempData["ErrorMessage"] = "Cannot delete member. They have active or future sessions.";
                return RedirectToAction(nameof(Index));
            }

            var Result = _memberService.RemoveMember(id);

            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                // This message will now appear if the database constraint fails
                TempData["ErrorMessage"] = "Member Failed To Delete. Check console log for database errors.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
