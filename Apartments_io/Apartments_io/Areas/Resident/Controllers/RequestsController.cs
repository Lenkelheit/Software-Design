using Microsoft.AspNetCore.Mvc;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using System.Threading.Tasks;

namespace Apartments_io.Areas.Resident.Controllers
{
    [Area("Resident")]
    public class RequestsController : Controller
    {
        // FIELDS
        readonly IUnitOfWork unitOfWork;

        readonly IRequestRepository requestRepository;
        IUserRepository userRepository;
        IApartmentRepository apartmentRepository;

        // CONSTRUCTORS
        public RequestsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            this.requestRepository = unitOfWork.GetRepository<Request, RequestRepository>();
            this.userRepository = null;
            this.apartmentRepository = null;
        }

        // PROPERTIES

        // lazy loaded repositories
        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null) userRepository = unitOfWork.GetRepository<User, UserRepository>();
                return userRepository;
            }
        }
        public IApartmentRepository ApartmentRepository
        {
            get
            {
                if (apartmentRepository == null) apartmentRepository = unitOfWork.GetRepository<Apartment, ApartmentRepository>();
                return apartmentRepository;
            }
        }

        // ACTIONS
        public IActionResult Index()
        {
            return View();
        }

        // ajax
        [HttpPost]
        public async Task<IActionResult> CreateRequest(int userId, int apartmentId)
        {
            // get user
            User user = await UserRepository.GetAsync(userId);
            if (user == null) return BadRequest("User does not exist");

            // get apartment
            Apartment apartment = await ApartmentRepository.GetAsync(apartmentId);
            if (apartment == null) return BadRequest("Apartment does not exist");

            // create request
            await requestRepository.InsertAsync(new Request()
            {
                Resident = user,
                Apartment = apartment,
            });

            await unitOfWork.SaveAsync();

            return Ok();
        }

        // ajax
        [HttpPost]
        public async Task<IActionResult> DeleteRequest(int userId, int apartmentId)
        {
            // get user
            User user = await UserRepository.GetAsync(userId);
            if (user == null) return BadRequest("User does not exist");

            // get apartment
            Apartment apartment = await ApartmentRepository.GetAsync(apartmentId);
            if (apartment == null) return BadRequest("Apartment does not exist");

            // get request
            Request request = requestRepository.GetByValues(user.Id, apartment.Id);
            if (request == null) return BadRequest("Request does not exist");

            // delete request
            requestRepository.Delete(request);
            await unitOfWork.SaveAsync();

            return Ok();
        }
    }
}