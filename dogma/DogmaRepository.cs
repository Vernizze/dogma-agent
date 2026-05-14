using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace dogma;

public class DogmaRepository
{
    private readonly string _basePath = "DogmaStorage/Active";

    public void Save(DogmaNode node)
    {
        var sb = new StringBuilder();
        sb.AppendLine("---");
        sb.AppendLine($"id: {node.Id}");
        sb.AppendLine($"name: \"{node.Name}\"");
        sb.AppendLine($"parentId: {node.ParentId ?? "null"}");
        sb.AppendLine($"status: {node.Status}");
        sb.AppendLine($"nature: \"{node.Nature}\"");
        sb.AppendLine($"createdAt: {node.CreatedAt:yyyy-MM-dd}");
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine($"# {node.Name}");
        sb.AppendLine(node.Content);

        var filePath = Path.Combine(_basePath, $"{node.Id}.md");
        File.WriteAllText(filePath, sb.ToString());
    }

    public string GetAllActiveContent()
    {
        var files = Directory.GetFiles(_basePath, "*.md");
        var combined = new StringBuilder();
        foreach (var file in files)
        {
            combined.AppendLine(File.ReadAllText(file));
            combined.AppendLine("---");
        }
        return combined.ToString();
    }

    public List<DogmaNode> LoadAllActive()
    {
        var dogmas = new List<DogmaNode>();
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        foreach (var file in Directory.GetFiles("DogmaStorage/Active", "*.md"))
        {
            var content = File.ReadAllText(file);
            // Separa o Frontmatter (entre ---) do conteúdo Markdown
            var parts = content.Split("---", StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 2)
            {
                var node = deserializer.Deserialize<DogmaNode>(parts[0]);
                node.Content = parts[1].Trim();
                dogmas.Add(node);
            }
        }
        return dogmas;
    }
}