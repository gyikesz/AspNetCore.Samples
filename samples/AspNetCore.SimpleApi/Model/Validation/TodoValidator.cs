using FluentValidation;

namespace AspNetCore.SimpleApi.Model.Validation
{
    public class TodoValidator : AbstractValidator<Todo>
    {
        public TodoValidator()
        {
            RuleFor(todo => todo.Name).NotEmpty();
        }
    }
}
