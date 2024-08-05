### Passo a Passo para Executar o Sistema

#### Pré-requisitos
- Instalar o [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Instalar o [MySQL Server](https://dev.mysql.com/downloads/mysql/) (versão 8.0 ou superior)
- Instalar o [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) ou outra ferramenta de gerenciamento de banco de dados MySQL

#### Passo 1: Clonar o Repositório
Clone o repositório do projeto para sua máquina local:
```sh
git clone <URL-do-repositório>
```

#### Passo 2: Configurar o Banco de Dados
1. Abra o MySQL Workbench ou outra ferramenta de gerenciamento de banco de dados MySQL.
2. Crie um novo banco de dados chamado `dbinventory`.
3. Execute o script SQL localizado em `05 Database/Script/banco.sql` para criar as tabelas necessárias.
   - No MySQL Workbench, isso pode ser feito selecionando o menu `File > Open SQL Script`, navegando até o arquivo `banco.sql` e clicando em `Open`.
   - Execute o script clicando no ícone `Execute` (um raio) ou pressionando `Ctrl+Shift+Enter`.

#### Passo 3: Configurar a String de Conexão
1. Abra o arquivo de configuração `appsettings.json` do projeto.
2. Atualize a string de conexão com as informações do seu banco de dados MySQL:
```json
"ConnectionStrings": {
  "cnDbInventory": "Server=localhost;Database=dbinventory;User=root;Password=SuaSenha;Port=3306"
}
```

#### Passo 4: Restaurar Dependências
1. Navegue até a pasta do projeto no terminal.
2. Execute o comando abaixo para restaurar todas as dependências do projeto:
```sh
dotnet restore
```

#### Passo 5: Compilar o Projeto
1. Compile o projeto executando o comando abaixo:
```sh
dotnet build
```

#### Passo 6: Executar o Projeto
1. Execute o projeto com o comando:
```sh
dotnet run
```

2. Após a execução do comando, a aplicação estará disponível em `https://localhost:5001` (ou outra porta configurada).

#### Passo 7: Acessar a Documentação da API
1. Acesse a documentação da API no navegador, disponível em:
```
https://localhost:5001/swagger/index.html
```

### Resumo das Etapas
1. Clonar o repositório.
2. Configurar o banco de dados MySQL.
3. Atualizar a string de conexão no `appsettings.json`.
4. Restaurar dependências do projeto.
5. Compilar o projeto.
6. Executar o projeto.
7. Acessar a documentação da API via Swagger.

Seguindo estes passos, você terá o sistema configurado e em execução em sua máquina local.