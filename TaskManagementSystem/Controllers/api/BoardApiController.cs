using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using System.Text.Json;
using AutoMapper;

namespace TaskManagementSystem.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardApiController : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IMapper _mapper;
        public BoardApiController(TaskManagerContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IEnumerable<BoardDto> OnGet()
            => _mapper.Map<IEnumerable<Board>, IEnumerable<BoardDto>>(_context.Boards.Include(x=>x.owner).ToList());

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDto>> GetBoardByIdAsync(int id)
        {
            var board =  await _context.Boards
                .FirstOrDefaultAsync(x => x.board_id == id);

            if (board == null)
                return BadRequest();

            return _mapper.Map<Board,BoardDto>(board);
            
        }
        [HttpGet("board/owners")]
        public async Task<IEnumerable<AccountDto>> GetBoardOwners()
        {
            var accounts= await _context.Boards
                .Select(x => x.owner).ToListAsync();

            return _mapper.Map<IEnumerable<Account>, IEnumerable<AccountDto>>(accounts);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBoardAsync(BoardDto boardDto)
        {
            ModelState.Remove("owner");
            if (!ModelState.IsValid)
                return BadRequest();

            var board = _mapper.Map<BoardDto, Board>(boardDto);

            var boardOwner = await _context
                 .Accounts.FirstOrDefaultAsync(x => x.account_id == board.owner_id);

            if (boardOwner == null)
                return BadRequest();

            var newBoard = new Board()
            {
                name = board.name,
                owner_id = board.owner_id,
                owner = boardOwner,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };
            _context.Boards.Add(newBoard);
            _context.SaveChanges();

            var newBoardId = _context.Boards
                .First(x => x.name == newBoard.name);

            var boardMember = new BoardMember()
            {
                account_id = boardOwner.account_id,
                accountId= boardOwner,
                board_id = newBoardId.board_id,
                boardId=newBoard
            };
            _context.BoardMembers.Add(boardMember);
            await _context.SaveChangesAsync();

            boardDto.board_id = newBoard.board_id;

            return CreatedAtAction(
                 nameof(GetBoardByIdAsync),
                 new { id = board.board_id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditBoardAsync(BoardDto boardDto, int id)
        {
            var board = _mapper.Map<BoardDto,Board> (boardDto);

            var existingBoard = _context.Boards.
                FirstOrDefault(x => x.board_id == id);
            
            var account = _context.Accounts
                .FirstOrDefault(x => x.account_id == board.owner_id);

            if (account == null)
                return BadRequest();

            if (existingBoard == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return RedirectToAction();

            existingBoard.name = board.name;
            existingBoard.owner_id = board.owner_id;
            existingBoard.owner = account;
            existingBoard.updated_at = DateTime.Now;

            var boardMember = _context.BoardMembers
                .First(x => x.board_id == existingBoard.board_id);

            boardMember.account_id = board.owner_id;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBoardAsync(int id)
        {
            var board = _context.Boards
                .FirstOrDefault(x => x.board_id == id);

            if (board == null)
                return BadRequest();

            _context.Remove(board);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
