# Anime API - Clean Architecture

Este projeto é uma API para gerenciar informações de animes, seguindo os princípios de **Clean Architecture**. Ele suporta operações CRUD com funcionalidades adicionais como filtros, deleção lógica e segurança com **API Key**. A API foi desenvolvida usando **.NET** e **PostgreSQL** como banco de dados.

## Tecnologias Utilizadas

- **.NET 8.0**
- **Entity Framework Core**
- **PostgreSQL**
- **XUnit** para testes unitários
- **Moq** para mocking nos testes
- **Swagger** para documentação de rotas

## Pré-requisitos

Antes de executar o projeto, certifique-se de ter as seguintes ferramentas instaladas:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- Um editor de texto como [Visual Studio](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)

## Como Configurar o Projeto

1. **Clone o Repositório**:

   ```bash
   git clone https://github.com/leandrosfarias/AnimeAPI-CleanArchitecture.git
   cd AnimeAPI
   ```

2. **Configurar Variáveis de Ambiente**: Crie um arquivo `.env` na raiz do projeto de AnimeAPI.API com as seguintes variáveis de ambiente para configurar a conexão com o banco de dados:

```
API_KEY=minha-chave-de-api-secreta
```

A chave de API é necessária para conseguir acessar as rotas

3. **Instalar Dependências**: Para instalar as dependências do projeto, execute o comando:

```
dotnet restore
```

4. **Configurar o Banco de Dados**:

- Verifique se o PostgreSQL está rodando na sua máquina.
- Crie o banco de dados com o nome que desejar (utilizei "Animes")

5. Configurar Conexão com o Banco de Dados

A conexão com o banco de dados PostgreSQL é configurada diretamente no arquivo `appsettings.json`. Verifique o arquivo `appsettings.json` no projeto **AnimeAPI.API** e atualize os parâmetros da string de conexão conforme necessário:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=Animes;Username=seu_usuario;Password=sua_senha"
  }
}
```

5. **Aplicar as Migrations**: Execute o seguinte comando para criar a(s) tabela(s) necessária(s) no banco de dados:

```
dotnet ef database update -p AnimeAPI.Infrastructure -s AnimeAPI.API
```

## Como Executar o Projeto

1. **Rodando a API:** Para rodar a API, execute o seguinte comando na pasta raiz do projeto:

```
dotnet run --project AnimeAPI.API
```

Você pode acessar a documentação e testar as rotas via Swagger no seguinte endereço: `http://localhost:5243/swagger/index.html`

2. **Autenticação via API Key:**

- Para acessar os endpoints protegidos, você deve fornecer uma API Key no cabeçalho x-api-key das requisições.
- Você pode usar a chave configurada no arquivo .env.

## Como Executar os Testes

1. **Rodar todos os testes:** Para rodar os testes unitários, execute o seguinte comando:

```
dotnet test
```

Isso executará todos os testes implementados no projeto.

## Observações

- O projeto segue o padrão Clean Architecture, com camadas separadas de API, Aplicação, Domínio e Infraestrutura.
- O repositório implementa injeção de dependências e usa Entity Framework Core para interagir com o banco de dados.
