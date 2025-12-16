# 🏨 Hotel Management System

Sistema completo de gestão hoteleira desenvolvido em ASP.NET Core com Razor Pages.

![Screenshot](https://img.shields.io/badge/ASP.NET%20Core-10.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

## 📋 Funcionalidades

- ✅ **Gestão de Reservas** - Sistema completo de reservas com check-in/check-out
- ✅ **Gestão de Quartos** - Controlo de disponibilidade e manutenção
- ✅ **Gestão de Clientes** - Base de dados de clientes
- ✅ **Gestão de Funcionários** - Controlo de colaboradores e setores
- ✅ **Relatórios** - Estatísticas e relatórios de ocupação
- ✅ **Autenticação** - Sistema de login com ASP.NET Identity

## 🛠️ Tecnologias Utilizadas

- **ASP.NET Core 10.0** - Framework principal
- **Razor Pages** - Interface do utilizador
- **Entity Framework Core** - ORM
- **SQLite** - Base de dados
- **Bootstrap 5** - Design responsivo
- **Identity Framework** - Autenticação e autorização

## 🚀 Como Executar
```bash
# Clone o repositório
git clone https://github.com/FranciscoMonteiro23/HotelManagement.git

# Entre na pasta do projeto
cd HotelManagement

# Restaure as dependências
dotnet restore

# Execute a aplicação
dotnet run --urls "http://localhost:5110"
```

Aceda a aplicação em: `http://localhost:5110`

## 📦 Requisitos

- .NET 10.0 SDK ou superior
- Visual Studio Code (recomendado)

## 🗃️ Estrutura do Projeto
```
HotelManagement/
├── Areas/Identity/     # Páginas de autenticação
├── Data/              # Contextos e DbInitializer
├── Models/            # Modelos de dados
├── Pages/             # Razor Pages
│   ├── Clientes/      # Gestão de clientes
│   ├── Funcionarios/  # Gestão de funcionários
│   ├── Quartos/       # Gestão de quartos
│   ├── Reservas/      # Gestão de reservas
│   └── Relatorios/    # Relatórios
├── wwwroot/           # Ficheiros estáticos
└── Program.cs         # Configuração da aplicação
```

## 👤 Autor

**Francisco Monteiro**
- GitHub: [@FranciscoMonteiro23](https://github.com/FranciscoMonteiro23)

## 📄 Licença

Este projeto está sob a licença MIT.

## 🤝 Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.

---

⭐ Se este projeto foi útil, deixe uma estrela!
