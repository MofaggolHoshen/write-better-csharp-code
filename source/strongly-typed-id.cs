#:package Microsoft.EntityFrameworkCore@10.*
#:package Microsoft.EntityFrameworkCore.InMemory@10.*
#:property PublishAot=false

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StronglyTypedIdExample;

// ============================================================================
// STRONGLY TYPED IDs - Prevent mixing ClientId with AccountId at compile time
// ============================================================================

public readonly record struct ClientId(Guid Value)
{
    public static ClientId New() => new(Guid.NewGuid());
    public static ClientId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}

public readonly record struct AccountId(Guid Value)
{
    public static AccountId New() => new(Guid.NewGuid());
    public static AccountId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}

// ============================================================================
// DOMAIN CLASSES - Client and Account with strongly typed IDs
// ============================================================================

public class Client
{
    public ClientId Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public ICollection<Account> Accounts { get; private set; } = new List<Account>();

    private Client() { } // EF Core

    public Client(string name, string email)
    {
        Id = ClientId.New();
        Name = name;
        Email = email;
        CreatedAt = DateTime.UtcNow;
    }
}

public class Account
{
    public AccountId Id { get; private set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public ClientId ClientId { get; private set; }  // Foreign key - strongly typed!
    public Client Client { get; private set; } = null!;

    private Account() { } // EF Core

    public Account(string accountNumber, ClientId clientId, decimal balance = 0)
    {
        Id = AccountId.New();
        AccountNumber = accountNumber;
        ClientId = clientId;
        Balance = balance;
    }
}

// ============================================================================
// EF CORE VALUE CONVERTERS - Map strongly typed IDs to Guid in database
// ============================================================================

public class ClientIdConverter : ValueConverter<ClientId, Guid>
{
    public ClientIdConverter() : base(id => id.Value, value => new ClientId(value)) { }
}

public class AccountIdConverter : ValueConverter<AccountId, Guid>
{
    public AccountIdConverter() : base(id => id.Value, value => new AccountId(value)) { }
}

// ============================================================================
// DB CONTEXT - Configure entities with value converters
// ============================================================================

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Account> Accounts => Set<Account>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Client configuration
        modelBuilder.Entity<Client>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Id).HasConversion(new ClientIdConverter());
            e.HasMany(c => c.Accounts).WithOne(a => a.Client).HasForeignKey(a => a.ClientId);
        });

        // Account configuration
        modelBuilder.Entity<Account>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Id).HasConversion(new AccountIdConverter());
            e.Property(a => a.ClientId).HasConversion(new ClientIdConverter());
        });
    }
}

// ============================================================================
// MAIN - Usage example
// ============================================================================

public class Program
{
    public static void Main()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("Demo")
            .Options;

        using var db = new AppDbContext(options);

        // Create client and accounts
        var client = new Client("John Doe", "john@example.com");
        var checking = new Account("CHK-001", client.Id, 1000m);
        var savings = new Account("SAV-001", client.Id, 5000m);

        db.Clients.Add(client);
        db.Accounts.AddRange(checking, savings);
        db.SaveChanges();

        // Type safety demo - these would NOT compile:
        // ClientId wrong = checking.Id;  // Error: Cannot convert AccountId to ClientId
        // AccountId also = client.Id;    // Error: Cannot convert ClientId to AccountId

        // Query with strongly typed ID
        var found = db.Clients.Include(c => c.Accounts).First(c => c.Id == client.Id);
        Console.WriteLine($"Client: {found.Name}, Accounts: {found.Accounts.Count}");
    }
}
