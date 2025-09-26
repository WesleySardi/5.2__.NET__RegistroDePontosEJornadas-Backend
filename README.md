# BMA Time Entries –– Backend

### ASP.NET Core Web API + SQL Server

## Descrição do Projeto

Este projeto é uma API para **gestão de registros de ponto (batidas)** e jornadas de trabalho. Ele utiliza:

* **Backend:** ASP.NET Core Web API com **Entity Framework Core** (SQL Server)
* **Frontend:** React (não incluso neste repositório)
* **Objetivo:** CRUD completo para registros de ponto com validação, paginação e ordenação.

---

## Arquitetura

O backend segue uma estrutura limpa:

```
Controllers → Services → Repositories → Database
```

* **Controllers:** Recebem requisições HTTP, fazem validação de entrada e retornam respostas HTTP.
* **Services:** Contêm a lógica de negócio e regras de validação adicionais.
* **Repositories:** Encapsulam o acesso ao banco via **Entity Framework Core**.
* **DTOs:** Requests e responses, evitando expor entidades diretamente.
* **AutoMapper:** Para conversão entre entidades e DTOs/ViewModels.
* ...

**Decisões técnicas:**

* **EF Core:** Facilita mapeamento objeto-relacional e migrations.
* **DTOs:** Isolam camada de API do modelo de domínio.
* **PagedResult:** Suporte a paginação genérica.
* ...

---

## Banco de Dados (SQL Server)

Tabela: `TimeEntries`

| Coluna       | Tipo             | Restrição                                  |
| ------------ | ---------------- | ------------------------------------------ |
| Id           | UNIQUEIDENTIFIER | PK                                         |
| EmployeeId   | NVARCHAR         | NOT NULL                                   |
| EmployeeName | NVARCHAR         | NOT NULL                                   |
| Timestamp    | DATETIME2        | NOT NULL                                   |
| Type         | NVARCHAR         | NOT NULL (Entrada, Saída, Intervalo)       |
| Location     | NVARCHAR         | NULL                                       |
| Notes        | NVARCHAR         | NULL                                       |
| CreatedAt    | DATETIME2        | NOT NULL (seed automático)                 |
| UpdatedAt    | DATETIME2        | NULL                                       |

**Índices:**

* `EmployeeId`
* `Timestamp`
* `Type`

**Migrations:** Usadas para criar/alterar tabelas e colunas, garantindo histórico de alterações do schema.

---

## Endpoints da API

### Time Entries

| Método | Rota                     | Descrição                                                                                                                                         |
| ------ | ------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------- |
| GET    | `/api/TimeEntries`      | Lista todos os registros com filtros, paginação e ordenação. Parâmetros opcionais: `employeeId`, `type`, `from`, `to`, `page`, `pageSize`, `sort` |
| GET    | `/api/TimeEntries/{id}` | Retorna um registro específico pelo ID                                                                                                            |
| POST   | `/api/TimeEntries`      | Cria um novo registro                                                                                                                             |
| PUT    | `/api/TimeEntries/{id}` | Atualiza um registro existente                                                                                                                    |
| DELETE | `/api/TimeEntries/{id}` | Remove um registro existente                                                                                                                      |

---

### Exemplos de requisições

**GET com filtros e paginação**

```http
GET /api/TimeEntries?employeeId=EMP001&type=Entrada&from=2025-09-01&to=2025-09-23&page=1&pageSize=10&sort=timestamp_desc
```

**GET por id**

```http
GET /api/TimeEntries/861C7B37-15C5-4736-A884-8AFBB16FEF07
```

**POST para criar registro**

```http
POST /api/TimeEntries

Content-Type: application/json

{
    "employeeId": "EMP001",
    "employeeName": "João Silva",
    "timestamp": "2025-09-18T09:51:04",
    "type": "Entrada",
    "location": "Portaria A",
    "notes": "Entrada padrão"
}
```

**PUT para atualizar registro**

```http
PUT /api/TimeEntries/d536caa3-5066-4aa9-9a4e-56036d47ea64

Content-Type: application/json

{
    "employeeId": "EMP001",
    "employeeName": "João Silva",
    "timestamp": "2025-09-18T10:00:00",
    "type": "Saida",
    "location": "Portaria A",
    "notes": "Saída para almoço"
}
```

**DELETE para remover registro**

