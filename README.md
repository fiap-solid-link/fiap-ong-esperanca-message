# Esperanca.Message

[![NuGet](https://img.shields.io/nuget/v/Esperanca.Message.svg)](https://www.nuget.org/packages/Esperanca.Message/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Esperanca.Message.svg)](https://www.nuget.org/packages/Esperanca.Message/)

Contratos de mensageria (comandos e eventos) do domínio **OngEsperança**, usados para integração entre os serviços da plataforma.

Esta lib **não contém lógica de negócio** — apenas os contratos compartilhados (DTOs/records) que produtores e consumidores precisam conhecer para se comunicar via barramento de mensagens.

## Instalação

```bash
dotnet add package Esperanca.Message
```

Ou no `.csproj`:

```xml
<PackageReference Include="Esperanca.Message" Version="1.0.0" />
```

## Estrutura

A lib expõe duas categorias de contratos, marcadas por interfaces:

- `ICommandMessage` — intenção de executar uma ação (imperativo: *"faça X"*).
- `IEventMessage` — fato que já aconteceu (passado: *"X aconteceu"*).

```
Esperanca.Message
├── _Shared
│   ├── ICommandMessage      // marker interface para comandos
│   └── IEventMessage        // marker interface para eventos
└── Events
    ├── DoacaoRecebida       // doação recém-registrada
    └── DoacaoProcessadaEvent // doação confirmada/processada
```

## Eventos disponíveis

### `DoacaoRecebida`

Publicado quando uma nova doação é registrada e está aguardando processamento.

```csharp
public record DoacaoRecebida(
    Guid IdDoacao,
    Guid IdCampanha,
    Guid IdDoador,
    decimal Valor,
    string IdempotencyKey,
    DateTime OcorridoEm) : IEventMessage;
```

### `DoacaoProcessadaEvent`

Publicado após a doação ser efetivamente processada (ex: pagamento confirmado).

```csharp
public record DoacaoProcessadaEvent(
    Guid IdDoacao,
    Guid IdCampanha,
    decimal Valor) : IEventMessage;
```

## Exemplo de uso

**Produtor** (publica o evento):

```csharp
using Esperanca.Message.Events;

var evento = new DoacaoRecebida(
    IdDoacao: Guid.NewGuid(),
    IdCampanha: campanhaId,
    IdDoador: doadorId,
    Valor: 150.00m,
    IdempotencyKey: Guid.NewGuid().ToString(),
    OcorridoEm: DateTime.UtcNow);

await bus.PublishAsync(evento);
```

**Consumidor** (reage ao evento):

```csharp
using Esperanca.Message.Events;

public class DoacaoRecebidaHandler
{
    public async Task HandleAsync(DoacaoRecebida evento, CancellationToken ct)
    {
        // processar a doação...
    }
}
```

## Versionamento

Este pacote segue [SemVer](https://semver.org/lang/pt-BR/):

- **MAJOR** — mudança que quebra contrato existente (rename de campo, remoção, mudança de tipo).
- **MINOR** — adição de novo evento/comando ou novo campo opcional.
- **PATCH** — correções que não afetam o contrato.

> ⚠️ Como esta lib é um contrato compartilhado, **breaking changes exigem coordenação entre todos os serviços que consomem o pacote**. Prefira deprecar em vez de remover.

## Requisitos

- .NET 10.0 ou superior

## Contribuindo

1. Faça fork do repositório
2. Crie uma branch (`git checkout -b feature/novo-evento`)
3. Adicione o contrato em `Events/` ou `Commands/` implementando a interface marker apropriada
4. Abra um Pull Request descrevendo o produtor e os consumidores esperados

## Publicando uma nova versão

Apenas mantenedores. O fluxo é automatizado via GitHub Actions:

```bash
git tag v1.2.3
git push origin v1.2.3
```

A pipeline em `.github/workflows/publish.yml` builda, empacota e publica no NuGet.org automaticamente.

## Licença

[MIT](LICENSE)
