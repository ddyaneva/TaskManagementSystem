using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IMapper _mapper;
        public AccountApiController(TaskManagerContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public  IEnumerable<AccountDto> OnGet()
            =>  _mapper.Map<IEnumerable<Account>, IEnumerable<AccountDto>>(_context.Accounts.ToList());
          
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<AccountDto>> OnGetByIdAsync(int id)
        {
            var item = await _context.Accounts.FindAsync(id);

            if (item == null)
                return BadRequest();

            return _mapper.Map<Account, AccountDto>(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActorAsync(AccountDto accountDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction();

            var account = _mapper.Map<AccountDto, Account>(accountDto);

            var newAccount = new Account()
                {
                    first_name = account.first_name,
                    last_name = account.last_name,
                    email = account.email,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

            _context.Accounts.Add(newAccount);
            await  _context.SaveChangesAsync();

            accountDto.account_id = newAccount.account_id;

            return CreatedAtAction(
                 nameof(OnGet),
                 new { id = account.account_id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, AccountDto accountDto)
        {
            var account = _mapper.Map<AccountDto,Account>(accountDto);

            if (!ModelState.IsValid)
                return RedirectToAction();

            var updatedAccount = _context.Accounts
                                    .FirstOrDefault(x => x.account_id == id);

            if (updatedAccount == null)
                return BadRequest();

            updatedAccount.first_name = account.first_name;
            updatedAccount.last_name = account.last_name;
            updatedAccount.email = account.email;

            updatedAccount.updated_at = DateTime.Now;
            await _context.SaveChangesAsync();

        return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var account = _context.Accounts
                .FirstOrDefault(x => x.account_id == id);

            if (account == null)
                return BadRequest();

            _context.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
