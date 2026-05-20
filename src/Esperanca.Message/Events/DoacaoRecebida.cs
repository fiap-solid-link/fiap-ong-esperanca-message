using Esperanca.Message._Shared;

namespace Esperanca.Message.Events;

public record DoacaoRecebida(
    Guid IdDoacao,
    Guid IdCampanha,
    Guid IdDoador,
    string ApelidoDoador,
    decimal Valor,
    DateTime DataIntencao,
    Guid IdempotencyKey) : IEventMessage;

