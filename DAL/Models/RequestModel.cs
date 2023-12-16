
using System.ComponentModel.DataAnnotations;

namespace SolveChess.DAL.Models;

public class RequestModel
{

    [Required]
    public string UserId { get; set; } = null!;
    [Required]
    public string SenderId { get; set; } = null!;

}

