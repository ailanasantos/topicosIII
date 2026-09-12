# Mini Supermercado

Sistema web completo para gerenciamento de mini supermercado, desenvolvido em ASP.NET Core MVC com Entity Framework Core e SQL Server.

## Funcionalidades

### Produtos
- Cadastro completo (nome, descrição, preço, estoque, código de barras)
- Categorias para organização
- Controle de estoque com alerta de estoque mínimo
- Ativar/desativar produtos
- Busca e ordenação na listagem

### Categorias
- CRUD completo
- Contagem de produtos por categoria

### Clientes
- Cadastro com CPF, telefone, e-mail e endereço

### Ponto de Venda (PDV)
- Tela de caixa para registrar vendas
- Seleção de produtos por busca
- Controle de quantidade e estoque
- Formas de pagamento: Dinheiro, Cartão de Crédito/Débito, PIX
- Vinculação opcional a cliente

### Vendas
- Histórico completo de vendas
- Detalhes da venda com itens
- Cancelamento com devolução automática do estoque

### Relatórios
- Vendas do dia e do mês
- Total faturado
- Produtos com estoque baixo
- Produtos mais vendidos
- Total de clientes e produtos

### Segurança
- Autenticação por cookies com BCrypt (hash de senhas)
- Proteção contra brute force (5 tentativas = bloqueio por 5 min)
- Sessão com expiração de 8 horas
- Token CSRF em todos os formulários
- Login obrigatório (todas as páginas protegidas)

## Tecnologias

- **Backend:** ASP.NET Core 10.0 MVC
- **ORM:** Entity Framework Core 10.0
- **Banco de Dados:** SQL Server
- **Frontend:** Tailwind CSS (via CDN)
- **Autenticação:** Cookie Authentication + BCrypt.Net

## Pré-requisitos

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) (local ou Docker)

## Configuração

### 1. Clone o repositório

```bash
git clone https://github.com/ailanasantos/topicosIII.git
cd topicosIII/ambiente-web
```

### 2. Configure o banco de dados

Edite `appsettings.Development.json` com sua connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TopicosWebDb;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True"
  }
}
```

### 3. Execute o projeto

```bash
dotnet run
```

> ℹ️ As migrações são aplicadas automaticamente na primeira execução.

Acesse: `https://localhost:7025`

### 4. Login

O usuário administrador é criado automaticamente na primeira execução:

- **E-mail:** `admin@minisuper.com`
- **Senha:** `M1ni#Super2026!`

> ⚠️ Altere a senha após o primeiro login.

## Estrutura do Projeto

```
ambiente-web/
├── Controllers/              # Lógica de controle
│   ├── AccountController.cs  # Login/Logout
│   ├── CategoriasController.cs
│   ├── ClientesController.cs
│   ├── HomeController.cs
│   ├── ProdutosController.cs
│   └── VendasController.cs   # PDV e Relatórios
├── Data/
│   └── AppDbContext.cs       # Contexto do Entity Framework
├── Migrations/               # Migrações do banco
├── Models/                   # Modelos de dados
│   ├── Categoria.cs
│   ├── Cliente.cs
│   ├── ItemVenda.cs
│   ├── Produto.cs
│   ├── Usuario.cs
│   └── Venda.cs
├── Views/                    # Interfaces (Razor)
│   ├── Account/              # Login
│   ├── Categorias/           # CRUD Categorias
│   ├── Clientes/             # CRUD Clientes
│   ├── Home/                 # Painel inicial
│   ├── Produtos/             # CRUD Produtos
│   ├── Vendas/               # PDV, Vendas, Relatórios
│   └── Shared/               # Layout principal (_Layout, _LayoutLogin)
├── wwwroot/                  # Arquivos estáticos (CSS, JS)
├── Program.cs                # Ponto de entrada e configurações
└── appsettings.json          # Configurações gerais
```

## Regras de Negócio

- Estoque é decrementado automaticamente ao finalizar uma venda
- Estoque é devolvido automaticamente ao cancelar uma venda
- Produtos com estoque igual ou menor ao estoque mínimo são sinalizados em vermelho
- Categorias exibem a contagem de produtos vinculados
- Usuário administrador é criado automaticamente (apenas se não existir nenhum usuário)
- Senhas são armazenadas com hash BCrypt (nunca em texto puro)

## Licença

Projeto acadêmico - Tópicos III