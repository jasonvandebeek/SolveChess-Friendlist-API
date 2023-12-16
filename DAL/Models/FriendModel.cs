
using System.ComponentModel.DataAnnotations;

namespace SolveChess.DAL.Models;

public class FriendModel
{

    [Required]
    public string UserId { get; set; } = null!;
    [Required]
    public string FriendId { get; set; } = null!;

}

