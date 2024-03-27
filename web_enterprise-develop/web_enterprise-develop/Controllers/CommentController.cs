using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.Repositories.Implement;
using WebEnterprise.ViewModels.Comment;

namespace WebEnterprise.Controllers
{
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public INotyfService _notyfService { get; }
        private readonly IEmailSender _emailSender;

        public CommentController(IUnitOfWork unitOfWork, IMapper mapper, INotyfService notyfService,
            IHttpContextAccessor httpContextAccessor, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
        }

        public async Task<ActionResult> Create(CreateComment createComment)
        {
            if (ModelState.IsValid)
            {
                string UserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                if (await _unitOfWork.UserRepository.IsAllowed(UserId, createComment.FacultyId))
                {
                    if(await _unitOfWork.CommentRepository.IsCommented(createComment.ContributionId))
                    {
                        _notyfService.Error("Each contribution only has a comment");
                    }
                else
                    {
                        Comment comment = _mapper.Map<Comment>(createComment);
                        comment.UserId = UserId;
                        await _unitOfWork.CommentRepository.Add(comment);
                        _notyfService.Success("Creating comment successfully");

                        var receiver = await _unitOfWork.UserRepository.findStudentEmail(createComment.StudentId);
                        var subject = "Send a notification";
                        var message = "Your contribution is vertified and commented by the faculty coordinator, please check it!!!";
                        await _emailSender.SendEmailAsync(receiver, subject, message);
                    }
                }
                else
                {
                    _notyfService.Error("Only coordination of this faculty is allowed to comment");
                }

                return RedirectToAction("Detail","Contribution", new { id = createComment.ContributionId });
            }
            else
            {
                _notyfService.Error("Get error in the system");

                return RedirectToAction("Detail", "Contribution", new { id = createComment.ContributionId });
            }
        }
    }
}
