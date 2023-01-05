using CleanArchitecture.Data;
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

StreamerDbContext dbContext = new();

//await AddNewRecords();
//QueryStreaming();
//await QueryFilter();
//await QueryMethods();
await QueryLinq();

Console.WriteLine("Presione cualquier tecla para terminar el programa");
Console.ReadKey();


async Task QueryLinq()
{
    List<Streamer> streamers = await (from i in dbContext.Streamers select i).ToListAsync();

    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Name}");
    }
}

async Task QueryMethods()
{
    DbSet<Streamer> streamer = dbContext!.Streamers!;

    Streamer firstAsync = await streamer.Where(x => x.Name.Contains("a")).FirstAsync();

    Streamer? firstOrDefaultAsync = await streamer.Where(x => x.Name.Contains("a")).FirstOrDefaultAsync();

    Streamer? FirstOrDefaultAsyncV2 = await streamer.FirstOrDefaultAsync(x => x.Name.Contains("a"));

    Streamer singleAsync = await streamer.Where(x => x.Id == 1).SingleAsync();

    Streamer? singleOrDefaultAsync = await streamer.Where(x => x.Id == 1).SingleOrDefaultAsync();

    Streamer? findAsync = await streamer.FindAsync(1);

    int count = await streamer.CountAsync();
    long longCount = await streamer.LongCountAsync();
    Streamer min = await streamer.MinAsync();
    Streamer max = await streamer.MaxAsync();
}

async Task QueryFilter()
{
    Console.WriteLine("Ingrese una empresa");
    string? streamingName = Console.ReadLine();
    List<Streamer> streamers = await dbContext!.Streamers!.Where(x => x.Name == streamingName).ToListAsync();

    foreach (Streamer streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Name}");
    }

    //List<Streamer> streamerPartialResults = await dbContext!.Streamers!.Where(x => x.Name.Contains(streamingName)).ToListAsync();
    List<Streamer> streamerPartialResults = await dbContext!.Streamers!.Where(x => EF.Functions.Like(x.Name, $"%{streamingName}%")).ToListAsync();

    foreach (Streamer streamer in streamerPartialResults)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Name}");
    }
}

void QueryStreaming()
{
    List<Streamer> streamers = dbContext!.Streamers!.ToList();

    foreach (Streamer streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Name}");
    }
}

async Task AddNewRecords()
{
    Streamer streamer = new()
    {
        Name = "Disney",
        Url = "https://www.disney.com"
    };

    dbContext!.Streamers.Add(streamer);

    await dbContext.SaveChangesAsync();

    List<Video> movies = new()
{
    new Video
    {
        Name = "Pinocho",
        StreamerId  = streamer.Id
    },
    new Video
    {
        Name = "Cenicienta",
        StreamerId  = streamer.Id
    },
    new Video
    {
        Name = "101 Dalmatas",
        StreamerId  = streamer.Id
    },
    new Video
    {
        Name = "Jorobado de Notradame",
        StreamerId  = streamer.Id
    }
};

    await dbContext.AddRangeAsync(movies);

    await dbContext.SaveChangesAsync();
}