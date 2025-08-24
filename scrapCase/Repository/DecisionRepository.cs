using Npgsql;
using scrapCase.Models;

namespace scrapCase.Repository;

public class DecisionRepository : IDecisionRepository
{
    private readonly string _connectionString;

    public DecisionRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddDecisionAsync(Decision decision)
    {
        Console.WriteLine("AddDecision started id: " + decision.CaseId);

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        Console.WriteLine("Database connection opened.");

        var checkSql = "SELECT 1 FROM public.decisions WHERE case_id = @case_id";
        await using (var checkCmd = new NpgsqlCommand(checkSql, conn))
        {
            checkCmd.Parameters.AddWithValue("case_id", decision.CaseId);
            var exists = await checkCmd.ExecuteScalarAsync();

            if (exists != null)
            {
                Console.WriteLine($"Decision with id {decision.CaseId} already exists, skipping insert.");
                return; 
            }
        }

        var insertSql = @"
INSERT INTO public.decisions(case_id, decision_datetime, decision_text)
VALUES(@case_id, @decision_datetime, @decision_text)
RETURNING case_id;";

        await using var cmd = new NpgsqlCommand(insertSql, conn);
        cmd.Parameters.AddWithValue("case_id", decision.CaseId);
        cmd.Parameters.AddWithValue("decision_datetime", decision.DateTime);
        cmd.Parameters.AddWithValue("decision_text", decision.DecisionText);

        var insertedId = await cmd.ExecuteScalarAsync();
        Console.WriteLine($"Inserted case_id: {insertedId}");


    }

   

    public async Task<Decision?> GetByIdAsync(string id)
    {
        Console.WriteLine($"GetByIdAsync started for id: {id}");

        await using var conn = new NpgsqlConnection(_connectionString);
        Console.WriteLine("Opening database connection...");
        await conn.OpenAsync();
        Console.WriteLine("Database connection opened.");

        var sql = @"SELECT case_id, decision_datetime, decision_text 
            FROM public.decisions 
            WHERE case_id = @case_id";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("case_id", id); 
        Console.WriteLine($"SQL command prepared for case_id: {id}");

        await using var reader = await cmd.ExecuteReaderAsync();
        Console.WriteLine("SQL command executed, reading results...");

        if (await reader.ReadAsync())
        {
            Console.WriteLine($"Record found for id: {id}");
            return new Decision
            {
                CaseId = reader.GetString(0),
                DateTime = reader.GetDateTime(1),
                DecisionText = reader.GetString(2)
            };
        }

        Console.WriteLine($"No record found for id: {id}");
        return null;


    }

    public async Task<List<Decision>>? GetAllDecisionsAsync()
    {
        var decisions = new List<Decision>();

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"SELECT case_id, decision_datetime, decision_text 
                    FROM public.decisions";

        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            decisions.Add(new Decision
            {
                CaseId = reader.GetString(0),
                DateTime = reader.GetDateTime(1),
                DecisionText = reader.GetString(2)
            });
        }

        return decisions;
    }
}
