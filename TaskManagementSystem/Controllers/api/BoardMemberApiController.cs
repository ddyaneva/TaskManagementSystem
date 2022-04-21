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
    public class BoardMemberApiController : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IMapper _mapper;
        public BoardMemberApiController(TaskManagerContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public  IEnumerable<BoardMemberDto> GetBoards()
            => _mapper.Map<IEnumerable<BoardMember>, IEnumerable<BoardMemberDto>>(_context.BoardMembers.ToList());

        [HttpGet("{id}")]
        public async Task<IEnumerable<AccountDto>> GetAccountByBoardIdAsync(int id)
        {
            var accounts = await _context.BoardMembers.Where(x => x.board_id == id)
                                           .Select(x => x.accountId)
                                           .ToListAsync();
            return _mapper.Map<IEnumerable<Account>, IEnumerable<AccountDto>>(accounts);

        }
        [HttpPost]
        public async Task<ActionResult> CreateBoardMembersAsync(BoardMemberDto boardMemberDto)
        {
            var boardMember = _mapper.Map<BoardMemberDto, BoardMember>(boardMemberDto);

            var account = await _context.Accounts
                .FirstOrDefaultAsync(x => x.account_id == boardMember.account_id);

            if (account == null)
                return BadRequest();

            var board = await _context.Boards
                .FirstOrDefaultAsync(x => x.board_id == boardMember.board_id);

            if (board == null)
                return BadRequest();
            
            var newBoardMember = new BoardMember()
            {
                account_id = boardMember.account_id,
                accountId = account,
                board_id = boardMember.board_id,
                boardId = board
            };

            _context.BoardMembers.Add(newBoardMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
