# CocktailCollator

## Getting Started

### Local Development Configuration (User Secrets)

Sensitive configuration values are not stored within the repository. Instead, they are managed via **.NET User Secrets**, which stores secrets on your local machine.

#### How to Open and Edit Secrets

##### Option A: Visual Studio IDE (Recommended)
1. In Visual Studio, right-click the **`CocktailCollator.Web`** project in the Solution Explorer.
2. Select **Manage User Secrets**.
   *(Visual Studio will automatically initialise and open `secrets.json` in the editor).*
3. Add your local configuration (see template below) and save the file.

##### Option B: .NET CLI
From the repository root:
1. Initialize user secrets (if not already initialized):
   ```bash
   dotnet user-secrets init --project CocktailCollator.Web
   ```
2. Set configuration values using the CLI:
   ```bash
   dotnet user-secrets set "ConnectionStrings:CocktailCollator" "<Your-SQL-Server-Connection-String>" --project CocktailCollator.Web
   dotnet user-secrets set "FileStorePath" "<Your-Local-Storage-Path>" --project CocktailCollator.Web
   ```

### `secrets.json` Template

Place the following JSON structure in your `secrets.json` file:

```json
{
  "ConnectionStrings": {
    "CocktailCollator": "Server=YOUR_SERVER;Database=CocktailCollatorDB;User Id=cocktail_collator;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  },
  "FileStorePath": "C:\\CocktailCollator"
}
```

#### Required Configuration Keys

| Key | Description | Example |
| :--- | :--- | :--- |
| `ConnectionStrings:CocktailCollator` | SQL Server connection string for the app and EF Core | `Server=192.168.56.5;Database=CocktailCollatorDB;User Id=cocktail_collator;Password=...;TrustServerCertificate=True;` |
| `FileStorePath` | Local directory path for file attachments | `C:\CocktailCollator` |