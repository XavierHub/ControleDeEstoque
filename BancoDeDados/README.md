### Descrição das Tabelas e Relacionamentos

#### Tabela: `Product`
- **Descrição**: Armazena informações básicas sobre os produtos.
- **Campos**:
  - `Id`: Identificador único do produto (chave primária).
  - `PartNumber`: Código do produto.
  - `Name`: Nome do produto.

#### Tabela: `Stock`
- **Descrição**: Armazena informações sobre o estoque de cada produto.
- **Campos**:
  - `Id`: Identificador único do registro de estoque (chave primária).
  - `ProductId`: Identificador do produto (chave estrangeira referenciando `Product(Id)`).
  - `Quantity`: Quantidade disponível no estoque.
  - `UnitPrice`: Preço por unidade do produto na última compra.
  - `AveragePrice`: Preço médio por unidade do produto no estoque.
  - `Total`: Valor total do estoque para o produto.
- **Relacionamento**:
  - Muitos para um com a tabela `Product`.

#### Tabela: `Consumption`
- **Descrição**: Armazena informações sobre o consumo diário de cada produto, utilizado para gerar relatórios.
- **Campos**:
  - `Id`: Identificador único do registro de consumo (chave primária).
  - `ProductId`: Identificador do produto consumido (chave estrangeira referenciando `Product(Id)`).
  - `QuantityConsumed`: Quantidade consumida do produto.
  - `ConsumptionDate`: Data do consumo.
  - `TotalAveragePrice`: Preço médio total das unidades consumidas.
  - `TotalCost`: Custo total das unidades consumidas.
- **Relacionamento**:
  - Muitos para um com a tabela `Product`.

#### Tabela: `Logging`
- **Descrição**: Armazena logs de erros e eventos do sistema.
- **Campos**:
  - `Id`: Identificador único do registro de log (chave primária).
  - `Timestamp`: Data e hora em que o evento de log ocorreu.
  - `Level`: Nível de severidade do log (ex.: `Information`, `Warning`, `Error`, `Fatal`).
  - `Template`: Modelo ou formato da mensagem de log, incluindo placeholders que são substituídos por valores reais.
  - `Message`: Mensagem de log completa, incluindo valores substituídos nos placeholders.
  - `Exception`: Detalhes da exceção (se houver) que causou o evento de log.
  - `Properties`: Propriedades adicionais associadas ao evento de log, armazenadas em formato JSON ou semelhante.
  - `_ts`: Timestamp do banco de dados, indicando quando o registro foi inserido.

### Relacionamentos
- A tabela `Product` tem um relacionamento de um-para-muitos com as tabelas `Stock` e `Consumption`.
- A tabela `Stock` armazena o estoque atual de cada produto.
- A tabela `Consumption` armazena os registros diários de consumo de cada produto, permitindo um registro por dia e por produto, usado para gerar relatórios de consumo diário.
- A tabela `Logging` armazena os eventos de log gerados pelo sistema, fornecendo detalhes como data, nível de severidade, mensagem e exceções associadas.