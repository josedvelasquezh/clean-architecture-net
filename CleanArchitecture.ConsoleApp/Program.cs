using CleanArchitecture.Data;
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

StreamerDbContext dbContext = new();

//await AddNewRecords();
//QueryStreaming();
//await QueryFilter();
//await QueryMethods();
//await QueryLinq();
//await TrackingAndNotTracking();
//await AddNewStreamerWithVideo();
//await AddNewStreamerWithideoId();
//await AddNewActorWithVideo();
//await AddNewDirectorWithVideo();
await MultipleEntitiesQuery();

Console.WriteLine("Presione cualquier tecla para terminar el programa");
Console.ReadKey();

async Task MultipleEntitiesQuery()
{
    Video? videoWithActores = await dbContext!.Videos!.Include(q => q.Actores).FirstOrDefaultAsync(q => q.Id == 1);

    List<string?> actor = await dbContext.Actores.Select(q => q.Name).ToListAsync();

    var videoWithDirector = await dbContext!.Videos!
                            .Where(q => q.Director != null)
                            .Include(q => q.Director)
                            .Select(q =>
                               new
                               {
                                   Director_Nombre_Completo = $"{q.Director.Name} {q.Director.LastName}",
                                   Movie = q.Name
                               }
                             )
                            .ToListAsync();

}


async Task AddNewDirectorWithVideo()
{

    Director director = new Director
    {
        Name = "Lorenzo",
        LastName = "Basteri",
        VideoId = 1
    };

    await dbContext.AddAsync(director);
    await dbContext.SaveChangesAsync();
}

async Task AddNewActorWithVideo()
{
    Actor actor = new Actor
    {
        Name = "Brad",
        LastName = "Pitt"
    };

    await dbContext.AddAsync(actor);
    await dbContext.SaveChangesAsync();

    VideoActor videoActor = new VideoActor
    {
        ActorId = actor.Id,
        VideoId = 1
    };

    await dbContext.AddAsync(videoActor);
    await dbContext.SaveChangesAsync();
}

async Task AddNewStreamerWithideoId()
{

    Video movie = new Video
    {
        Name = "Batman Forever",
        StreamerId = 1002
    };

    await dbContext.AddAsync(movie);
    await dbContext.SaveChangesAsync();
}


async Task AddNewStreamerWithVideo()
{

    Streamer streamer = new Streamer
    {
        Name = "HBO",
        Url = "http://hbo.com"
    };

    Video movie = new Video
    {
        Name = "Juegos del Hambre",
        Streamer = streamer
    };

    await dbContext.AddAsync(movie);
    await dbContext.SaveChangesAsync();
}


async Task TrackingAndNotTracking()
{
    Streamer? streameWithTracking = await dbContext!.Streamers!.FirstOrDefaultAsync(x => x.Id == 1);
    Streamer? streameWithNotTracking = await dbContext!.Streamers!.AsNoTracking().FirstOrDefaultAsync(x => x.Id == 2);

    streameWithTracking.Name = "Netflix Supers";
    streameWithNotTracking.Name = "Amazon Plus";

    await dbContext!.SaveChangesAsync();
}

async Task QueryLinq()
{
    Console.WriteLine("Escribe nombre");
    string? streamerName = Console.ReadLine();

    //List<Streamer> streamers = await (from i in dbContext.Streamers select i).ToListAsync();
    List<Streamer> streamers = await (from i in dbContext.Streamers where EF.Functions.Like(i.Name, $"%{streamerName}%") select i).ToListAsync();

    foreach (Streamer streamer in streamers)
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