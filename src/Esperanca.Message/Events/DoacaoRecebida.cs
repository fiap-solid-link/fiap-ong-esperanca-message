using Esperanca.Message._Shared;

namespace Esperanca.Message.Events;

public record DoacaoRecebida(
    Guid IdDoacao,
    Guid IdCampanha,
    Guid IdDoador,
    decimal Valor,
    string IdempotencyKey,
    DateTime OcorridoEm) : IEventMessage;

