using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskApiController : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IMapper _mapper;
        public TaskApiController(TaskManagerContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<TaskDto> GetTasks()
          => _mapper.Map<IEnumerable<TaskManagementSystem.Models.Task>, IEnumerable<TaskDto>>(_context.Tasks.ToList());

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetByIdAsync(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.task_id == id);

            if(task==null)
                return BadRequest();

            return _mapper.Map<TaskManagementSystem.Models.Task,TaskDto>(task);
        }
        [HttpGet("project/tasks/{id}")]
        public async Task<IEnumerable<ProjectDto>> GetProjectsFromTasksAsync(int id)
        {
            var project = await _context.Tasks.Select(x => x.project)
                                              .Where(x => x.project_id == id)
                                              .ToListAsync();
            return _mapper.Map<IEnumerable<Project>,IEnumerable<ProjectDto>>(project);
        }

        [HttpGet("boards/tasks/{id}")]
        public async Task<IEnumerable<BoardDto>> GetBoardFromTasksAsync(int id)
        {
            var board = await _context.Tasks.Select(x => x.board )
                                              .Where(x => x.board_id == id)
                                              .ToListAsync();

            return _mapper.Map<IEnumerable<Board>, IEnumerable<BoardDto>>(board);
        }
        [HttpPost("search")]
        public IEnumerable<TaskDto> GetProjectsFromTasks(SearchModel searchModel)
        {
            var title = searchModel.title;
            Types type = Types.Story;
            var result = _context.Tasks.ToList();

            if (!String.IsNullOrEmpty(title))
                result = result.Where(x => x.title.Contains(title)).ToList();

            if (searchModel.type != "")
            {
                type = (Types)Int32.Parse(searchModel.type);
                result = result.Where(x => x.type == type).ToList();
            }

            if(searchModel.assigned_to != "")
            {
                var accountId = Int32.Parse(searchModel.assigned_to);
                result = result.Where(x => x.assigned_to == accountId).ToList();
            }
                   
            return _mapper.Map<IEnumerable<TaskManagementSystem.Models.Task>, IEnumerable<TaskDto>>(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTaskAsync(TaskDto modelDto)
        {
            var model = _mapper.Map<TaskDto, TaskManagementSystem.Models.Task>(modelDto);

             Types type = Types.Story;
            if (Enum.IsDefined(typeof(Types), model.type))
            {
                type = (Types)model.type;
            }
            else return BadRequest();

            Status status = Status.New;
            Priorities priority = Priorities.Low;
            if (Enum.IsDefined(typeof(Status), model.status))
                status = (Status)model.status;

            if (Enum.IsDefined(typeof(Priorities), model.priority))
                priority = (Priorities)model.priority;

            ModelState.Remove("board");
            ModelState.Remove("project");
            ModelState.Remove("reportedBy");
            if (!ModelState.IsValid)
                return BadRequest();

            var reportedByModel = _context.Accounts.FirstOrDefault(x => x.account_id == model.reported_by);
            if (reportedByModel == null)
                return BadRequest();

            var projectModel = _context.Projects.FirstOrDefault(x => x.project_id == model.project_id);
            if (projectModel == null)
                return BadRequest();

            var boardModel = _context.Boards.FirstOrDefault(x => x.board_id == model.board_id);
            if (boardModel == null)
                return BadRequest();


            var newTask = new TaskManagementSystem.Models.Task()
            {
                title = model.title,
                project_id = model.project_id,
                board_id = model.board_id,
                type = type,
                status = status,
                description = model.description,
                reportedBy = reportedByModel,
                reported_by = model.reported_by,
                assigned_to = null,
                board = boardModel,
                project = projectModel,
                priority = priority,
                story_points = null,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();

            modelDto.task_id = newTask.task_id;

            return CreatedAtAction(
                 nameof(GetTasks),
                 new { id = model.task_id });
        }
      
        [HttpPut("{id}")]
        public async Task<ActionResult> EditAsync(int id, TaskDto modelDto)
        {
            var model = _mapper.Map<TaskDto, TaskManagementSystem.Models.Task>(modelDto);

            var updatedTask = await _context.Tasks
                .FirstOrDefaultAsync(x => x.task_id == id);

            if (updatedTask == null)
                return BadRequest();

            Types type = Types.Story;

            if (Enum.IsDefined(typeof(Types), model.type))
            {
                type = (Types)model.type;
            }
            else 
                return BadRequest();

            Status status = Status.New;
            Priorities priority = Priorities.Low;

            if (Enum.IsDefined(typeof(Status), model.status))
                status = (Status)model.status;

            if (Enum.IsDefined(typeof(Priorities), model.priority))
                priority = (Priorities)model.priority;

            ModelState.Remove("board");
            ModelState.Remove("project");
            ModelState.Remove("reportedBy");
            if (!ModelState.IsValid)
                return BadRequest();

            var reportedByModel = _context.Accounts
                .FirstOrDefault(x => x.account_id == model.reported_by);

            if (reportedByModel == null)
                return BadRequest();

            var projectModel = _context.Projects
                .FirstOrDefault(x => x.project_id == model.project_id);

            if (reportedByModel == null)
                return BadRequest();

            var boardModel = _context.Boards
                .FirstOrDefault(x => x.board_id == model.board_id);

            if (reportedByModel == null)
                return BadRequest();

            updatedTask.title = model.title;
            updatedTask.project_id = model.project_id;
            updatedTask.board_id = model.board_id;
            updatedTask.type = type;
            updatedTask.status = status;
            updatedTask.description = model.description;
            updatedTask.reportedBy = reportedByModel;
            updatedTask.reported_by = model.reported_by;
            updatedTask.assigned_to = null;
            updatedTask.board = boardModel;
            updatedTask.project = projectModel;
            updatedTask.priority = priority;
            updatedTask.story_points = null;
            updatedTask.updated_at = DateTime.Now;

            _context.Tasks.Add(updatedTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var task = _context.Tasks.FirstOrDefault(x => x.task_id == id);

            if (task == null)
                return BadRequest();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
