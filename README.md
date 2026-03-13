# MedGrupo Teste

API REST para gerenciamento de contatos, desenvolvida em .NET 8 com foco em organizacao por camadas, regras de negocio isoladas e testes automatizados.

## Objetivo

O projeto atende ao desafio de criar um sistema para:

- cadastrar contatos
- listar contatos
- visualizar detalhes de um contato
- atualizar contato
- desativar contato
- excluir contato

Regras de negocio implementadas:

- nome obrigatorio
- data de nascimento nao pode ser maior que a data atual
- idade calculada em tempo de execucao
- contato deve ser maior de idade
- listagem e visualizacao por id consideram contatos ativos
- atualizacao parcial: apenas os campos enviados sao alterados

## Arquitetura

A solucao foi organizada seguindo separacao de responsabilidades inspirada em Onion Architecture:

- `MedGrupo-teste.Domain`: entidades e regras de negocio
- `MedGrupo-teste.Application`: servicos e DTOs da aplicacao
- `MedGrupo-teste.Infraestructure`: persistencia com EF Core e repositorios
- `MedGrupo-teste.Api`: endpoints REST
- `MedGrupo-teste.Teste`: testes automatizados

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- xUnit
- Swagger

## Estrutura principal

- [Contato.cs](./MedGrupo-teste.Domain/Entities/Contato.cs)
- [ServicoContato.cs](./MedGrupo-teste.Application/Services/ServicoContato.cs)
- [RepositorioContato.cs](./MedGrupo-teste.Infraestructure/Repositories/RepositorioContato.cs)
- [BaseRepository.cs](./MedGrupo-teste.Infraestructure/Repositories/BaseRepository.cs)
- [ContatosController.cs](./MedGrupo-teste.Api/Controllers/ContatosController.cs)

## Configuracao

A connection string atual esta em:

- [appsettings.json](./MedGrupo-teste.Api/appsettings.json)

Valor atual:

```json
"DefaultConnection": "Server=note-wartelon;Database=MedGrupo-teste;Trusted_Connection=True;TrustServerCertificate=True;"
```

Antes de executar em outra maquina, ajuste esse valor para uma instancia valida do SQL Server.

Exemplo com LocalDB:

```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MedGrupo-teste;Trusted_Connection=True;TrustServerCertificate=True;"
```

## Como executar

1. Restaurar os pacotes:

```bash
dotnet restore MedGrupo-teste.sln
```

2. Executar a API:

```bash
dotnet run --project .\MedGrupo-teste.Api\MedGrupo-teste.Api.csproj
```

3. Acessar o Swagger:

```text
https://localhost:<porta>/swagger
```

Observacao:

- a base e criada automaticamente no startup com `Database.EnsureCreated()`

## Endpoints

### Criar contato

`POST /api/contatos`

Exemplo:

```json
{
  "nome": "Maria Silva",
  "dataNascimento": "1995-08-10",
  "sexo": "Feminino"
}
```

### Listar contatos com filtros e paginacao

`GET /api/contatos`

Query params suportados:

- `nome`
- `sexo`
- `somenteAtivos`
- `pagina`
- `tamanhoPagina`

Exemplo:

```text
GET /api/contatos?nome=mari&sexo=Feminino&somenteAtivos=false&pagina=1&tamanhoPagina=10
```

Retorno:

```json
{
  "itens": [
    {
      "id": "00000000-0000-0000-0000-000000000000",
      "nome": "Maria Silva",
      "dataNascimento": "1995-08-10",
      "sexo": "Feminino",
      "idade": 30,
      "estaAtivo": true
    }
  ],
  "pagina": 1,
  "tamanhoPagina": 10,
  "totalItens": 1,
  "totalPaginas": 1
}
```

### Obter contato por id

`GET /api/contatos/{id}`

Considera apenas contatos ativos.

### Atualizar contato parcialmente

`PUT /api/contatos/{id}`

ou

`PATCH /api/contatos/{id}`

Os campos sao anulaveis. Apenas os campos enviados sao alterados.

Exemplo enviando apenas nome:

```json
{
  "nome": "Maria Oliveira"
}
```

Exemplo enviando nome e sexo:

```json
{
  "nome": "Maria Oliveira",
  "sexo": "Feminino"
}
```

### Desativar contato

`PATCH /api/contatos/{id}/deactivate`

### Excluir contato

`DELETE /api/contatos/{id}`

## Filtros dinamicos implementados

Na listagem foram implementados:

- filtro por sexo
- busca parcial por nome com `LIKE`
- opcao para listar somente ativos ou todos
- paginacao com `pagina` e `tamanhoPagina`

## Testes

Executar todos os testes:

```bash
dotnet test .\MedGrupo-teste.Teste\MedGrupo-teste.Teste.csproj
```

Coberturas principais:

- validacao de maioridade
- validacao de data de nascimento
- desativacao de contato
- filtros dinamicos
- paginacao
- atualizacao parcial

## Pontos de destaque

- regras de negocio centralizadas na entidade `Contato`
- endpoint de atualizacao parcial com parametros anulaveis
- repositorio base generico com interface `IBaseRepositorio<TEntity>`
- repositorio especifico com filtros dinamicos
- testes automatizados cobrindo fluxo de negocio e aplicacao

## Arquivo HTTP de apoio

Para testes manuais rapidos:

- [MedGrupo-teste.Api.http](./MedGrupo-teste.Api/MedGrupo-teste.Api.http)

## Autor

Projeto desenvolvido como teste tecnico para processo seletivo.
