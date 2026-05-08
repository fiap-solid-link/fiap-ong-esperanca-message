using Esperanca.Message._Shared;

namespace Esperanca.Message.Events;

public record DoacaoProcessadaEvent(
    Guid IdDoacao,
    Guid IdCampanha,
    decimal Valor,
    DateTime DataProcessamento) : IEventMessage;
