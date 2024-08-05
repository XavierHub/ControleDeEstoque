## Casos de Uso

### 1. Cadastrar Produto

**Descrição:** O usuário cadastra um novo produto no sistema.

**Fluxo Principal:**
1. O usuário solicita a página de cadastro de produto.
2. O sistema exibe o formulário de cadastro.
3. O usuário preenche os campos obrigatórios (Nome, PartNumber).
4. O usuário submete o formulário.
5. O sistema valida os dados e verifica se o PartNumber já existe.
6. O sistema salva o novo produto.
7. O sistema retorna uma confirmação de sucesso.

**Fluxos Alternativos:**
- 5a. Se o PartNumber já existe:
  1. O sistema exibe uma mensagem de erro.
  2. O usuário altera o PartNumber e reenvia o formulário.

### 2. Atualizar Produto

**Descrição:** O usuário atualiza os dados de um produto existente.

**Fluxo Principal:**
1. O usuário solicita a página de atualização de produto.
2. O sistema exibe o formulário com os dados atuais do produto.
3. O usuário altera os dados desejados.
4. O usuário submete o formulário.
5. O sistema valida os dados.
6. O sistema verifica se o novo PartNumber já existe.
7. O sistema atualiza os dados do produto.
8. O sistema retorna uma confirmação de sucesso.

**Fluxos Alternativos:**
- 6a. Se o novo PartNumber já existe:
  1. O sistema exibe uma mensagem de erro.
  2. O usuário altera o PartNumber e reenvia o formulário.

### 3. Excluir Produto

**Descrição:** O usuário exclui um produto do sistema.

**Fluxo Principal:**
1. O usuário solicita a exclusão de um produto.
2. O sistema verifica se o produto possui estoque.
3. O sistema exclui o produto.
4. O sistema retorna uma confirmação de sucesso.

**Fluxos Alternativos:**
- 2a. Se o produto possui estoque:
  1. O sistema exibe uma mensagem de erro indicando que não é possível excluir produtos com estoque.
  
### 4. Adicionar Estoque

**Descrição:** O usuário adiciona uma quantidade de estoque para um produto.

**Fluxo Principal:**
1. O usuário solicita a página de adição de estoque.
2. O sistema exibe o formulário de adição de estoque.
3. O usuário preenche a quantidade e o preço por unidade.
4. O usuário submete o formulário.
5. O sistema valida os dados.
6. O sistema atualiza a quantidade de estoque do produto.
7. O sistema recalcula o preço médio do produto.
8. O sistema retorna uma confirmação de sucesso.

**Fluxos Alternativos:**
- 3a. Se a quantidade é negativa:
  1. O sistema exibe uma mensagem de erro.
  2. O usuário corrige a quantidade e reenvia o formulário.

### 5. Remover Estoque

**Descrição:** O usuário remove uma quantidade de estoque de um produto.

**Fluxo Principal:**
1. O usuário solicita a página de remoção de estoque.
2. O sistema exibe o formulário de remoção de estoque.
3. O usuário preenche a quantidade a ser removida.
4. O usuário submete o formulário.
5. O sistema valida os dados.
6. O sistema verifica se há estoque suficiente.
7. O sistema atualiza a quantidade de estoque do produto.
8. O sistema registra a remoção no relatório de consumo.
9. O sistema retorna uma confirmação de sucesso.

**Fluxos Alternativos:**
- 6a. Se não há estoque suficiente:
  1. O sistema exibe uma mensagem de erro.
  2. O usuário corrige a quantidade e reenvia o formulário.

### 6. Consultar Estoque de Produto

**Descrição:** O usuário consulta a quantidade de estoque de um produto.

**Fluxo Principal:**
1. O usuário solicita a página de consulta de estoque de um produto.
2. O sistema exibe a quantidade de estoque atual do produto.

**Fluxos Alternativos:**
- Não há fluxos alternativos.

### 7. Consultar Consumo Diário

**Descrição:** O usuário consulta o consumo diário de um produto específico.

**Fluxo Principal:**
1. O usuário solicita a página de consulta de consumo diário de um produto.
2. O sistema exibe o formulário para selecionar a data e o produto.
3. O usuário seleciona o produto e a data.
4. O usuário submete o formulário.
5. O sistema valida os dados.
6. O sistema exibe o relatório de consumo diário para o produto selecionado.

**Fluxos Alternativos:**
- 3a. Se a data não for selecionada:
  1. O sistema utiliza a data atual.

## Casos de Teste

### 1. Cadastrar Produto

**Cenários de Teste:**
- Cadastro bem-sucedido de um novo produto.
- Tentativa de cadastrar um produto com PartNumber já existente.

### 2. Atualizar Produto

**Cenários de Teste:**
- Atualização bem-sucedida de um produto.
- Tentativa de atualizar um produto inexistente.
- Tentativa de atualizar um produto para um PartNumber já existente.

### 3. Excluir Produto

**Cenários de Teste:**
- Exclusão bem-sucedida de um produto.
- Tentativa de excluir um produto com estoque remanescente.
- Tentativa de excluir um produto inexistente.

### 4. Adicionar Estoque

**Cenários de Teste:**
- Adição bem-sucedida de estoque a um produto.
- Tentativa de adicionar estoque com quantidade negativa.

### 5. Remover Estoque

**Cenários de Teste:**
- Remoção bem-sucedida de estoque de um produto.
- Tentativa de remover mais estoque do que disponível.
- Tentativa de remover estoque de um produto inexistente.

### 6. Consultar Estoque de Produto

**Cenários de Teste:**
- Consulta bem-sucedida de estoque de um produto existente.
- Tentativa de consultar estoque de um produto inexistente.

### 7. Consultar Consumo Diário

**Cenários de Teste:**
- Consulta bem-sucedida de consumo diário de um produto para uma data específica.
- Consulta bem-sucedida de consumo diário de um produto para a data atual.
- Tentativa de consultar consumo diário de um produto inexistente.

Esses casos de uso e fluxos alternativos cobrem as funcionalidades essenciais da aplicação, garantindo que as principais interações do usuário com o sistema sejam contempladas.
