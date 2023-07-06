using System.Text;
using Microsoft.VisualBasic.FileIO;

Console.WriteLine("Converting CSV files to HTML...");

var includesFile = new FileInfo("include.txt");
if (!includesFile.Exists)
{
    Console.WriteLine("include.txt not found, please create it and add the column headers you want to include in the output.");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    return;
}
var includes = File.ReadAllLines(includesFile.FullName).ToList();
Console.WriteLine($"Include: {string.Join(", ", includes)}");

var files = Directory.GetFiles(".\\", "*.csv");
if (!files.Any())
{
    Console.WriteLine("No CSV files found, please place the CSV files in the same folder as this program.");
    if (Console.IsInputRedirected)
    {
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
    return;
}
Console.WriteLine($"Reading {files.Length} CSV files...");

var builder = new StringBuilder();
builder.AppendLine("<!DOCTYPE html>");
builder.AppendLine("<html>");
builder.AppendLine("<head><style>div { break-inside: avoid !important; } p { font-size: 14px; } p.legal { font-size: 11px; }</style></head>");
builder.AppendLine("<body>");
int i = 1;
foreach (string csvFile in files)
{
    Console.WriteLine($"Processing {csvFile}...");
    using var parser = new TextFieldParser(csvFile);
    parser.TextFieldType = FieldType.Delimited;
    parser.SetDelimiters(",");
    parser.HasFieldsEnclosedInQuotes = true;

    if (parser.ReadFields() is not string[] allHeaders) continue;
    var includedHeaders = Column.Parse(allHeaders, includes);

    while (!parser.EndOfData && parser.ReadFields() is string[] allFields)
    {
        var fields = includedHeaders
            .Select(h => Field.Parse(h, allFields))
            .Where(f => !string.IsNullOrWhiteSpace(f.Value))
            .ToList();

        builder.AppendLine("    <div>");
        Field? getField(string header) => fields.FirstOrDefault(f => f.Header.Equals(header, StringComparison.OrdinalIgnoreCase));
        if (getField("Order ID") is Field order && getField("Camper's Name") is Field camper)
        {
            Console.WriteLine($"    {camper} ({order}) #{i}");
            builder.AppendLine($"    <h3>{camper} ({order}) #{i}</h3>");
            fields.Remove(order);
            fields.Remove(camper);
            i++;
        }

        builder.AppendLine("        <p>");
        foreach (var field in fields)
        {
            builder.AppendLine($"            <b>{field.Header} </b><label>{field}</label><br/>");
        }
        builder.AppendLine("        </p>");
        builder.AppendLine("        <br/>");
        builder.AppendLine("        <p class=\"legal\">");
        builder.AppendLine("            Camp Andrews, its agents, employees, affiliates, successors, and Honey Brook Community Church are hereinafter collectively referred to as \"Provider.\"");
        builder.AppendLine("            <br/>In the event that I cannot be reached in an emergency, I give my permission to the physician selected by Provider to hospitalize and secure proper treatment as necessary for my child named above.");
        builder.AppendLine("            <br/>In consideration of permission granted my child to participate in camping and related activities, which I acknowledge are inherently dangerous, I hereby accept unto myself all responsibility and all liability for any injury, death or other claim, loss or damage, caused by, or arising out of camping and other related activities sponsored by Provider.  I hereby release and covenant with Provider that I will never, individually or as legal guardian of my child, institute any action for any injury, death or other claim, loss or damage, caused by, or arising out of camping and other related activities sponsored by Provider.  I further agree to indemnify and hold Provider harmless against any and all claims, demands, actions, and causes of action (including actual attorneys' fees, costs and expenses) of my child or my child's legal guardian that may arise as a result of my child's participation in camping and other related activities sponsored by Provider.");
        builder.AppendLine("            <br/>I voluntarily agree to assume all of the foregoing risks and accept sole responsibility for any injury to my child(ren) or myself (including, but not limited to, personal injury, disability, and death), illness, damage, loss, claim, liability, or expense, of any kind, that I or my child(ren) may experience or incur in connection with my child(ren)’s attendance at the 2020 Camp Andrews or participation in the 2020 Camp Andrews programming (“Claims”). On my behalf, and on behalf of my children, I hereby release, covenant not to sue, discharge, and hold harmless Provider, its employees, agents, and representatives, of and from the Claims, including all liabilities, claims, actions, damages, costs or expenses of any kind arising out of or relating thereto. I understand and agree that this release includes any Claims based on the actions, omissions, or negligence of the Club, its employees, agents, and representatives, whether a COVID-19 infection occurs before, during, or after participation in any of the Provider's programs.");
        builder.AppendLine("            <br/>I hereby give my permission for any photography of my child on camp premises to be used in publicity for Provider.");
        builder.AppendLine("            <br/><br/><br/>Intending to be legally bound,");
        builder.AppendLine("        </p>");
        builder.AppendLine("        <b>Signature of Parent/Guardian:</b>  ____________________________");
        builder.AppendLine("        <br/>Date:  ____________");
        builder.AppendLine("        <br/>Birthdate:____________"); 
        builder.AppendLine("    </div>");
        builder.AppendLine("    <br/>");
    }
}

builder.AppendLine("</body>");
builder.AppendLine("</html>");
File.WriteAllText("output.html", builder.ToString());

Console.WriteLine("Done, see output.html for results.");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

record Column(string Header, int Index, int Order)
{
    public static Column[] Parse(string[] columnHeaders, List<string> include)
    {
        return columnHeaders
            .Select((h, i) => new Column(h, i, include.FindIndex(i => i.Equals(h, StringComparison.OrdinalIgnoreCase))))
            .Where(c => c.Order >= 0)
            .OrderBy(c => c.Order)
            .ToArray();
    }
}

record Field(string Header, string Value)
{
    public static Field Parse(Column column, string[] allFields)
    {
        var header = column.Header.Replace("Product Form: ", "");
        var value = allFields[column.Index];
        if (value.Contains('\n'))
            value = $"<br/>{value.Replace("\n", "<br/>").Replace("\r", "")}";

        return new Field(header, value);
    }
    public override string ToString() => Value;
}