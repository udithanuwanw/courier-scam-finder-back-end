namespace courier_scam_finder_back_end.Models;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
