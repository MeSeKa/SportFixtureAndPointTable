using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportFixtureAndPointTable.Models;

namespace SportFixtureAndPointTable.Controllers
{
    public class TeamsController : Controller
    {
        private readonly SportFixtureAndPointsTableDBContext _context;

        public TeamsController(SportFixtureAndPointsTableDBContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Takım bilgilerini al
            var team = await _context.Teams
                .Include(t => t.Players)
                .Include(t => t.FixtureHomeTeams)
                    .ThenInclude(f => f.MatchResult)
                .Include(t => t.FixtureAwayTeams)
                    .ThenInclude(f => f.MatchResult)
                .FirstOrDefaultAsync(m => m.TeamId == id);

            if (team == null)
            {
                return NotFound();
            }

            // Maç istatistiklerini hesapla
            int wonMatches = 0, lostMatches = 0, drawnMatches = 0, notYetPlayedMatches = 0;

            // Ev sahibi olduğu maçları kontrol et
            foreach (var fixture in team.FixtureHomeTeams)
            {
                if (fixture.MatchResult != null)
                {
                    if (fixture.MatchResult.AwayScore == null || fixture.MatchResult.HomeScore == null)
                    {
                        notYetPlayedMatches++;
                    }
                    else if (fixture.MatchResult.HomeScore > fixture.MatchResult.AwayScore)
                    {
                        wonMatches++;
                    }
                    else if (fixture.MatchResult.HomeScore < fixture.MatchResult.AwayScore)
                    {
                        lostMatches++;
                    }
                    else
                    {
                        drawnMatches++;
                    }
                }
            }

            // Deplasman olduğu maçları kontrol et
            foreach (var fixture in team.FixtureAwayTeams)
            {
                if (fixture.MatchResult != null)
                {
                    if (fixture.MatchResult.AwayScore == null || fixture.MatchResult.HomeScore == null)
                    {
                        notYetPlayedMatches++;
                    }
                    else if (fixture.MatchResult.AwayScore > fixture.MatchResult.HomeScore)
                    {
                        wonMatches++;
                    }
                    else if (fixture.MatchResult.AwayScore < fixture.MatchResult.HomeScore)
                    {
                        lostMatches++;
                    }
                    else
                    {
                        drawnMatches++;
                    }
                }
            }

            // İstatistikleri ViewBag ile View'a gönder
            ViewBag.WonMatches = wonMatches;
            ViewBag.LostMatches = lostMatches;
            ViewBag.DrawnMatches = drawnMatches;
            ViewBag.NotYetPlayedMatches = notYetPlayedMatches;

            return View(team);
        }


        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamName,City")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamId,TeamName,City")] Team team)
        {
            if (id != team.TeamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.TeamId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.TeamId == id);
        }
    }
}
