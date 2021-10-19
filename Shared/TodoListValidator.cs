using FluentValidation;

namespace BlazorDemo.Shared
{
    public class TodoListValidator : AbstractValidator<TodoList>
    {
        public TodoListValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty();
        }
    }
}
