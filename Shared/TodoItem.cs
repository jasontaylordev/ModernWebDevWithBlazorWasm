namespace BlazorDemo.Shared;

public class TodoItem
{
    public int Id { get; set; }

    public int ListId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Note { get; set; } = string.Empty;

    public TodoItemPriority Priority { get; set; }

    public TodoItemState State { get; set; }

    public bool Done { get; set; }

    public TodoList List { get; set; }
}
