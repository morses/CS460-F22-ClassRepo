using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Standups.Data;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Standups.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmailListController : ControllerBase
    {
        private readonly ApplicationDbContext _identityContext;

        public EmailListController(ApplicationDbContext identityContext)
        {
            _identityContext = identityContext;
        }

        /// <summary>
        /// ***VERY*** poorly written API endpoint to simulate checking if a user has requested to receive our newsletter.
        /// This exposes us to SQL injection attacks
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        // GET api/emaillist/user%40example.com
        [HttpGet("{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> IsOnMailingList(string email)
        {
            if(email == null)
            {
                return NotFound();
            }
            string cmd = "SELECT UserName,Email,EmailConfirmed FROM AspNetUsers WHERE Email=" + "'" + email + "'";
            Debug.WriteLine(cmd);
            string answer = await RunQueryJSON(cmd, _identityContext);
            if (answer == null)
            {
                return NotFound();
            }

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(answer, "application/json");
        }

        /// <summary>
        /// 'Perfectly reasonable??' helper method to run a query on a db, gather all results
        /// and serialize to JSON
        /// </summary>
        /// <param name="q">SQL to execute, i.e. "SELECT * FROM Meetings"</param>
        /// <param name="db">DbContext to use</param>
        /// <returns>Results of query as a serialized JSON object</returns>
        private async Task<string> RunQueryJSON(string q, DbContext db)
        {
            // One way to execute "raw" sql through entity framework core, aka ADO.NET
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                bool open = command.Connection.State == ConnectionState.Open;
                if (!open)
                {
                    command.Connection.Open();
                }
                try
                {
                    command.CommandText = q;
                    List<DataTable> resultSet = new List<DataTable>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (!reader.IsClosed && reader.HasRows)
                        {
                            var table = new DataTable();
                            table.Load(reader);
                            resultSet.Add(table);
                        }
                        return JsonConvert.SerializeObject(resultSet);
                        // the previous line using NewtonSoft.JSON to turn it into JSON was needed to bypass
                        // a security setting on the built-in serializer, which doesn't allow serializing a DataTable
                        // because it is vulnerable: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/dataset-datatable-dataview/security-guidance
                        // Our use of JsonConvert is apparently itself vulnerable to DoS attack
                    }
                }
                catch (Exception e)
                {
                    return e.ToString();
                }
                finally
                {
                    if (!open)
                    {
                        command.Connection.Close();
                    }
                }
            }
        }
    }
}
