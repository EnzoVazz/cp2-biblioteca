# 📚 CP4 - Sistema de Biblioteca (Clean Architecture + EF Core + API REST)

## 👥 Integrantes do Grupo
* **Enzo Vaz** — RM: 561702
* **Lucas Ryuji Fukuda** — RM: 562152
* **Pietro Donella Salomão** — RM: 561722

## 🧭 Domínio do Projeto
O projeto consiste em um **Sistema de Gestão de Biblioteca** desenvolvido com foco em escalabilidade e manutenibilidade. O objetivo principal é gerenciar o acervo físico de livros, categorias (gêneros), funcionários e o fluxo de empréstimos para clientes.

## 🛠️ Tecnologias e Persistência
* **SGBD:** Oracle SQL (herdado do CP2)
* **ORM:** Entity Framework Core
* **Mapeamento:** Fluent API (chaves, restrições e relacionamentos 1:N e N:N)
* **Versionamento:** Migrations
* **API:** ASP.NET Core (`net10.0`) com Swagger/OpenAPI, health checks, logs estruturados e tratamento global de erros (RFC 7807)
* **Testes:** xUnit (`Biblioteca.Domain.Tests` e `Biblioteca.Application.Tests`)

## 🧱 Estrutura do Projeto (Clean Architecture)
A solução foi organizada seguindo os princípios da **Clean Architecture** e **DDD**, com separação clara de responsabilidades:

* **`Biblioteca.Domain`**: entidades ricas, regras de negócio e exceções de domínio (`DomainException`, `ResourceNotFoundException`, `ConflictException`).
* **`Biblioteca.Application`**: contratos (`IRepository<T>`), DTOs e serviços de caso de uso (`ILivroAppService`). A API **não** expõe entidades de domínio.
* **`Biblioteca.Infrastructure`**: persistência.
  * `/Persistence`: `BibliotecaContext` e implementação do **repositório genérico** (`Repository<T>`).
  * `/Persistence/Configurations`: mapeamentos Fluent API.
  * `/Migrations`: histórico estrutural do banco (CP2, intacto).
* **`Biblioteca.API`**: composição (DI), controllers HTTP, Swagger, health checks (`GET /health`) e `GlobalExceptionHandler`. **Não** injeta `DbContext` nos controllers.
* **`Biblioteca.Domain.Tests`**: testes de domínio sem mock (referencia **somente** Domain).
* **`Biblioteca.Application.Tests`**: testes de aplicação com Moq de `IRepository<T>` (não sobe API nem banco).

## 🗄️ Validação do Cenário (Esquema Físico — CP2)
O banco de dados foi gerado com sucesso no Oracle SGBD. Abaixo, a evidência do esquema físico validando a criação das tabelas e das associativas geradas pelo EF Core:

![Esquema do Banco de Dados](./docs/esquema-banco.jpeg)

## 🚀 Como executar a API, o Swagger e o health check
1. Configure a string de conexão no arquivo `appsettings.json` (projeto `Biblioteca.API`) substituindo as credenciais temporárias pelas suas locais.
   > ⚠️ **Nota de Segurança:** as credenciais reais foram omitidas deste repositório para evitar o vazamento de dados sensíveis.
2. Abra o terminal na pasta do projeto `Biblioteca.API`:
   ```bash
   cd src/Biblioteca/Biblioteca.API
   ```
3. Aplique as migrations (CP2) no Oracle local:
   ```bash
   dotnet ef database update --project ../Biblioteca.Infrastructure --startup-project .
   ```
4. Suba a API:
   ```bash
   dotnet run
   ```
