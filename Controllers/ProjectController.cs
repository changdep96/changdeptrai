using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManage.Data;
using ProjectManage.Models;

namespace NetcoreReact_1_master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        //getall: 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> getAllProject()
        {
            return await _context.projects.ToListAsync();
        }
        // getprojct by name: /api/Project/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectByName(int id)
        {
            var nameProject = await _context.projects.FindAsync(id);
            if (nameProject == null)
                return BadRequest();
            else
                return nameProject;


        }

        // post-create new Project: api/Project
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project NewProject)
        {
            _context.projects.Add(NewProject);
            await _context.SaveChangesAsync();
            //if(newOne == null)
            // return console.Write("lỗi khi thêm project mới");
            return CreatedAtAction(nameof(GetProjectByName), new { id = NewProject.Id }, NewProject);
        }

        // put-edit newproject: api/Project/1
        [HttpPut("{id}")]
        public async Task<ActionResult<Project>> EditProject(int id, Project NewProject)
        {
            if (id != NewProject.Id)
            {
                return BadRequest();
            }

            _context.Entry(NewProject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewProjectExists(id))
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
        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> DeleteProject(int id)

        {
            var oldProject = await _context.projects.FindAsync(id);
            if (oldProject == null)
            {
                return NotFound();
            }
            _context.projects.Remove(oldProject);
            await _context.SaveChangesAsync();
            return oldProject;

        }
        // NewProjectExists
        private bool NewProjectExists(int id)
        {
            return _context.projects.Any(e => e.Id == id);
        }

    }
}