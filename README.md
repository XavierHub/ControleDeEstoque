# InventoryControlApp

## Visão Geral

InventoryControlApp é um projeto de API web em .NET 8, projetado para gerenciar o estoque de peças. O sistema alerta os usuários sobre estoque insuficiente ou peças inexistentes, impede o consumo excessivo e fornece relatórios diários de consumo com custos associados com base no preço médio de custo.

### Casos de Uso
[Especificação dos casos de uso e Testes](CasosDeUso/README.md)

## Tecnologias Utilizadas

### Backend

- **.NET 8**: Framework principal usado para construir a API web.
- **C#**: Linguagem de programação principal.
- **ASP.NET Core**: Framework para construção da API web.
- **Dapper**: Micro ORM utilizado para acesso eficiente aos dados.
- **Dapper.Dommel**: Extensão do Dapper que simplifica operações com métodos de extensão.
- **MySQL**: Sistema de gerenciamento de banco de dados relacional utilizado para persistência de dados.
- **MediatR**: Biblioteca para implementação do CQRS (Command Query Responsibility Segregation).
- **AutoMapper**: Mapeamento de objetos para transformar modelos em DTOs e vice-versa.

### Testes

- **xUnit**: Framework de testes para escrita de testes unitários.
- **Moq**: Biblioteca para criação de objetos mock para testes.
- **Moq.Dapper**: Biblioteca para mockar métodos de extensão do Dapper.

### Logging e Monitoramento

- **Serilog**: Biblioteca de logging utilizada para logging estruturado.
- **Serilog.Sinks.MySQL**: Sink do Serilog para logging em um banco de dados MySQL.
- **Application Insights**: Monitoramento e gerenciamento de performance.

### Injeção de Dependência

- **Microsoft.Extensions.DependencyInjection**: Framework de injeção de dependência embutido utilizado para gerenciar dependências.

### Middleware

- **Custom Middleware**: Para tratamento global de exceções e logging apropriado.

## Padrões de Design Utilizados

- **Domain Driven Design (DDD)**: Abordagem de modelagem de software centrada no domínio.
- **Domain Notifications**: Padrão para gestão e propagação de notificações de domínio.
- **Inversion of Control (IoC)**: Princípio de design onde o controle do fluxo do programa é invertido.
- **Repository Pattern**: Abstração da lógica de acesso a dados, utilizando Dapper.
- **Options Pattern**: Padrão para gerenciamento de configurações fortemente tipadas no .NET.
- **Chain of Responsibility**: Implementado com Middleware do .NET 8 para tratamento de solicitações.

## Estrutura do Projeto

### Camadas

- **Controllers**: Manipulação de requisições e respostas HTTP.
- **Commands**: Implementação dos comandos CQRS para operações de escrita.
- **Queries**: Implementação das consultas CQRS para operações de leitura.
- **Services**: Serviços de domínio e lógica de negócios.
- **Repositories**: Abstração de acesso aos dados utilizando Dapper e Dommel.
- **Models**: Definições de entidades e modelos de dados.
  
### Banco de Dados
[Diagrama do banco de dados](BancoDeDados/README.md)

## Contribuindo

Sinta-se à vontade para contribuir com este projeto fazendo um fork, criando um branch, adicionando suas alterações e enviando um pull request.

## Licença

Este projeto está licenciado sob a [Licença MIT](https://opensource.org/licenses/MIT).

---

**Desenvolvido por Felipe Xavier**
