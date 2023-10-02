using adoptera_hund.api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace adoptera_hund.api.Data;

public class AdopteraHundContext : IdentityDbContext<UserModel>
{
    public DbSet<DogModel> DogsDataBase {get; set;}
    public AdopteraHundContext(DbContextOptions options) : base(options)
    {

    }
}
