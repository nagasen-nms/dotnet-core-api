using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoApi.Models;
using Microsoft.Data.SqlClient;
using System;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                // _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.TodoItems.Add(new TodoItem { Commodity = "Item1" });
                _context.SaveChanges();
            }
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItem()
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "tcp:mpwmoc01-sqlserver.database.windows.net,1433";
            builder.UserID = "mpwmoc01sqlserver";
            builder.Password = "B1xusB1xus";
            builder.InitialCatalog = "mpwmoc01-sqldatabase";

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                // String sql = "SELECT TOP (10) * FROM DATAMART.Item";
                // String sql = "SELECT TOP (1000) [Commodity], [ItemID], [SubCategory], [Category], [Item], [CommodityDetail] FROM [DATAMART].[Item]";
                String sql = "SELECT TOP (1000) Commodity, ItemID, SubCategory, Category, Item, CommodityDetail FROM DATAMART.Item";
                // String sql = "SELECT Currency, CurrencyAbbr FROM DATAMART.Currency";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                            _context.TodoItems.Add(new TodoItem
                            {
                                Commodity = reader.GetString(0),
                                ItemID = reader.GetInt32(1),
                                SubCategory = reader.GetString(2),
                                Category = reader.GetString(3),
                                Item = reader.GetString(4),
                                CommodityDetail = reader.GetString(5)
                            });
                            _context.SaveChanges();
                        }
                    }
                }
            }
            
            

            return await _context.TodoItems.ToListAsync();
        }

        // // GET: api/Todo/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        // {
        //     var todoItem = await _context.TodoItems.FindAsync(id);

        //     if (todoItem == null)
        //     {
        //         return NotFound();
        //     }

        //     return todoItem;
        // }

        // // PUT: api/Todo/5
        // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // // more details see https://aka.ms/RazorPagesCRUD.
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        // {
        //     if (id != todoItem.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(todoItem).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!TodoItemExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // // POST: api/Todo
        // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // // more details see https://aka.ms/RazorPagesCRUD.
        // [HttpPost]
        // public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        // {
        //     _context.TodoItems.Add(todoItem);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        // }

        // // DELETE: api/Todo/5
        // [HttpDelete("{id}")]
        // public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        // {
        //     var todoItem = await _context.TodoItems.FindAsync(id);
        //     if (todoItem == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.TodoItems.Remove(todoItem);
        //     await _context.SaveChangesAsync();

        //     return todoItem;
        // }

        // private bool TodoItemExists(long id)
        // {
        //     return _context.TodoItems.Any(e => e.Id == id);
        // }
    }
}