```http
DELETE /api/TimeEntries/d536caa3-5066-4aa9-9a4e-56036d47ea64
```

---

## Retorno GET por id

```json
{
  "id": "861c7b37-15c5-4736-a884-8afbb16fef07",
  "employeeId": "EMP002",
  "employeeName": "Maria Oliveira",
  "timestamp": "2025-09-25T01:06:00",
  "type": "Entrada",
  "location": "",
  "notes": "",
  "createdAt": "2025-09-25T00:06:18.2104822",
  "updatedAt": "2025-09-25T20:06:57.9552145"
}
```

## Paginação

A API retorna uma estrutura `PagedResult<T>`:

```json
{
  "items": [
    {
      "id": "861c7b37-15c5-4736-a884-8afbb16fef07",
      "employeeId": "EMP002",
      "employeeName": "Maria Oliveira",
      "timestamp": "2025-09-25T01:06:00",
      "type": "Entrada",
      "location": "",
      "notes": "",
      "createdAt": "2025-09-25T00:06:18.2104822",
      "updatedAt": "2025-09-25T20:06:57.9552145"
    },
    {
      "id": "3998369a-a5b9-4f03-85a3-f1fa6167d2de",
      "employeeId": "EMP001",
      "employeeName": "João Silva",
      "timestamp": "2025-09-24T15:06:00",
      "type": "Saida",
      "location": "",
      "notes": "",
      "createdAt": "2025-09-25T00:06:18.2104822",
      "updatedAt": "2025-09-25T00:09:31.6644439"
    },
    {
      "id": "1c501853-3acd-45ef-a88a-a563a4e3995c",
      "employeeId": "EMP001",
      "employeeName": "João Silva",
      "timestamp": "2025-09-24T11:06:18.1719402",
      "type": "Entrada",
      "location": "Portaria A",
      "notes": "Entrada padrão",
      "createdAt": "2025-09-25T00:06:18.2104822",
      "updatedAt": null
    }
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 20,
  "totalPages": 3
}
```

**Filtros suportados:**

* `employeeId` → string
* `type` → string (Entrada, Saida, Intervalo)
* `from` → data inicial
* `to` → data final

**Ordenação:** `sort` (`timestamp_asc`, `timestamp_desc`, `employeeName_asc`, `employeeName_desc`)

---

## Configuração do Projeto

### 1. Ferramentas necessárias

* .NET SDK
* EF Core Tools
* SQL Server
* (Opcional) SQL Server Management Studio (SSMS)

### 2. Banco de Dados

Atualize a string de conexão no `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BmaTimeEntriesDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 3. Rodando o Backend

```bash
# Acessar pasta do projeto
cd 5.2__.NET__RegistroDePontosEJornadas-Backend\ProjetoBMA

# Restaurar pacotes
dotnet restore

# Aplicar migrations
dotnet ef database update

# Executar API
dotnet run --launch-profile "https" --project ProjetoBMA.csproj
```

**Swagger** disponível em: `https://localhost:7102/swagger/index.html`

### 4. Rodando os testes

```bash
# Acessar pasta do projeto
cd 5.2__.NET__RegistroDePontosEJornadas-Backend/ProjetoBMA.Tests

# Executar os testes
dotnet test
```

---

## Ferramentas e Tecnologias

* ASP.NET Core 8.0
* EF Core 9.0
* AutoMapper
* SQL Server
* .NET CLI (dotnet ef)
* C# 11
* Logging nativo (`ILogger`)
* DTOs e ViewModels
* Paginação genérica via `PagedResult<T>`
* ...

---

## Seeds de dados.

### 1. Cria uma seed de dados inicial ao executar o projeto

```bash
using Microsoft.EntityFrameworkCore;
using ProjetoBMA.Domain.Entities;

namespace ProjetoBMA.Data
{
    public static class SeedData
    {
        public static async Task EnsureSeedDataAsync(AppDbContext context)
        {
            if (await context.TimeEntries.AnyAsync()) return;

            var list = new List<TimeEntry>
            {
                new TimeEntry
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = "EMP001",
                    EmployeeName = "João Silva",
                    Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(8),
                    Type = "Entrada",
                    Location = "Portaria A",
                    Notes = "Entrada padrão",
                    CreatedAt = DateTime.UtcNow
                },
                new TimeEntry
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = "EMP001",
                    EmployeeName = "João Silva",
                    Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(12),
                    Type = "Intervalo",
                    CreatedAt = DateTime.UtcNow
                },
                new TimeEntry
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = "EMP002",
                    EmployeeName = "Maria Oliveira",
                    Timestamp = DateTime.UtcNow.AddHours(-2),
                    Type = "Saida",
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.TimeEntries.AddRange(list);
            await context.SaveChangesAsync();
        }
    }
}
```

