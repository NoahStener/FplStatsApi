using FplStatsApi.DTOs;
using FplStatsApi.Models;
using FplStatsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace FplStatsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FplController : ControllerBase
    {
        private readonly FplService _fplService;

        public FplController(FplService fplService)
        {
            _fplService = fplService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            try
            {
                var data = await _fplService.GetBootstrapDataAsync();
                var players = data.RootElement.GetProperty("elements");

                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving players: {ex.Message}");
            }

        }

        [HttpGet("players/simpleData")]
        public async Task<IActionResult> GetPlayerData()
        {
            var data = await _fplService.GetBootstrapDataAsync();
            var playerJson = data.RootElement.GetProperty("elements");

            var players = playerJson.EnumerateArray().Select(p => new FplPlayer
            {
                Id = p.GetProperty("id").GetInt32(),
                FullName = $"{p.GetProperty("first_name").GetString()} {p.GetProperty("second_name").GetString()}",
                TeamId = p.GetProperty("team").GetInt32(),
                PositionId = p.GetProperty("element_type").GetInt32(),
                ScoredGoals = p.GetProperty("goals_scored").GetInt32(),
                Assists = p.GetProperty("assists").GetInt32(),
                TotalPoints = p.GetProperty("total_points").GetInt32()

            }).ToList();
            return Ok(players);
        }

        [HttpGet("players/by-position/{position}")]
        public async Task<IActionResult> GetPlayersByPosition(FplPosition position)
        {
            var data = await _fplService.GetBootstrapDataAsync();
            var playersJson = data.RootElement.GetProperty("elements");

            var players = playersJson.EnumerateArray()
                .Where(p => p.GetProperty("element_type").GetInt32() == (int)position)
                .ToList();

            if (!players.Any())
            {
                return NotFound($"No players found for position {position}");
            }

            return position switch
            {
                FplPosition.GoalKeeper => Ok(players.Select(p => new GoalkeeperDto
                {
                    Name = $"{p.GetProperty("first_name").GetString()} {p.GetProperty("second_name").GetString()}",
                    Photo = $"{p.GetProperty("photo").GetString()}",
                    TeamId = p.GetProperty("team").GetInt32(),
                    CleanSheets = p.GetProperty("clean_sheets").GetInt32(),
                    GoalsConceded = p.GetProperty("goals_conceded").GetInt32(),
                    Saves = p.GetProperty("saves").GetInt32(),
                    PenaltiesSaved = p.GetProperty("penalties_saved").GetInt32()
                })),

                FplPosition.Defender => Ok(players.Select(p => new DefenderDto
                {
                    Name = $"{p.GetProperty("first_name").GetString()} {p.GetProperty("second_name").GetString()}",
                    Photo = $"{p.GetProperty("photo").GetString()}",
                    TeamId = p.GetProperty("team").GetInt32(),
                    CleanSheets = p.GetProperty("clean_sheets").GetInt32(),
                    GoalsConceded = p.GetProperty("goals_conceded").GetInt32(),
                    Goals = p.GetProperty("goals_scored").GetInt32(),
                    Assists = p.GetProperty("assists").GetInt32(),
                    YellowCards = p.GetProperty("yellow_cards").GetInt32(),
                    RedCards = p.GetProperty("red_cards").GetInt32(),
                    OwnGoals = p.GetProperty("own_goals").GetInt32()
                })),

                FplPosition.Midfielder => Ok(players.Select(p => new MidfielderDto
                {
                    Name = $"{p.GetProperty("first_name").GetString()} {p.GetProperty("second_name").GetString()}",
                    Photo = $"{p.GetProperty("photo").GetString()}",
                    TeamId = p.GetProperty("team").GetInt32(),
                    Goals = p.GetProperty("goals_scored").GetInt32(),
                    Assists = p.GetProperty("assists").GetInt32(),
                    ExpectedGoals = p.GetProperty("expected_goals").GetString(),
                    ExpectedAssists = p.GetProperty("expected_assists").GetString(),
                    Starts = p.GetProperty("starts").GetInt32(),
                    CleanSheets = p.GetProperty("clean_sheets").GetInt32(),
                    YellowCards = p.GetProperty("yellow_cards").GetInt32(),
                    RedCards = p.GetProperty("red_cards").GetInt32(),

                })),

                FplPosition.Forward => Ok(players.Select(p => new ForwardDto
                {
                    Name = $"{p.GetProperty("first_name").GetString()} {p.GetProperty("second_name").GetString()}",
                    Photo = $"{p.GetProperty("photo").GetString()}",
                    TeamId = p.GetProperty("team").GetInt32(),
                    Goals = p.GetProperty("goals_scored").GetInt32(),
                    ExpectedGoals = p.GetProperty("expected_goals").GetString(),
                    ExpectedAssists = p.GetProperty("expected_assists").GetString(),
                    Starts = p.GetProperty("starts").GetInt32(),
                    Assists = p.GetProperty("assists").GetInt32(),
                    YellowCards = p.GetProperty("yellow_cards").GetInt32(),
                    RedCards = p.GetProperty("red_cards").GetInt32()
                })),

                _ => BadRequest($"Invalid Position: {position}")
            };
        }

        [HttpGet("players/top-scorers")]
        public async Task<IActionResult> GetTopScorers(int limit = 10)
        {
            try
            {
                var data = await _fplService.GetBootstrapDataAsync();
                var playersJson = data.RootElement.GetProperty("elements");

                var topScorers = playersJson.EnumerateArray()
                    .OrderByDescending(p => p.GetProperty("goals_scored").GetInt32())
                    .Take(limit)
                    .Select(p => new
                    {
                        Name = $"{p.GetProperty("first_name").GetString()} {p.GetProperty("second_name").GetString()}",
                        Goals = p.GetProperty("goals_scored").GetInt32(),
                        Position = p.GetProperty("element_type").GetInt32(),
                        TotalPoints = p.GetProperty("total_points").GetInt32()
                    })
                    .ToList();

                return Ok(topScorers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving top scorers: {ex.Message}");
            }
        }

        [HttpGet("players/most-assists")]
        public async Task<IActionResult> GetMostAssists(int limit = 10)
        {
            try
            {
                var data = await _fplService.GetBootstrapDataAsync();
                var playersJson = data.RootElement.GetProperty("elements");

                var topAssists = playersJson.EnumerateArray()
                    .OrderByDescending(p => p.GetProperty("assists").GetInt32())
                    .Take(limit)
                    .Select(p => new
                    {
                        Name = $"{p.GetProperty("first_name").GetString()} {p.GetProperty("second_name").GetString()}",
                        Assists = p.GetProperty("assists").GetInt32(),
                        Position = p.GetProperty("element_type").GetInt32(),
                        TotalPoints = p.GetProperty("total_points").GetInt32()
                    })
                    .ToList();

                return Ok(topAssists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving most assists: {ex.Message}");
            }
        }





    }
}
