# Ambiente Web - Tópicos III

Aplicação web MVC para gerenciamento de produtos, desenvolvida com ASP.NET Core e Entity Framework Core.

## Funcionalidades

- **CRUD de Produtos**: Cadastro, listagem, edição, exclusão e detalhes de produtos
- **Tema Claro/Escuro**: Alternância de tema com persistência no localStorage
- **Sidebar Responsiva**: Menu lateral colapsável para desktop e drawer para mobile
- **Design com Tailwind CSS**: Interface moderna e responsiva

## Tecnologias

- ASP.NET Core 10.0 (MVC)
- Entity Framework Core 10.0 (SQL Server)
- Tailwind CSS (via CDN)
- Bootstrap Icons

## Pré-requisitos

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server (local ou Docker)
- Visual Studio Code ou Visual Studio 2022+

## Configuração

1. Clone o repositório:
```bash
git clone <url-do-repositorio>
cd topicosIII/ambiente-web
```

2. Configure a string de conexão em `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TopicosWebDb;User Id=sa;Password=<sua-senha>;TrustServerCertificate=True;"
  }
}
```

3. Aplique as migrações do banco de dados:
```bash
dotnet ef database update
```

4. Execute o projeto:
```bash
dotnet run
```

5. Acesse: `https://localhost:5001` ou `http://localhost:5000`

## Estrutura do Projeto

```
ambiente-web/
├── Controllers/
│   ├── HomeController.cs      # Controller padrão (Home, Privacy, Error)
│   └── ProdutosController.cs  # CRUD de Produtos
├── Data/
│   └── AppDbContext.cs        # Contexto do Entity Framework
├── Models/
│   ├── Produto.cs             # Modelo de Produto (Id, Nome, Preco)
│   ├── ErrorViewModel.cs      # Modelo de erro
│   └── HomeViewModel.cs       # ViewModel da Home
├── Views/
│   ├── Home/                  # Views da Home
│   ├── Produtos/              # Views CRUD de Produtos
│   └── Shared/
│       └── _Layout.cshtml     # Layout principal com sidebar e tema
├── wwwroot/
│   ├── css/site.css           # Estilos customizados
│   └── js/site.js             # JavaScript do projeto
├── Migrations/                # Migrações do EF Core
└── Program.cs                 # Ponto de entrada da aplicação
```

## Modelo de Dados

### Produto
| Campo  | Tipo    | Descrição         |
|--------|---------|-------------------|
| Id     | int     | Identificador único |
| Nome   | string  | Nome do produto   |
| Preco  | decimal | Preço do produto  |

## Observações

- O tema claro/escuro utiliza a estratégia de classes do Tailwind CSS (`darkMode: 'class'`)
- A preferência de tema é salva no `localStorage` do navegador
- A sidebar inicia colapsada e pode ser expandida pelo botão hamburguer