5. URLs:
   * **Swagger UI:** [http://localhost:5267/swagger](http://localhost:5267/swagger) (HTTPS: [https://localhost:7095/swagger](https://localhost:7095/swagger))
   * **OpenAPI:** `/swagger/v1/swagger.json`
   * **Health:** [http://localhost:5267/health](http://localhost:5267/health)

`/health` **não** aparece no Swagger (não é recurso de negócio).

## ❤️ Health checks (`GET /health`)
Registro via extensão `AddBibliotecaHealthChecks`. Relatório JSON com status geral, duração e lista de checks. HTTP: **Healthy/Degraded → 200**; **Unhealthy → 503**. Detalhe de exceção só em Development.

| Check | O que verifica | Falha |
|-------|----------------|-------|
| `self` | Processo da API no ar | — |
| `oracle` | Banco Oracle via `AddDbContextCheck<BibliotecaContext>` | **Unhealthy** (derruba o relatório → **503**) |
| `fiap` *(extra)* | `GET https://www.fiap.com.br` | **Degraded** (API continua 200; um `Unhealthy` aqui derrubaria `/health` inteiro) |

**Como validar 503:** com a connection string placeholder (ou Oracle parado), `GET /health` devolve Unhealthy. Evidência capturada: [`docs/health-unhealthy.json`](./docs/health-unhealthy.json). Evidência com banco FIAP ok (**HTTP 200**): [`docs/health-healthy.json`](./docs/health-healthy.json).

## 🌐 Recursos HTTP (CP3)
| Recurso | Rotas | Operações |
|---------|--------|-----------|
| Gêneros | `/api/generos` | GET lista, GET por id, POST, PUT, DELETE |
| Bibliotecas | `/api/bibliotecas` | GET lista, GET por id, POST |
| Livros | `/api/livros` | GET lista, GET por id, POST (`ILivroAppService`) |
| Clientes | `/api/clientes` | GET lista, GET por id, POST |

Os controllers usam DTOs e devolvem `Ok` / `Created` / `NoContent`. Regras de negócio continuam nas entidades; id inexistente lança `ResourceNotFoundException` (404 via handler).

### Exemplos de chamada
Criar um gênero:
```bash
curl -X POST http://localhost:5267/api/generos ^
  -H "Content-Type: application/json" ^
  -d "{\"nome\":\"Fantasia\",\"descricao\":\"Narrativas com elementos magicos e mundos imaginarios.\"}"
```

Health:
```bash
curl -i http://localhost:5267/health
```

Há também exemplos em `src/Biblioteca/Biblioteca.API/Biblioteca.http`.

## 📦 Repositório genérico
Contrato `IRepository<T>` (Application), com `T : BaseEntity`: `GetAllAsync`, `GetByIdAsync`, `AddAsync`, `UpdateAsync`, `DeleteAsync`, `ExistsByIdAsync`. Implementação EF em `Repository<T>` com `AsNoTracking` nas leituras.

Registro na DI:
```csharp
services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
```

## 📝 Logs e correlação (`traceId`)
Logs estruturados com `ILogger<T>` e propriedades nomeadas, incluindo `{TraceId}` (`HttpContext.TraceIdentifier`):

* **POST `/api/generos`** e **POST `/api/livros`**: log de **início** e de **sucesso**.
* **`GlobalExceptionHandler`**: log **Error** com o mesmo `traceId`, path e tipo da exceção. A resposta HTTP **não** devolve stack trace fora de Development; o detalhe fica no log. Em Development, `traceId` também vai em `ProblemDetails.Extensions`.

Trecho real (POST inválido, mesmo `traceId` no controller e no handler): [`docs/log-traceid.txt`](./docs/log-traceid.txt).

## ⚠️ Mapeamento de exceções (`GlobalExceptionHandler`)
Handler na API implementando `IExceptionHandler`. Respostas no padrão **RFC 7807** (`ProblemDetails`, `application/problem+json`).

| Exceção | Status HTTP | Título típico |
|---------|-------------|---------------|
| `ArgumentException` | **400** | Requisição inválida |
| `DomainException` (regra de negócio) | **400** | Regra de negócio violada |
| `ResourceNotFoundException` / `KeyNotFoundException` | **404** | Recurso não encontrado |
| `ConflictException` | **409** | Conflito (ex.: nome de gênero ou e-mail duplicado) |
| Demais exceções | **500** | Erro interno (mensagem genérica fora de Development) |

Exemplo anonimizado de 404: [`docs/problem-details-404.json`](./docs/problem-details-404.json).
Exemplo anonimizado de 400: [`docs/problem-details-400.json`](./docs/problem-details-400.json).
Print do Swagger UI: [`docs/swagger-ui.png`](./docs/swagger-ui.png).

## ✅ Testes (`dotnet test`)
A partir da raiz do repositório ou da pasta da solution:

```bash
dotnet test src/Biblioteca/Biblioteca.sln
```

| Projeto | O que cobre |
|---------|-------------|
| `Biblioteca.Domain.Tests` | Sem mock. `Genero` e `Livro`: `[Fact]` no caminho feliz e `[Theory]` + `[InlineData]` nas regras (descrição/nome, páginas). |
| `Biblioteca.Application.Tests` | Moq de `IRepository<T>`. Biblioteca inexistente → `ResourceNotFoundException` e `Add` **nunca** chamado (`Times.Never`); caminho feliz persiste uma vez (`Times.Once`). |

Saída capturada: [`docs/dotnet-test.txt`](./docs/dotnet-test.txt) (14 testes aprovados).
