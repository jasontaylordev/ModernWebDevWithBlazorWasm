using BlazorDemo.Server.Data;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorDemo.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoListsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TodoListsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/TodoLists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoList>>> GetTodoLists()
    {
        return await _context.TodoLists.ToListAsync();
    }

    // GET: api/TodoLists/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TodoList), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoList>> GetTodoList(int id)
    {
        var todoList = await _context.TodoLists
            .Include(tl => tl.Items)
            .FirstOrDefaultAsync(tl => tl.Id == id);

        if (todoList == null)
        {
            return NotFound();
        }

        return todoList;
    }

    // PUT: api/TodoLists/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutTodoList(int id, TodoList todoList)
    {
        if (id != todoList.Id)
        {
            return BadRequest();
        }

        _context.Entry(todoList).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoListExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/TodoLists
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [ProducesResponseType(typeof(TodoList), StatusCodes.Status201Created)]
    public async Task<ActionResult<TodoList>> PostTodoList(TodoList todoList)
    {
        _context.TodoLists.Add(todoList);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTodoList", new { id = todoList.Id }, todoList);
    }

    // DELETE: api/TodoLists/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTodoList(int id)
    {
        var todoList = await _context.TodoLists.FindAsync(id);
        if (todoList == null)
        {
            return NotFound();
        }

        _context.TodoLists.Remove(todoList);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoListExists(int id)
    {
        return _context.TodoLists.Any(e => e.Id == id);
    }
}
