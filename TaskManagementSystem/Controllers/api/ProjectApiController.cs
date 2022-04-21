using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagementSystem.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectApiController : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IMapper _mapper;
        public ProjectApiController(TaskManagerContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ProjectDto> GetProjectsAsync()
            => _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDto>>(_context.Projects.ToList());


        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(x => x.project_id == id);

            if (project == null)
                return BadRequest();

            return _mapper.Map<Project,ProjectDto>(project);
            
        }
        [HttpGet("project/owners")]
        public async Task<IEnumerable<AccountDto>> GetBoardOwners()
        {
            var boardOwners = await _context.Projects
                .Select(x => x.owner).ToListAsync();

            return _mapper.Map<IEnumerable<Account>,IEnumerable<AccountDto>>(boardOwners);

       }

        [HttpPost]
        public async Task<ActionResult> CreateProjectAsync(ProjectDto projectDto)
        {
            var project = _mapper.Map<ProjectDto, Project>(projectDto);

            var projectOwner = await _context.Accounts
                                        .FirstOrDefaultAsync(x => x.account_id ==project.owner_id);

            if (projectOwner == null)
                return BadRequest();

            ModelState.Remove("owner");
            if (!ModelState.IsValid)
                return BadRequest();

            var newProject = new Project()
            {
                key = project.key,
                title=project.title,
                owner_id = project.owner_id,
                owner = projectOwner,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();

            projectDto.project_id = newProject.project_id;

            return CreatedAtAction(
                 nameof(GetProjectsAsync),
                 new { id = project.project_id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditProjectAsync(ProjectDto projectDto, int id)
        {
            var project = _mapper.Map<ProjectDto, Project>(projectDto);

            var existingProject = _context.Projects
                .FirstOrDefault(x => x.project_id == id);

            var account = _context.Accounts.
                FirstOrDefault(x => x.account_id == project.owner_id);

            if (account == null)
                return BadRequest();

            if (existingProject == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return RedirectToAction();

            existingProject.key = project.key;
            existingProject.title = project.title;
            existingProject.owner_id = project.owner_id;
            existingProject.owner = account;
            existingProject.updated_at = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProjectAsync(int id)
        {
            var project = _context.Projects
                .FirstOrDefault(x => x.project_id == id);

            if (project == null)
                return BadRequest();

            _context.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}