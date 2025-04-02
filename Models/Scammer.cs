namespace courier_scam_finder_back_end.Models;
using System.ComponentModel.DataAnnotations;

public class Scammer
{
    [Key]
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string IdNumber { get; set; }
    public required string Address { get; set; }
}
