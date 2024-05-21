using DataBase.Entities.Concretes;
using DataBase.Repositories.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Lumia_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MenuController(ITeamRepository teamRepository, IWebHostEnvironment webHostEnvironment)
        {
            _teamRepository = teamRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _teamRepository.GetAllAsync();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Team team)
        {
            if(!ModelState.IsValid)
            {
                return View(team);
            }

            string path = _webHostEnvironment.WebRootPath + @"\Upload\manage\";
            string fileName = Guid.NewGuid() + team.ImgFile.FileName;
            string fullPath=Path.Combine(path, fileName);

            using(FileStream stream=new FileStream(fullPath, FileMode.Create))
            {
                team.ImgFile.CopyTo(stream);
            }
            team.ImgUrl = fileName;

            await _teamRepository.AddAsync(team);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult>Remove(int id)
        {
            await _teamRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]

        public async Task<IActionResult>Update(int id)
        {
            var existingData=await _teamRepository.GetByIdAsync(id);
            return View(existingData);  
        }
        [HttpPost]

        public IActionResult Update(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View(team);
            }
            if (team.ImgFile != null)
            {
                string path = _webHostEnvironment.WebRootPath + @"\Upload\manage\";
                string fileName = Guid.NewGuid() + team.ImgFile.FileName;
                string fullPath = Path.Combine(path, fileName);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    team.ImgFile.CopyTo(stream);
                }
                team.ImgUrl = fileName;
            }
            _teamRepository.UpdateAsync(team); 
            return RedirectToAction("Index");
        }
    }
}
