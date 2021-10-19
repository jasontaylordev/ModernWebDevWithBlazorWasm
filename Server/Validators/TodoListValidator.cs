using BlazorDemo.Server.Data;
using BlazorDemo.Shared;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlazorDemo.Server.Validators
{
    public class TodoListValidator : Shared.TodoListValidator
    {
        private readonly ApplicationDbContext _context;

        public TodoListValidator(ApplicationDbContext context) : base()
        {
            _context = context;

            RuleFor(v => v.Title)
                .MustAsync(BeUniqueTitle)
                    .WithMessage("'Title' must be unique.");
        }

        public async Task<bool> BeUniqueTitle(TodoList list, string title, CancellationToken cancellationToken)
        {
            return await _context.TodoLists
                .Where(tl => tl.Id != list.Id)
                .AllAsync(tl => tl.Title != title, cancellationToken);
        }
    }
}
