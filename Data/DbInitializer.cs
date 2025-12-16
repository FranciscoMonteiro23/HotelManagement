using HotelManagement.Models;
using System;
using System.Linq;

namespace HotelManagement.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HotelContext context)
        {
            // Garantir que a base de dados foi criada
            context.Database.EnsureCreated();

            // Verificar se já existem dados
            if (context.Cliente.Any())
            {
                return; // BD já foi populada
            }

            // ===== CLIENTES =====
            var clientes = new Cliente[]
            {
                new Cliente
                {
                    Nome = "João",
                    Apelido = "Silva",
                    Documento = "123456789",
                    Telefone = "912345678",
                    Email = "joao@email.com",
                    DataCadastro = DateTime.Parse("2024-01-01")
                },
                new Cliente
                {
                    Nome = "Maria",
                    Apelido = "Santos",
                    Documento = "987654321",
                    Telefone = "913456789",
                    Email = "maria@email.com",
                    DataCadastro = DateTime.Parse("2024-01-15")
                },
                new Cliente
                {
                    Nome = "Pedro",
                    Apelido = "Costa",
                    Documento = "456789123",
                    Telefone = "914567890",
                    Email = "pedro@email.com",
                    DataCadastro = DateTime.Parse("2024-02-01")
                },
                new Cliente
                {
                    Nome = "Ana",
                    Apelido = "Ferreira",
                    Documento = "789123456",
                    Telefone = "915678901",
                    Email = "ana@email.com",
                    DataCadastro = DateTime.Parse("2024-02-15")
                }
            };

            foreach (var c in clientes)
            {
                context.Cliente.Add(c);
            }
            context.SaveChanges();

            // ===== QUARTOS =====
            var quartos = new Quarto[]
            {
                new Quarto
                {
                    QuartoID = 101,
                    TipoQuarto = "Single",
                    Capacidade = 1,
                    PrecoPorNoite = 50.00M,
                    Status = StatusQuarto.Disponivel,
                    Descricao = "Quarto simples com cama de solteiro"
                },
                new Quarto
                {
                    QuartoID = 102,
                    TipoQuarto = "Double",
                    Capacidade = 2,
                    PrecoPorNoite = 75.00M,
                    Status = StatusQuarto.Disponivel,
                    Descricao = "Quarto duplo com duas camas"
                },
                new Quarto
                {
                    QuartoID = 201,
                    TipoQuarto = "Suite",
                    Capacidade = 2,
                    PrecoPorNoite = 150.00M,
                    Status = StatusQuarto.Disponivel,
                    Descricao = "Suite luxuosa com vista mar"
                },
                new Quarto
                {
                    QuartoID = 202,
                    TipoQuarto = "Family",
                    Capacidade = 4,
                    PrecoPorNoite = 120.00M,
                    Status = StatusQuarto.Disponivel,
                    Descricao = "Quarto familiar espaçoso"
                },
                new Quarto
                {
                    QuartoID = 301,
                    TipoQuarto = "Presidential",
                    Capacidade = 4,
                    PrecoPorNoite = 300.00M,
                    Status = StatusQuarto.Disponivel,
                    Descricao = "Suite presidencial premium"
                }
            };

            foreach (var q in quartos)
            {
                context.Quarto.Add(q);
            }
            context.SaveChanges();

            // ===== SETORES =====
            var setores = new Setor[]
            {
                new Setor
                {
                    SetorID = 1,
                    NomeSetor = "Recepção",
                    Descricao = "Atendimento ao cliente",
                    NumeroFuncionarios = 5
                },
                new Setor
                {
                    SetorID = 2,
                    NomeSetor = "Limpeza",
                    Descricao = "Manutenção dos quartos",
                    NumeroFuncionarios = 8
                },
                new Setor
                {
                    SetorID = 3,
                    NomeSetor = "Administração",
                    Descricao = "Gestão do hotel",
                    NumeroFuncionarios = 3
                },
                new Setor
                {
                    SetorID = 4,
                    NomeSetor = "Segurança",
                    Descricao = "Vigilância e proteção",
                    NumeroFuncionarios = 4
                }
            };

            foreach (var s in setores)
            {
                context.Setor.Add(s);
            }
            context.SaveChanges();

            // ===== FUNCIONÁRIOS =====
            var funcionarios = new Funcionario[]
            {
                new Funcionario
                {
                    Nome = "Carlos",
                    Apelido = "Mendes",
                    Documento = "111222333",
                    Telefone = "916789012",
                    Email = "carlos@hotel.com",
                    Cargo = CargoFuncionario.Recepcionista,
                    DataContratacao = DateTime.Parse("2023-06-01")
                },
                new Funcionario
                {
                    Nome = "Luisa",
                    Apelido = "Pereira",
                    Documento = "444555666",
                    Telefone = "917890123",
                    Email = "luisa@hotel.com",
                    Cargo = CargoFuncionario.Camareira,
                    DataContratacao = DateTime.Parse("2023-06-15")
                },
                new Funcionario
                {
                    Nome = "Ricardo",
                    Apelido = "Alves",
                    Documento = "777888999",
                    Telefone = "918901234",
                    Email = "ricardo@hotel.com",
                    Cargo = CargoFuncionario.Gerente,
                    DataContratacao = DateTime.Parse("2023-01-01")
                }
            };

            foreach (var f in funcionarios)
            {
                context.Funcionario.Add(f);
            }
            context.SaveChanges();

            // ===== RESERVAS =====
            var reservas = new Reserva[]
            {
                new Reserva
                {
                    QuartoID = 101,
                    ClienteID = 1,
                    DataCheckIn = DateTime.Parse("2024-12-10"),
                    DataCheckOut = DateTime.Parse("2024-12-12"),
                    NumeroHospedes = 1,
                    ValorTotal = 100.00M,
                    Status = StatusReserva.Confirmada,
                    DataReserva = DateTime.Parse("2024-12-01"),
                    Observacoes = "Cliente preferencial"
                },
                new Reserva
                {
                    QuartoID = 201,
                    ClienteID = 2,
                    DataCheckIn = DateTime.Parse("2024-12-15"),
                    DataCheckOut = DateTime.Parse("2024-12-20"),
                    NumeroHospedes = 2,
                    ValorTotal = 750.00M,
                    Status = StatusReserva.Confirmada,
                    DataReserva = DateTime.Parse("2024-12-05"),
                    Observacoes = "Lua de mel"
                },
                new Reserva
                {
                    QuartoID = 202,
                    ClienteID = 3,
                    DataCheckIn = DateTime.Parse("2024-12-20"),
                    DataCheckOut = DateTime.Parse("2024-12-25"),
                    NumeroHospedes = 4,
                    ValorTotal = 600.00M,
                    Status = StatusReserva.Pendente,
                    DataReserva = DateTime.Parse("2024-12-08"),
                    Observacoes = "Família com crianças"
                }
            };

            foreach (var r in reservas)
            {
                context.Reserva.Add(r);
            }
            context.SaveChanges();

            // ===== ATRIBUIÇÃO DE SETORES =====
            var atribuicoes = new AtribuicaoSetor[]
            {
                new AtribuicaoSetor { FuncionarioID = 1, SetorID = 1 }, // Carlos -> Recepção
                new AtribuicaoSetor { FuncionarioID = 2, SetorID = 2 }, // Luisa -> Limpeza
                new AtribuicaoSetor { FuncionarioID = 3, SetorID = 3 }, // Ricardo -> Administração
                new AtribuicaoSetor { FuncionarioID = 3, SetorID = 1 }  // Ricardo -> Recepção também
            };

            foreach (var a in atribuicoes)
            {
                context.AtribuicaoSetor.Add(a);
            }
            context.SaveChanges();
        }
    }
}