using BlazorDemo.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorDemo.Server.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoList>().HasData(
            new TodoList { Id = 1, Title = "Todo List" });

        modelBuilder.Entity<TodoItem>().HasData(
            new TodoItem { Id = 1, ListId = 1, Title = "Make a todo list" },
            new TodoItem { Id = 2, ListId = 1, Title = "Check off the first item" },
            new TodoItem { Id = 3, ListId = 1, Title = "Realise you've already done two things on the list!" },
            new TodoItem { Id = 4, ListId = 1, Title = "Reward yourself with a nice, long nap" });
    }
}
