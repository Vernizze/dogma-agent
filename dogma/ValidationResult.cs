using System.Text.Json.Serialization;

namespace dogma;

public class ValidationResult
{
    [JsonPropertyName("Id")]
    public string Id { get; set; }

    [JsonPropertyName("Acao")]
    public string Acao { get; set; } // "MANTER" ou "REVOGAR"

    [JsonPropertyName("MemoriaCalculo")]
    public string MemoriaCalculo { get; set; }
}