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

            var result = _mapper.Map<TaskManagementSystem.Models.Task,TaskDto>(task);

            if (task.story_points != null)
                result.story_points = (int)task.story_points;

            if (task.assigned_to != null)
                result.assigned_to = (int)task.assigned_to;
            return result;

        }

        [HttpGet("project/tasks/{id}")]
        public async Task<IEnumerable<TaskDto>> GetProjectsFromTasksAsync(int id)
        {
            var tasks = await _context.Tasks.Select(x => x)
                                              .Where(x => x.project_id == id)
                                              .ToListAsync();
            return _mapper.Map<IEnumerable<TaskManagementSystem.Models.Task>,IEnumerable<TaskDto>>(tasks);
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
                result = result.Where(x => x.title.ToLower().Contains(title.ToLower())).ToList();

            if (searchModel.type != null && !String.IsNullOrEmpty(searchModel.type))
            {
                type = (Types)Int32.Parse(searchModel.type);
                result = result.Where(x => x.type == type).ToList();
            }

            if(searchModel.assigned_to != null && !String.IsNullOrEmpty(searchModel.assigned_to))
            {
                var accountId = Int32.Parse(searchModel.assigned_to);
                result = result.Where(x => x.assigned_to == accountId).ToList();
            }
                   
            return _mapper.Map<IEnumerable<TaskManagementSystem.Models.Task>, IEnumerable<TaskDto>>(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTaskAsync(TaskDto modelDto)
        {
            ModelState.Remove("board");
            ModelState.Remove("project");
            ModelState.Remove("reportedBy");
            if (!ModelState.IsValid)
                return BadRequest();

            var assigned_to = _context.Accounts
                                      .FirstOrDefault(x => x.account_id == modelDto.assigned_to);

            var model = _mapper.Map<TaskDto, TaskManagementSystem.Models.Task>(modelDto);

            var reportedByModel = _context.Accounts
                                           .FirstOrDefault(x => x.account_id == modelDto.reported_by);
            if (reportedByModel == null)
                return BadRequest();

            var projectModel = _context.Projects
                                        .FirstOrDefault(x => x.project_id == modelDto.project_id);
            if (projectModel == null)
                return BadRequest();

            var boardModel = _context.Boards
                                     .FirstOrDefault(x => x.board_id == modelDto.board_id);
            if (boardModel == null)
                return BadRequest();

            model.reportedBy = reportedByModel;
            model.project = projectModel;
            model.board = boardModel;
            model.created_at = DateTime.Now;
            model.updated_at = DateTime.Now;

            if (assigned_to != null)
            {
                model.assigned_to = assigned_to.account_id;
                model.assigedTo = assigned_to;
            }
            if (modelDto.story_points > 0)
                model.story_points = modelDto.story_points;

             _context.Tasks.Add(model);
            await _context.SaveChangesAsync();

            modelDto.task_id = model.task_id;
            

            return CreatedAtAction(
                 nameof(GetTasks),
                 new { id = modelDto.task_id });
        }
      
        [HttpPut("{id}")]
        public async Task<ActionResult> EditAsync(int id, TaskDto modelDto)
        {
            ModelState.Remove("board");
            ModelState.Remove("project");
            ModelState.Remove("reportedBy");
            if (!ModelState.IsValid)
                return BadRequest();

            var model = _context.Tasks.FirstOrDefault(x => x.task_id == modelDto.task_id);

            if (model == null)
                return BadRequest();

;            _mapper.Map<TaskDto, TaskManagementSystem.Models.Task>(modelDto,model);

            var assigned_to = _context.Accounts
                          .FirstOrDefault(x => x.account_id == modelDto.assigned_to);

            if (assigned_to != null)
            {
                model.assigned_to = assigned_to.account_id;
                model.assigedTo = assigned_to;
            }


            if (modelDto.story_points >= 0)
                model.story_points = modelDto.story_points;

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

            model.title = modelDto.title;
            model.project_id = modelDto.project_id;
            model.board_id = modelDto.board_id;
            model.type = modelDto.type;
            model.status = modelDto.status;
            model.description = modelDto.description;
            model.reportedBy = reportedByModel;
            model.reported_by = modelDto.reported_by;
            model.board = boardModel;
            model.project = projectModel;
            model.priority = modelDto.priority;
            model.updated_at = DateTime.Now;

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
