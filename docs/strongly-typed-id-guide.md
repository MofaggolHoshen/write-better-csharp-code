# Strongly Typed IDs in C#

A comprehensive guide to implementing type-safe identifiers that prevent ID mixups at compile time.

## Table of Contents

- [The Problem](#the-problem)
- [The Solution](#the-solution)
- [Implementation](#implementation)
  - [Strongly Typed ID Structure](#strongly-typed-id-structure)
  - [Domain Entities](#domain-entities)
  - [EF Core Integration](#ef-core-integration)
- [Usage Patterns](#usage-patterns)
- [Benefits](#benefits)
- [Variations](#variations)
- [Best Practices](#best-practices)
- [Limitations & Considerations](#limitations--considerations)

---

## The Problem

Consider this common bug-prone code:

```csharp
public void TransferFunds(Guid fromAccountId, Guid toAccountId, Guid clientId, decimal amount)
{
    // Easy to mix up parameters - all are Guid!
}

// Caller accidentally swaps arguments:
TransferFunds(clientId, accountId, otherAccountId, 100m);  // Compiles fine, fails at runtime!
```

**Issues with primitive IDs:**

- No compile-time protection against mixing different ID types
- Runtime errors that are hard to debug
- Self-documenting code requires discipline
- Easy to pass wrong ID to repository/service methods

---

## The Solution

Wrap each ID type in a distinct struct, making the compiler enforce correct usage:

```csharp
public void TransferFunds(AccountId from, AccountId to, ClientId client, decimal amount)
{
    // Type-safe! Can't mix up ClientId with AccountId
}

// This won't compile:
TransferFunds(clientId, accountId, otherAccountId, 100m);  // Compile error!
```

---

## Implementation

### Strongly Typed ID Structure

Use `readonly record struct` for optimal performance and built-in equality:

```csharp
public readonly record struct ClientId(Guid Value)
{
    /// <summary>Creates a new unique ClientId</summary>
    public static ClientId New() => new(Guid.NewGuid());

    /// <summary>Represents an empty/unset ClientId</summary>
    public static ClientId Empty => new(Guid.Empty);

    /// <summary>Returns the string representation of the underlying Guid</summary>
    public override string ToString() => Value.ToString();
}

public readonly record struct AccountId(Guid Value)
{
    public static AccountId New() => new(Guid.NewGuid());
    public static AccountId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}
```

**Why `readonly record struct`?**

| Feature    | Benefit                                                              |
| ---------- | -------------------------------------------------------------------- |
| `readonly` | Immutable - thread-safe, no accidental modification                  |
| `record`   | Auto-generates `Equals`, `GetHashCode`, deconstruction               |
| `struct`   | Stack-allocated, no heap allocation, same memory footprint as `Guid` |

### Domain Entities

Use strongly typed IDs in your domain classes:

```csharp
public class Client
{
    public ClientId Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public ICollection<Account> Accounts { get; private set; } = new List<Account>();

    private Client() { } // Required for EF Core materialization

    public Client(string name, string email)
    {
        Id = ClientId.New();  // Auto-generate ID on creation
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

    private Account() { } // Required for EF Core

    public Account(string accountNumber, ClientId clientId, decimal balance = 0)
    {
        Id = AccountId.New();
        AccountNumber = accountNumber;
        ClientId = clientId;  // Type-safe foreign key assignment
        Balance = balance;
    }
}
```

### EF Core Integration

EF Core doesn't natively understand custom ID types. Use **Value Converters** to map between your strongly typed IDs and the database column type.

#### Step 1: Create Value Converters

```csharp
public class ClientIdConverter : ValueConverter<ClientId, Guid>
{
    public ClientIdConverter()
        : base(
            id => id.Value,           // To database: extract Guid
            value => new ClientId(value))  // From database: wrap in ClientId
    { }
}

public class AccountIdConverter : ValueConverter<AccountId, Guid>
{
    public AccountIdConverter()
        : base(
            id => id.Value,
            value => new AccountId(value))
    { }
}
```

#### Step 2: Configure DbContext

```csharp
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
            e.HasMany(c => c.Accounts)
             .WithOne(a => a.Client)
             .HasForeignKey(a => a.ClientId);
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
```

#### Alternative: Convention-Based Registration (EF Core 8+)

Register converters globally for all properties of a type:

```csharp
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    configurationBuilder
        .Properties<ClientId>()
        .HaveConversion<ClientIdConverter>();

    configurationBuilder
        .Properties<AccountId>()
        .HaveConversion<AccountIdConverter>();
}
```

---

## Usage Patterns

### Creating Entities

```csharp
// Create with auto-generated IDs
var client = new Client("John Doe", "john@example.com");
var checking = new Account("CHK-001", client.Id, 1000m);
var savings = new Account("SAV-001", client.Id, 5000m);

db.Clients.Add(client);
db.Accounts.AddRange(checking, savings);
await db.SaveChangesAsync();
```

### Querying

```csharp
// Find by strongly typed ID
var client = await db.Clients
    .Include(c => c.Accounts)
    .FirstOrDefaultAsync(c => c.Id == clientId);

// Find accounts for a client
var accounts = await db.Accounts
    .Where(a => a.ClientId == clientId)
    .ToListAsync();
```

### API Endpoints

```csharp
[HttpGet("{id}")]
public async Task<ActionResult<ClientDto>> GetClient(Guid id)
{
    var clientId = new ClientId(id);  // Wrap incoming Guid
    var client = await _db.Clients.FindAsync(clientId);
    return client is null ? NotFound() : Ok(client.ToDto());
}
```

### Compile-Time Safety

```csharp
// These will NOT compile - caught at build time!
ClientId wrong = checking.Id;      // Error: Cannot convert AccountId to ClientId
AccountId also = client.Id;        // Error: Cannot convert ClientId to AccountId

void ProcessAccount(AccountId id) { }
ProcessAccount(client.Id);         // Error: Cannot convert ClientId to AccountId
```

---

## Benefits

| Benefit                    | Description                                             |
| -------------------------- | ------------------------------------------------------- |
| **Compile-time safety**    | Impossible to mix up different ID types                 |
| **Self-documenting code**  | Method signatures clearly indicate which ID is expected |
| **Refactoring confidence** | Compiler catches all misuses when changing ID types     |
| **Zero runtime overhead**  | Structs have same memory layout as primitives           |
| **Domain clarity**         | IDs are first-class domain concepts                     |
| **Easier debugging**       | Stack traces show `ClientId` instead of generic `Guid`  |

---

## Variations

### Integer-Based IDs

```csharp
public readonly record struct OrderId(int Value)
{
    public override string ToString() => Value.ToString();
}

public class OrderIdConverter : ValueConverter<OrderId, int>
{
    public OrderIdConverter() : base(id => id.Value, v => new OrderId(v)) { }
}
```

### String-Based IDs

```csharp
public readonly record struct Sku(string Value)
{
    public static Sku Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SKU cannot be empty");
        return new Sku(value.ToUpperInvariant());
    }

    public override string ToString() => Value;
}
```

### With Validation

```csharp
public readonly record struct Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (!IsValid(value))
            throw new ArgumentException($"Invalid email: {value}");
        Value = value.ToLowerInvariant();
    }

    private static bool IsValid(string email)
        => !string.IsNullOrWhiteSpace(email) && email.Contains('@');

    public override string ToString() => Value;
}
```

### Generic Base (Advanced)

```csharp
public interface IStronglyTypedId<T>
{
    T Value { get; }
}

public readonly record struct ClientId(Guid Value) : IStronglyTypedId<Guid>
{
    public static ClientId New() => new(Guid.NewGuid());
}
```

---

## Best Practices

### 1. Use Factory Methods

```csharp
// Good - clear intent
var id = ClientId.New();

// Avoid - less clear
var id = new ClientId(Guid.NewGuid());
```

### 2. Provide Empty/Default Constant

```csharp
public static ClientId Empty => new(Guid.Empty);

// Usage
if (clientId == ClientId.Empty)
    throw new ArgumentException("ClientId is required");
```

### 3. Override ToString()

```csharp
public override string ToString() => Value.ToString();

// Enables clean logging
logger.LogInformation("Processing client {ClientId}", clientId);
```

### 4. Keep IDs Immutable

```csharp
// Good - readonly record struct
public readonly record struct ClientId(Guid Value);

// Avoid - mutable
public struct ClientId { public Guid Value { get; set; } }
```

### 5. Consider JSON Serialization

```csharp
[JsonConverter(typeof(ClientIdJsonConverter))]
public readonly record struct ClientId(Guid Value);

public class ClientIdJsonConverter : JsonConverter<ClientId>
{
    public override ClientId Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        => new(reader.GetGuid());

    public override void Write(Utf8JsonWriter writer, ClientId value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Value);
}
```

### 6. Add Implicit/Explicit Conversion (Optional)

```csharp
public readonly record struct ClientId(Guid Value)
{
    // Explicit conversion - requires cast, safer
    public static explicit operator Guid(ClientId id) => id.Value;
    public static explicit operator ClientId(Guid guid) => new(guid);
}

// Usage
Guid raw = (Guid)clientId;
ClientId typed = (ClientId)rawGuid;
```

---

## Limitations & Considerations

### EF Core & NativeAOT

EF Core's runtime model building is incompatible with NativeAOT/trimming. Options:

```csharp
// Option 1: Disable AOT in file-based apps
#:property PublishAot=false

// Option 2: Use compiled models for production
// https://aka.ms/efcore-docs-trimming
```

### Serialization Overhead

Custom converters needed for:

- JSON (System.Text.Json / Newtonsoft)
- XML serialization
- gRPC/Protobuf

### ORMs & Micro-ORMs

- **EF Core**: Full support via Value Converters
- **Dapper**: Use custom type handlers
- **ADO.NET**: Extract `.Value` manually

### Model Binding (ASP.NET Core)

Create custom model binders or use `[FromRoute]` with Guid and convert:

```csharp
[HttpGet("{id:guid}")]
public IActionResult Get(Guid id) => Get(new ClientId(id));
```

---

## Complete Example

```csharp
#:package Microsoft.EntityFrameworkCore@10.*
#:package Microsoft.EntityFrameworkCore.InMemory@10.*
#:property PublishAot=false

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

// Strongly typed IDs
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

// Domain entities
public class Client
{
    public ClientId Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Account> Accounts { get; private set; } = [];

    private Client() { }
    public Client(string name) { Id = ClientId.New(); Name = name; }
}

public class Account
{
    public AccountId Id { get; private set; }
    public ClientId ClientId { get; private set; }
    public decimal Balance { get; set; }

    private Account() { }
    public Account(ClientId clientId, decimal balance)
    { Id = AccountId.New(); ClientId = clientId; Balance = balance; }
}

// Value converters
public class ClientIdConverter : ValueConverter<ClientId, Guid>
{
    public ClientIdConverter() : base(id => id.Value, v => new ClientId(v)) { }
}

public class AccountIdConverter : ValueConverter<AccountId, Guid>
{
    public AccountIdConverter() : base(id => id.Value, v => new AccountId(v)) { }
}

// DbContext
public class AppDb(DbContextOptions<AppDb> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Account> Accounts => Set<Account>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Client>(e => {
            e.HasKey(c => c.Id);
            e.Property(c => c.Id).HasConversion(new ClientIdConverter());
        });
        mb.Entity<Account>(e => {
            e.HasKey(a => a.Id);
            e.Property(a => a.Id).HasConversion(new AccountIdConverter());
            e.Property(a => a.ClientId).HasConversion(new ClientIdConverter());
            e.HasOne<Client>().WithMany(c => c.Accounts).HasForeignKey(a => a.ClientId);
        });
    }
}

// Usage
var options = new DbContextOptionsBuilder<AppDb>().UseInMemoryDatabase("Demo").Options;
using var db = new AppDb(options);

var client = new Client("Alice");
db.Clients.Add(client);
db.Accounts.Add(new Account(client.Id, 1000m));
db.SaveChanges();

var found = db.Clients.Include(c => c.Accounts).First();
Console.WriteLine($"{found.Name}: {found.Accounts.Count} account(s)");
```

---

## References

- [EF Core Value Conversions](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions)
- [C# Record Structs](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record)
- [Domain-Driven Design - Value Objects](https://martinfowler.com/bliki/ValueObject.html)
- [StronglyTypedId NuGet Package](https://www.nuget.org/packages/StronglyTypedId) - Source generator alternative
