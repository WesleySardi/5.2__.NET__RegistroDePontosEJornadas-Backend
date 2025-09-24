# BMA Time Entries – Backend

### Nível Pleno — ASP.NET Core Web API + SQL Server

## Descrição do Projeto

Este projeto é uma API para **gestão de registros de ponto (batidas)** e jornadas de trabalho. Ele faz parte do teste técnico da **BMA Sistemas** e utiliza:

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

**Decisões técnicas:**

* **EF Core:** Facilita mapeamento objeto-relacional e migrations.
* **DTOs:** Isolam camada de API do modelo de domínio.
* **PagedResult:** Suporte a paginação genérica.

---

## Banco de Dados (SQL Server)

Tabela: `TimeEntries`

| Coluna       | Tipo             | Restrição                                  |
| ------------ | ---------------- | ------------------------------------------ |
| Id           | UNIQUEIDENTIFIER | PK                                         |
| EmployeeId   | NVARCHAR(50)     | NOT NULL                                   |
| EmployeeName | NVARCHAR(200)    | NOT NULL                                   |
| Timestamp    | DATETIME2        | NOT NULL                                   |
| Type         | INT              | NOT NULL (enum: Entrada, Saída, Intervalo) |
| Location     | NVARCHAR(100)    | NULL                                       |
| Notes        | NVARCHAR(MAX)    | NULL                                       |
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
| GET    | `/api/time-entries`      | Lista todos os registros com filtros, paginação e ordenação. Parâmetros opcionais: `employeeId`, `type`, `from`, `to`, `page`, `pageSize`, `sort` |
| GET    | `/api/time-entries/{id}` | Retorna um registro específico pelo ID                                                                                                            |
| POST   | `/api/time-entries`      | Cria um novo registro                                                                                                                             |
| PUT    | `/api/time-entries/{id}` | Atualiza um registro existente                                                                                                                    |
| DELETE | `/api/time-entries/{id}` | Remove um registro existente                                                                                                                      |

---

### Exemplos de requisições

**GET com filtros e paginação**

```http
GET /api/time-entries?employeeId=EMP001&type=0&from=2025-09-01&to=2025-09-23&page=1&pageSize=10&sort=timestamp_desc
```

**POST para criar registro**

```http
POST /api/time-entries
Content-Type: application/json

{
    "employeeId": "EMP001",
    "employeeName": "João Silva",
    "timestamp": "2025-09-18T09:51:04",
    "type": 0, // Entrada
    "location": "Portaria A",
    "notes": "Entrada padrão"
}
```

**PUT para atualizar registro**

```http
PUT /api/time-entries/d536caa3-5066-4aa9-9a4e-56036d47ea64
Content-Type: application/json

{
    "employeeId": "EMP001",
    "employeeName": "João Silva",
    "timestamp": "2025-09-18T10:00:00",
    "type": 1, // Saída
    "location": "Portaria A",
    "notes": "Saída para almoço"
}
```

**DELETE para remover registro**

```http
DELETE /api/time-entries/d536caa3-5066-4aa9-9a4e-56036d47ea64
```

---

## Paginação

A API retorna uma estrutura `PagedResult<T>`:

```json
{
  "items": [...],
  "totalCount": 50,
  "page": 1,
  "pageSize": 20,
  "totalPages": 3
}
```

**Filtros suportados:**

* `employeeId` → string
* `type` → enum (0 = Entrada, 1 = Saída, 2 = Intervalo)
* `from` → data inicial
* `to` → data final

**Ordenação:** `sort` (`timestamp_asc`, `timestamp_desc`, `employeeName_asc`, `employeeName_desc`)

---

## Configuração do Projeto

### 1. Banco de Dados

Atualize a string de conexão no `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BmaTimeEntriesDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 2. Rodando o Backend

```bash
# Restaurar pacotes
dotnet restore

# Aplicar migrations
dotnet ef database update

# Executar API
dotnet run
```

**Swagger** disponível em: `https://localhost:{porta}/swagger`

---

## Ferramentas e Tecnologias

* ASP.NET Core 9.0
* EF Core 9.0
* AutoMapper
* SQL Server
* .NET CLI (dotnet ef)
* C# 11
* Logging nativo (`ILogger`)
* DTOs e ViewModels
* Paginação genérica via `PagedResult<T>`

---

## Considerações Finais

* **Validações:** Todos os campos obrigatórios com Data Annotations.
* **Auditoria:** `CreatedAt` e `UpdatedAt`.
* **Boa prática:** Repositórios retornam entidades, Services fazem mapeamento e regras de negócio.
* **CORS:** Configurado para permitir consumo da API pelo frontend.