---

## Integração Contínua (CI) com GitHub Actions

O projeto possui um workflow simples de **Integração Contínua** configurado para rodar automaticamente os testes sempre que houver um **push** em qualquer branch ou **pull request**.

O arquivo de workflow `.github/workflows/dotnet-tests.yml` realiza os seguintes passos:

1. **Checkout do repositório**  
   Utiliza `actions/checkout@v3` para obter a versão mais recente do código.

2. **Configuração do .NET SDK**  
   Com `actions/setup-dotnet@v3`, garante que o **.NET 8.0** esteja disponível para o pipeline.

3. **Restauração de dependências**  
   Executa `dotnet restore` no projeto de testes `ProjetoBMA.Tests` para baixar todos os pacotes necessários.

4. **Build do projeto**  
   Compila o projeto de testes sem restaurar dependências novamente, usando `dotnet build --no-restore`.

5. **Execução dos testes**  
   Roda `dotnet test` para garantir que todos os testes passem. O `--no-build` evita recompilar o projeto e `--verbosity normal` exibe os logs detalhados da execução.

> Com isso, qualquer alteração no código que quebre testes será identificada automaticamente antes de realizar o merge, mantendo a qualidade do projeto.

---

## 📂 Estrutura do Projeto

```bash
│   appsettings.Development.json
│   appsettings.json
│   Dockerfile
│   Program.cs
│   ProjetoBMA.csproj
│   ProjetoBMA.csproj.user
│   ProjetoBMA.http
│   ProjetoBMA.sln
│
├───Controllers
│       TimeEntriesController.cs
│
├───Data
│       AppDbContext.cs
│       SeedData.cs
│
├───Domain
│   ├───Entities
│   │       TimeEntry.cs
│   │
│   └───Enums
│           TimeEntryType.cs
│
├───DTOs
│   ├───Commands
│   │       CreateTimeEntryCommand.cs
│   │       UpdateTimeEntryCommand.cs
│   │
│   ├───Queries
│   │       TimeEntryQueryParametersQuery.cs
│   │
│   ├───Results
│   │       TimeEntryResult.cs
│   │
│   └───ViewModels
│           TimeEntryViewModel.cs
│
├───Mappings
│       MappingProfile.cs
│
├───Middleware
│       ExceptionMiddleware.cs
│
├───Migrations
│       20250925030604_InitialCreate.cs
│       20250925030604_InitialCreate.Designer.cs
│       AppDbContextModelSnapshot.cs
│
├───Properties
│       launchSettings.json
│
├───Repositories
│   │   TimeEntryRepository.cs
│   │
│   └───Interfaces
│           ITimeEntryRepository.cs
│
├───Services
│   │   TimeEntryService.cs
│   │
│   └───Interfaces
│           ITimeEntryService.cs
│
└───Utils
        PagedResult.cs
        QueryableExtensions.cs
        TimeEntryHelper.cs
```

---

## 📂 Estrutura do Projeto de Testes

```bash
│   appsettings.Development.json
│   appsettings.json
│   Program.cs
│   ProjetoBMA.Tests.csproj
│   ProjetoBMA.Tests.csproj.user
│   ProjetoBMA.Tests.http
│   ProjetoBMA.Tests.sln
│
├───Controllers
│       TimeEntriesControllerTests.cs
│
├───Properties
│       launchSettings.json
│
└───Services
        TimeEntryServiceTests.cs
```

---

## Considerações Finais

* **Validações:** Todos os campos obrigatórios com Data Annotations.
* **Auditoria:** `CreatedAt` e `UpdatedAt`.
* **Boa prática:** Repositórios retornam entidades, Services fazem mapeamento e regras de negócio.
* **CORS:** Configurado para permitir consumo da API pelo frontend.
* ...

## 🧑‍💻 Autor

Desenvolvido por **Wesley Erik Sardi**

🚀 Backend em **ASP.NET Core Web API + SQL Server**
