# 🚗 Fleet Control - Sistema de Controle de Frotas

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)]()
[![Tests](https://img.shields.io/badge/tests-39%2F39%20passing-brightgreen)]()
[![License](https://img.shields.io/badge/license-MIT-blue)]()
[![.NET](https://img.shields.io/badge/.NET-10-purple)]()
[![C#](https://img.shields.io/badge/C%23-14.0-green)]()

Aplicação profissional de controle de frotas desenvolvida em **C# .NET 10** com **Clean Architecture**, **DDD** e **CQRS**. Sistema completo para gerenciar condutores, veículos e viagens com validação, logging e tratamento de erros padronizado.

## 📋 Índice

- [Características](#características)
- [Arquitetura](#arquitetura)
- [Tecnologias](#tecnologias)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Como Executar](#como-executar)
- [Testes](#testes)
- [Endpoints da API](#endpoints-da-api)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Documentação](#documentação)
- [Contribuindo](#contribuindo)

---

## ✨ Características

### ✅ Funcionalidades Implementadas

- **Gerenciar Condutores**
  - Criar, atualizar, listar e deletar condutores
  - Validação de CPF e CNH
  - Controle de status (Ativo/Inativo)

- **Gerenciar Veículos**
  - Criar, atualizar, listar e deletar veículos
  - Validação de Placa, RENAVAM e Chassi
  - Rastreamento de proprietário e localização

- **Gerenciar Viagens** 🆕
  - Criar, iniciar, concluir, pausar e retomar viagens
  - Estado machine com 5 estados (Planejada, EmAndamento, Pausada, Concluida, Cancelada)
  - Validações de negócio (condutor/veículo sem viagem ativa)
  - Rastreamento de quilometragem e localização
  - Filtros por condutor ou veículo

### 🛡️ Recursos de Infraestrutura

- **Validação Global** com FluentValidation
- **Tratamento de Erros** com RFC 7807 (ProblemDetails)
- **Logging Estruturado** com Serilog
- **Middleware de Rastreamento** de requisições
- **Testes Unitários** com xUnit e Moq
- **Factory Pattern** para dados de teste

---

## 🏗️ Arquitetura

```
Clean Architecture (4 Camadas)

┌─────────────────────────────────────┐
│        API Layer (Controllers)      │  ← HTTP Endpoints
│      (Fleet.Api)                     │
└──────────────────┬──────────────────┘
                   │ depends on
┌─────────────────────────────────────┐
│   Application Layer (Use Cases)     │  ← Commands, Queries, Handlers
│      (Fleet.Application)             │
└──────────────────┬──────────────────┘
                   │ depends on
┌─────────────────────────────────────┐
│      Domain Layer (Entities)        │  ← Business Logic
│      (Fleet.Domain)                  │
└──────────────────┬──────────────────┘
                   │ implements
┌─────────────────────────────────────┐
│ Infrastructure Layer (Data Access)  │  ← EF Core, Repositories
│    (Fleet.Infrastructure)            │
└─────────────────────────────────────┘
```

### Padrões Implementados

- **CQRS** - Command Query Responsibility Segregation com MediatR
- **DDD** - Domain-Driven Design com Entities e Value Objects
- **Repository Pattern** - Abstração de acesso a dados
- **Dependency Injection** - Inversão de controle
- **Fluent Validation** - Validação declarativa
- **Global Exception Handler** - Tratamento centralizado de erros

---

## 🛠️ Tecnologias

| Componente | Versão | Descrição |
|-----------|--------|-----------|
| **.NET** | 10 | Framework principal |
| **C#** | 14.0 | Linguagem de programação |
| **Entity Framework Core** | 10.0.3 | ORM para acesso a dados |
| **MediatR** | 12.4.1 | Implementação de CQRS |
| **FluentValidation** | 11.11.0 | Validação de regras de negócio |
| **Serilog** | 8.1.0 | Logging estruturado |
| **SQL Server** | 2022+ | Banco de dados |
| **xUnit** | 2.9.3 | Framework de testes |
| **Moq** | 4.20.72 | Mocking para testes |
| **AutoFixture** | 4.18.1 | Geração de dados para testes |
| **FluentAssertions** | 6.12.1 | Assertions fluentes |

---

## 📦 Pré-requisitos

- **Visual Studio Community 2026+** ou VS Code
- **.NET 10 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/10.0))
- **SQL Server 2022+** (LocalDB ou completo)
- **Git**

### Verificar instalação

```powershell
# Verificar versão do .NET
dotnet --version

# Verificar SQL Server
sqlcmd -S (local) -m "SELECT @@VERSION"
```

---

## 📥 Instalação

### 1. Clonar o Repositório

```bash
git clone https://github.com/DuCrato/controle-frotas.git
cd controle-frotas
```

### 2. Restaurar Dependências

```bash
dotnet restore
```

### 3. Configurar Banco de Dados

#### a) Atualizar Connection String

Editar `src/Fleet.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Server=(localdb)\\mssqllocaldb;Database=FleetControlDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

#### b) Aplicar Migrations

```bash
# Navegar para pasta da Infrastructure
cd src/Fleet.Infrastructure

# Aplicar migrations ao banco
dotnet ef database update

# Voltar ao diretório raiz
cd ../..
```

### 4. Compilar o Projeto

```bash
dotnet build
```

---

## 🚀 Como Executar

### Via Visual Studio

1. Abrir `controle-frotas.sln` no Visual Studio
2. Definir `Fleet.Api` como projeto de inicialização
3. Pressionar `F5` ou `Debug` → `Start Debugging`
4. A aplicação abrirá em `https://localhost:5001`

### Via CLI

```bash
cd src/Fleet.Api
dotnet run
```

### Acessar a API

- **Swagger UI**: https://localhost:5001/swagger
- **Health Check**: https://localhost:5001/health

---

## ✅ Testes

### Executar Todos os Testes

```bash
# Via CLI
dotnet test

# Via Visual Studio
Test → Run All Tests (Ctrl + R, A)
```

### Executar Testes de um Projeto Específico

```bash
dotnet test tests/Fleet.Application.Tests/Fleet.Application.Tests.csproj
```

### Executar Testes de uma Classe Específica

```bash
dotnet test --filter "ClassName=Fleet.Application.Tests.Viagens.CriarViagemHandlerTests"
```

### Cobertura de Testes

```bash
# Instalar tool
dotnet tool install -g coverlet.console

# Executar com cobertura
coverlet tests/Fleet.Application.Tests/bin/Debug/net10.0/Fleet.Application.Tests.dll --target "dotnet" --targetargs "test tests/Fleet.Application.Tests --no-build" --format opencover
```

**Status Atual:**
- ✅ 39 testes implementados
- ✅ 39 testes passando
- ✅ 0 testes falhando

---

## 📡 Endpoints da API

### 🧑 Condutores

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/condutores` | Criar novo condutor |
| `PUT` | `/api/condutores/{id}` | Atualizar condutor |
| `DELETE` | `/api/condutores/{id}` | Deletar condutor |
| `GET` | `/api/condutores/{id}` | Obter condutor por ID |
| `GET` | `/api/condutores` | Listar todos os condutores |

### 🚙 Veículos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/veiculos` | Criar novo veículo |
| `PUT` | `/api/veiculos/{id}` | Atualizar veículo |
| `DELETE` | `/api/veiculos/{id}` | Deletar veículo |
| `GET` | `/api/veiculos/{id}` | Obter veículo por ID |
| `GET` | `/api/veiculos` | Listar todos os veículos |

### 🚗 Viagens

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/viagens` | Criar nova viagem |
| `POST` | `/api/viagens/{id}/iniciar` | Iniciar viagem |
| `POST` | `/api/viagens/{id}/concluir` | Concluir viagem |
| `POST` | `/api/viagens/{id}/pausar` | Pausar viagem |
| `POST` | `/api/viagens/{id}/retomar` | Retomar viagem pausada |
| `POST` | `/api/viagens/{id}/cancelar` | Cancelar viagem |
| `GET` | `/api/viagens/{id}` | Obter viagem por ID |
| `GET` | `/api/viagens` | Listar todas as viagens |
| `GET` | `/api/viagens/condutor/{condutorId}` | Viagens por condutor |
| `GET` | `/api/viagens/veiculo/{veiculoId}` | Viagens por veículo |

---

## 📂 Estrutura do Projeto

```
controle-frotas/
├── src/
│   ├── Fleet.Domain/                    # Camada de Domínio (Lógica de Negócio)
│   │   ├── Condutores/
│   │   │   ├── Entidades/
│   │   │   ├── Enum/
│   │   │   └── ValueObjects/
│   │   ├── Veiculos/
│   │   │   ├── Entidades/
│   │   │   ├── Enum/
│   │   │   └── ValueObjects/
│   │   └── Viagens/                     # 🆕 Feature Viagens
│   │       ├── Entidades/
│   │       ├── Enum/
│   │       └── ValueObjects/
│   │
│   ├── Fleet.Application/               # Camada de Aplicação (Use Cases)
│   │   ├── Condutores/
│   │   │   ├── Command/
│   │   │   ├── Query/
│   │   │   ├── Handler/
│   │   │   └── Interface/
│   │   ├── Veiculos/
│   │   │   ├── Command/
│   │   │   ├── Query/
│   │   │   ├── Handler/
│   │   │   └── Interface/
│   │   └── Viagens/                     # 🆕 Feature Viagens
│   │       ├── Command/
│   │       ├── Query/
│   │       ├── Handler/
│   │       └── Interface/
│   │
│   ├── Fleet.Infrastructure/            # Camada de Infraestrutura (Data Access)
│   │   ├── Context/
│   │   ├── Repositories/
│   │   ├── Mapping/
│   │   ├── Migrations/
│   │   └── DependencyInjection.cs
│   │
│   └── Fleet.Api/                       # Camada de Apresentação (HTTP)
│       ├── Controllers/
│       ├── Infrastructure/
│       │   ├── GlobalExceptionHandler.cs
│       │   └── RequestLoggingMiddleware.cs
│       ├── Validators/
│       ├── appsettings.json
│       └── Program.cs
│
├── tests/
│   └── Fleet.Application.Tests/         # Testes Unitários
│       ├── Condutores/
│       ├── Veiculos/
│       ├── Viagens/                     # 🆕 Feature Viagens
│       └── Common/
│
├── VIAGENS.md                           # Documentação da Feature Viagens
├── VIAGENS_IMPLEMENTACAO_COMPLETA.md   # Detalhes de Implementação
├── VALIDATION.md                         # Documentação de Validação
├── MIDDLEWARE.md                         # Documentação de Middleware
└── README.md                             # Este arquivo
```

---

## 📚 Documentação

### Arquivos de Documentação

| Documento | Descrição |
|-----------|-----------|
| **README.md** | Este arquivo - Visão geral e setup |

### Exemplo de Uso - Criar Viagem

```bash
curl -X POST https://localhost:5001/api/viagens \
  -H "Content-Type: application/json" \
  -d '{
    "veiculoId": "550e8400-e29b-41d4-a716-446655440000",
    "condutorId": "550e8400-e29b-41d4-a716-446655440001",
    "latitudeOrigem": -23.5505,
    "longitudeOrigem": -46.6333,
    "enderecoOrigem": "Rua A, 100, São Paulo, SP",
    "latitudeDestino": -23.6509,
    "longitudeDestino": -46.6500,
    "enderecoDestino": "Avenida B, 500, Santos, SP",
    "dataHoraPrevistaSaida": "2026-03-24T10:00:00Z",
    "dataHoraPrevistaChegada": "2026-03-24T12:00:00Z",
    "distanciaEstimada": 75.5,
    "observacoes": "Carga frágil"
  }'
```

### Exemplo de Resposta Sucesso (201)

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440002"
}
```

### Exemplo de Resposta Erro (400)

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Erro de Validação",
  "status": 400,
  "detail": "One or more validation failures have occurred.",
  "instance": "/api/viagens",
  "traceId": "0HN1GKFD0J8L2:00000001",
  "errors": {
    "LatitudeOrigem": [
      "Latitude deve estar entre -90 e 90"
    ],
    "DistanciaEstimada": [
      "Distância estimada deve ser maior que zero"
    ]
  }
}
```

---

## 🔍 Padrões de Resposta

### Sucesso (2xx)

```json
{
  "status": 200,
  "data": { ... }
}
```

### Erro RFC 7807 (4xx/5xx)

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Descrição do erro",
  "status": 400,
  "detail": "Mensagem detalhada do erro",
  "instance": "/api/endpoint",
  "traceId": "...",
  "errors": { ... } // Apenas para ValidationException
}
```

---

## 🧪 Dados de Teste

O projeto inclui `TestFactory` classes para gerar dados de teste:

```csharp
// Criar um condutor
var condutor = CondutorTestFactory.Criar();

// Criar uma viagem aleatória
var viagem = ViagemTestFactory.CriarAleatorio();

// Criar múltiplas viagens
var viagens = ViagemTestFactory.CriarLista(5);

// Criar viagens de um condutor específico
var conduporId = Guid.NewGuid();
var viagensPorCondutor = ViagemTestFactory.CriarListaPorCondutor(condutorId, 3);
```

---

## 🔄 Fluxo de Estado - Viagem

```
┌─────────────┐
│  Planejada  │ ← Estado inicial
└──────┬──────┘
       │
       ├─→ Iniciar() ──→ ┌─────────────┐
       │                 │ EmAndamento │
       │                 └─┬───────┬───┘
       │                   │       │
       │                   │       ├─→ Pausar() ──→ ┌────────┐
       │                   │       │                │ Pausada│
       │                   │       │                └──┬─────┘
       │                   │       │                   │
       │                   │       ├─→ Retomar() ◄───┘
       │                   │       │
       │                   │       └─→ Concluir() ──→ ┌──────────┐
       │                   │                         │ Concluida│
       │                   │                         └──────────┘
       │                   │
       │                   └─→ Cancelar() ──→ ┌──────────┐
       │                                      │ Cancelada│
       │                                      └──────────┘
       │
       └─→ Cancelar() ────────────────────────┐
                                              │
                                              V
                                          (Cancelada)
```

---

## 🐛 Debugging

### Logs Estruturados

Os logs são salvos em `logs/` com estrutura JSON:

```powershell
# Ver logs em tempo real
Get-Content logs/fleet-*.json -Tail 50 -Wait

# Filtrar por erro
Get-Content logs/fleet-*.json | Select-String "Error"
```

### Debugging no Visual Studio

1. Definir breakpoints (`F9`)
2. Iniciar debug (`F5`)
3. Usar Debug Console e Watch windows
4. Inspecionar HttpContext com `httpContext.Items`

---

## 📈 Performance

### Índices de Banco

- `IX_Condutores_Cpf` - Busca rápida por CPF
- `IX_Veiculos_Placa` - Busca rápida por Placa
- `IX_Viagens_VeiculoId` - Filtros por veículo
- `IX_Viagens_CondutorId` - Filtros por condutor
- `IX_Viagens_Status` - Filtros por status

### Otimizações

- ✅ `AsNoTracking()` para queries apenas leitura
- ✅ Índices nas colunas de filtro
- ✅ Logging assíncrono
- ✅ Connection pooling automático

---

## 🤝 Contribuindo

### Branch Strategy

```bash
# Feature nova
git checkout -b feature/nova-funcionalidade

# Bugfix
git checkout -b bugfix/descricao-do-bug

# Fazer commit
git commit -m "feat: descrição clara da mudança"

# Push e criar Pull Request
git push origin feature/nova-funcionalidade
```

### Padrões de Código

- Seguir [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Nomes em inglês para código, português para comentários
- Usar `var` apenas quando o tipo é óbvio
- Adicionar XML comments para públicos
- Testes devem usar padrão Given_When_Then

### Checklist Antes de Commit

- [ ] Código compila sem warnings
- [ ] Todos os testes passam
- [ ] Adicionados testes para nova funcionalidade
- [ ] Documentação atualizada
- [ ] Sem código comentado

---

## 📞 Suporte

### Problemas Comuns

**P: Erro de conexão ao banco**
```
A: Verificar connection string em appsettings.json
   Verificar se SQL Server está rodando
   Aplicar migrations: dotnet ef database update
```

**P: Testes falhando**
```
A: Limpar e restaurar: dotnet clean && dotnet restore
   Rebuild: dotnet build
   Rodar novamente: dotnet test
```

**P: Porta 5001 já em uso**
```
A: dotnet run --urls "https://localhost:5002"
   ou mudar em launchSettings.json
```

---

## 📄 Licença

Este projeto está sob licença MIT. Ver arquivo `LICENSE` para detalhes.

---

## 👨‍💼 Autor

**Desenvolvedor:** Willian Mateus  
**GitHub:** [@DuCrato](https://github.com/DuCrato)  
**Projeto:** Fleet Control - Sistema de Controle de Frotas

---

**Última Atualização:** 23 de Março de 2026  
**Versão:** 1.0.0  
**Status:** ✅ Produção
